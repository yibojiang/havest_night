using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Character character;

    protected void Awake()
    {
        character = GetComponent<Character>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (character.alive == false)
        {
            return;
        }
    }
}
