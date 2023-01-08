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

    public float stamina = 10;

    public float runSpeed = 1.0f;
    public float slowRunSpeed = 1.0f;
    public float jumpSpeed = 10.0f;
    
    public float sprintSpeed = 1.0f;

    public float sprintTimer = 0.0f;
    public float sprintDuration = 0.5f;

    public Animator animator;

    public CharacterController controller;

    public bool isGrounded;

    public int currentLane = 0;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        UpdateLaneCollision();

        xSpeed = runSpeed;
    }

    void UpdateLaneCollision()
    {
        for (int laneIndex = 0; laneIndex < LaneManager.instance.Lanes.Length; laneIndex++)
        {
            if (currentLane == laneIndex)
            {
                Physics.IgnoreCollision(LaneManager.instance.Lanes[laneIndex].collider, GetComponent<Collider>(), false);
            }
            else
            {
                Physics.IgnoreCollision(LaneManager.instance.Lanes[laneIndex].collider, GetComponent<Collider>(), true);
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        animator.SetFloat("Speed", xSpeed);

        sprintTimer -= Time.deltaTime;
        if (sprintTimer < 0)
        {
            sprintTimer = 0.0f;
            xSpeed = runSpeed;
            animator.speed = 1.0f;
        }
    }

    protected void FixedUpdate()
    {
        
        // Moving along the x axis
        var movingVector = new Vector3(xSpeed, ySpeed, 0.0f);
        CollisionFlags collisionFlags = controller.Move(movingVector * Time.fixedDeltaTime);

        if (collisionFlags == CollisionFlags.Above)
        {
            
        }
        
        // Moving along the y axis
        if (controller.isGrounded == false)
        {
            ySpeed -= gravity * Time.fixedDeltaTime;
            // controller.Move(ySpeed * Time.fixedDeltaTime);
        }
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            ySpeed = jumpSpeed;
            animator.SetTrigger("Jump");
        }
    }

    public void Sprint()
    {
        sprintTimer = sprintDuration;
        xSpeed = sprintSpeed;
        animator.speed = sprintSpeed / runSpeed;
    }

    public void ChangeLaneUp()
    {
        if (currentLane - 1 >= 0)
        {
            currentLane++;
        }
        UpdateLaneCollision();
    }
    
    public void ChangeLaneDown()
    {
        if (currentLane + 1 <= LaneManager.instance.Lanes.Length - 1)
        {
            currentLane++;
        }
        UpdateLaneCollision();
    }

    protected void OnGUI()
    {
#if DEBUG_CHARACTER
        GUILayout.Label($"xSpeed {xSpeed}");
        GUILayout.Label($"ySpeed {ySpeed}");
        GUILayout.Label($"isGrounded {controller.isGrounded}");
#endif
    }
}
