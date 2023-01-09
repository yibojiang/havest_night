using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : Controller
{
    private System.Random random;

    private float randomActionTimer;
    private float randomActionInterval = 2.5f;
    private float randomActionIntervalOff = 2.0f;

    private float respawnTimer;
    private float respawnInterval = 5.0f;
    private float respawnIntervalOff = 3.0f;

    protected void Awake()
    {
        respawnTimer = respawnInterval + Random.Range(0.0f, respawnIntervalOff);
        randomActionTimer = randomActionInterval + Random.Range(0.0f, randomActionIntervalOff);
    }

    // Start is called before the first frame update
    void Start()
    {
        // playerColor = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
        // SpawnPlayer(Random.Range(0, LaneManager.instance.Lanes.Length));
        // status = PlayerStatus.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if (status == PlayerStatus.Alive)
        {
            if (character.alive == false)
            {
                status = PlayerStatus.Dead;
                return;
            }
            
            // Apply some action on ai randomly between intervals
            randomActionTimer -= Time.deltaTime;
            if (randomActionTimer < 0.0f)
            {
                RandomAction();
                randomActionTimer = randomActionInterval + Random.Range(0.0f, randomActionIntervalOff);
            }
        }

        if (status == PlayerStatus.Dead || status == PlayerStatus.Unborn)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer < 0.0f)
            {
                if (character)
                {
                    Destroy(character.gameObject);
                }
                playerColor = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
                status = PlayerStatus.Alive;
                SpawnPlayer(Random.Range(0, LaneManager.instance.Lanes.Length));
                respawnTimer = respawnInterval + Random.Range(0.0f, respawnIntervalOff);
            }
        }
    }

    void RandomAction()
    {
        int roll = UnityEngine.Random.Range(0, 100);
        if (roll < 25)
        {
            character.Jump();
        }
        else if (roll < 50)
        {
            character.ChangeLaneUp();
        }
        else if (roll < 75)
        {
            character.ChangeLaneDown();
        }
        else
        {
            character.Sprint();
        }
    }

    void FixedUpdate()
    {
        if (status == PlayerStatus.Alive)
        {
            int triggerLayerMask = 1 << LayerMask.NameToLayer("Trigger");
            RaycastHit[] hits;
            
            // Raycast against the slaughter machine
            hits = Physics.RaycastAll(character.transform.position, character.transform.position + new Vector3(-4.0f, 0, 0), Mathf.Infinity,
                triggerLayerMask);

            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit.collider.CompareTag("SlaughterMachine"))
                {
                    character.Sprint();
                }
            }
            
            // Raycast against the trap
            hits = Physics.RaycastAll(character.transform.position, character.transform.position + new Vector3(0.5f, 0, 0), Mathf.Infinity,
                triggerLayerMask);
            
            int roll = UnityEngine.Random.Range(0, 100);
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit.collider.CompareTag("Banana_Used"))
                {
                    if (roll < 35)
                    {
                        character.Jump();
                    }
                    else if (roll < 79)
                    {
                        character.ChangeLaneUp();
                    }
                    else
                    {
                        character.ChangeLaneDown();
                    }
                }
            }
        }
    }
    
    protected override void SpawnPlayer(int inLaneId)
    {
        var laneId = inLaneId;
        Vector3 lanePosition = LaneManager.instance.Lanes[laneId].collider.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;
        GameObject newPlayer = Instantiate(characterPrefab, new Vector3(cameraPosition.x + 10.0f, lanePosition.y + 0.7f, lanePosition.z), Quaternion.identity);
        character = newPlayer.GetComponent<Character>();
        character.currentLane = laneId;
        character.runSpeed = character.runSpeed * 0.7f;
        character.GetComponent<SpriteRenderer>().color = playerColor;
        
        // GameObject playerText = Instantiate(characterTextPrefab, Vector3.zero, Quaternion.identity);
        // playerText.transform.SetParent(newPlayer.transform);
        // playerText.transform.localPosition = new Vector3(0, 2, 0);
        // playerText.transform.localScale = new Vector3(1, 1, 1);
        // var textComponponent = playerText.GetComponent<TextMeshPro>();
        // textComponponent.text = $"COM";
        // textComponponent.color = playerColor;
    }

}
