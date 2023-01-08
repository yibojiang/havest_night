using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Character character;

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

    void FixedUpdate()
    {
        if (!character)
        {
            return;
        }
        
        if (character.alive == false)
        {
            return;
        }
        
        int triggerLayerMask = 1 << LayerMask.NameToLayer("Trigger");
        RaycastHit[] hits;
        // Raycast against the slaughter machine
        hits = Physics.RaycastAll(character.transform.position, character.transform.position + new Vector3(-50.0f, 0, 0), Mathf.Infinity,
            triggerLayerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (hit.collider.CompareTag("SlaughterMachine"))
            {
                character.Sprint();
            }
        }

    }
}
