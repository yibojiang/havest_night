using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class LaneInfo
{
    public float yPosition;
    public Collider collider;
}

[Serializable]
public class SpawnInfo
{
    public GameObject prefab;
    public float spawnerDuration = 1.5f;
    public float spawnerDurationRandomOff = 0.5f;
}

public class SingletonBehaviour<T> : MonoBehaviour where T: SingletonBehaviour<T>
{
    public static T instance { get; protected set; }
 
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            instance = (T)this;
        }
    }
}

public class LaneManager : SingletonBehaviour<LaneManager>
{
    [SerializeField] public LaneInfo[] Lanes;

    [SerializeField] public SpawnInfo fireRing;
    
    public float fireRingSpawnerTimer;
    
    public float trapSpawnerTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireRingSpawnerTimer += Time.deltaTime;
        if (fireRingSpawnerTimer > fireRing.spawnerDuration)
        {
            SpawnFireRing();
        }
    }

    void SpawnFireRing()
    {
        
    }
}
