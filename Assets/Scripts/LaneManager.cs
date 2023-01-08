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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
