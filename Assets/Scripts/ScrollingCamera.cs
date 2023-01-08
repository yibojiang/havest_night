using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCamera : MonoBehaviour
{
    public float cameraSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        var movingVector = new Vector3(cameraSpeed, 0.0f, 0.0f);
        transform.position += movingVector * Time.deltaTime;
    }
}
