using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    protected Character character;
    
    public GameObject characterPrefab;
    
    public GameObject characterTextPrefab;
    
    public PlayerStatus status = PlayerStatus.Unborn;

    private int laneId = 0;
    
    public Color playerColor = Color.white;
    
    protected virtual void SpawnPlayer(int inLaneId)
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
