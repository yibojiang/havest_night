#define DEBUG_CHARACTER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float gravity = 30.0f;
    
    public float xSpeed = 1.0f;
    protected float ySpeed = 0.0f;
    public float jumpSpeed = 10.0f;

    public Animator animator;

    public CharacterController controller;

    public int currentLane = 0;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        animator.SetFloat("Speed", xSpeed);
    }

    protected void FixedUpdate()
    {
        
        // Moving along the x axis
        var movingVector = new Vector3(xSpeed, ySpeed, 0.0f);
        CollisionFlags collisionFlags = controller.Move(movingVector * Time.fixedDeltaTime);

        // Console.log("is grounded")

        // Moving along the y axis
        if (controller.isGrounded == false)
        {
            ySpeed -= gravity * Time.fixedDeltaTime;
            // controller.Move(ySpeed * Time.fixedDeltaTime);
        }
    }

    public void Jump()
    {
        if (controller.isGrounded == true)
        {
            ySpeed = jumpSpeed;
            animator.SetTrigger("Jump");
        }
    }

    protected void OnGUI()
    {
#if DEBUG_CHARACTER
        GUILayout.Label($"xSpeed {xSpeed}");
        GUILayout.Label($"ySpeed {ySpeed}");
        GUILayout.Label($"isGrounded {controller.isGrounded}");
        // GUILayout.Label("ySpeed");
#endif
    }
}
