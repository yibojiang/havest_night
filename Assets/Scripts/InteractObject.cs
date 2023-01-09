using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public int currentLane;
    // Start is called before the first frame update
    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = currentLane;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
