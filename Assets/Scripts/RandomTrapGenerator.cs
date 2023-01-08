using UnityEngine;
using System.Collections.Generic;

class RandomTrapGenerator: MonoBehaviour
{
    [SerializeField] private int poolSize = 20;
    [SerializeField] private GameObject prototype;
    [SerializeField] private GameObject[] lanes;
    [SerializeField] private int maxNumberPerLane;
    [SerializeField] private float coolDown;
    [SerializeField] private float[] laneCoolDown;
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

        inUse = new List<GameObject>[lanes.Length];
        for(int i = 0; i < inUse.Length; i++)
        {
            inUse[i] = new List<GameObject>();
        }

        laneCoolDown = new float[lanes.Length];
        for(int i = 0; i < lanes.Length; i++)
        {
            laneCoolDown[i] = coolDown;
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

            if (inUse[i].Count  < maxNumberPerLane && laneCoolDown[i] > coolDown && available.Count > 0)
            {
                int roll = random.Next(0, spawnRatePerFrameDivider);
                if(roll < spawnRatePerFrame)
                {
                    GameObject newSpawn = available[available.Count - 1];
                    available.RemoveAt(available.Count - 1);
                    inUse[i].Add(newSpawn);

                    Vector3 spawnPosition = new Vector3(Camera.main.transform.position.x + spawnOffset.x, lanes[i].transform.position.y + spawnOffset.y, lanes[i].transform.position.z); ;
                    newSpawn.transform.position = spawnPosition;
                    newSpawn.SetActive(true);
                    laneCoolDown[i] = 0f;
                }
            }
        }
    }
}

