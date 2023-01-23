using UnityEngine;
using System.Collections.Generic;

class RandomTrapGenerator: MonoBehaviour
{
    [SerializeField] private int poolSize = 20;
    [SerializeField] private GameObject prototype;
    [SerializeField] private int maxNumberPerLane;
    [SerializeField] private float coolDown;
    private float[] laneCoolDown;
    [SerializeField] private int spawnRatePerFrame = 33;
    [SerializeField] private int spawnRatePerFrameDivider = 10000;
    [SerializeField] private Vector2 spawnOffset;

    private List<GameObject> available;
    private List<GameObject>[] inUse;
    private System.Random random;

    public void Start()
    {
        random = new System.Random();

        available = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject duplicate = Instantiate(prototype, Vector3.zero, Quaternion.identity);
            duplicate.SetActive(false);
            available.Add(duplicate);
        }

        inUse = new List<GameObject>[LaneManager.instance.Lanes.Length];
        for(int i = 0; i < inUse.Length; i++)
        {
            inUse[i] = new List<GameObject>();
        }

        laneCoolDown = new float[LaneManager.instance.Lanes.Length];
        for(int i = 0; i < LaneManager.instance.Lanes.Length; i++)
        {
            laneCoolDown[i] = Random.Range(0.0f, coolDown);
        }
    }
    public void Update()
    {
        //recycle
        for(int i = 0; i < inUse.Length; i++)
        {
            for(int j = inUse[i].Count - 1; j > -1; j--)
            {
                if(Camera.main.WorldToViewportPoint(inUse[i][j].transform.position).x < 0f)
                {
                    available.Add(inUse[i][j]);
                    inUse[i][j].transform.position = new Vector3(-100, -100, inUse[i][j].transform.position.z);
                    inUse[i][j].SetActive(false);

                    inUse[i].RemoveAt(j);
                }
            }
        }

        //spawn
        for (int i = 0; i < inUse.Length; i++)
        {
            laneCoolDown[i] += Time.deltaTime;

            if (laneCoolDown[i] > coolDown)
            {
                laneCoolDown[i] -= coolDown;
                if (inUse[i].Count  < maxNumberPerLane && available.Count > 0)
                {
                    int roll = random.Next(0, spawnRatePerFrameDivider);
                    if (roll < spawnRatePerFrame)
                    {
                        GameObject newSpawn = available[available.Count - 1];
                        available.RemoveAt(available.Count - 1);
                        inUse[i].Add(newSpawn);

                        Vector3 spawnPosition = new Vector3(Camera.main.transform.position.x + spawnOffset.x, LaneManager.instance.Lanes[i].collider.transform.position.y + spawnOffset.y, LaneManager.instance.Lanes[i].collider.transform.position.z); ;
                        newSpawn.transform.position = spawnPosition;
                        newSpawn.SetActive(true);
                        var InteractObject = newSpawn.GetComponent<InteractObject>();
                        if (InteractObject)
                        {
                            InteractObject.currentLane = i;
                        }

                        newSpawn.GetComponent<SpriteRenderer>().sortingOrder = i;
                    }
                }
            }            
        }
    }
}

