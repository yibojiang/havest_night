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
        Collider characterCollider = GetComponent<Collider>();
        ySpeed = 0.0f;
        for (int laneIndex = 0; laneIndex < LaneManager.instance.Lanes.Length; laneIndex++)
        {
            if (currentLane == laneIndex)
            {
                Physics.IgnoreCollision(LaneManager.instance.Lanes[laneIndex].collider, characterCollider, false);
            }
            else
            {
                Physics.IgnoreCollision(LaneManager.instance.Lanes[laneIndex].collider, characterCollider, true);
            }
        }

        Vector3 targetPosition = new Vector3(transform.position.x, LaneManager.instance.Lanes[currentLane].collider.transform.position.y + 0.5f, transform.position.z);
        characterCollider.enabled = false;
        transform.position = targetPosition;
        // controller.Move(targetPosition - transform.position);
        transform.position = targetPosition;
        characterCollider.enabled = true;
        // controller.transform.position = LaneManager.instance.Lanes[currentLane].collider.transform.position +
        //                                 new Vector3(0.0f, 0.5f, 0.0f);
    }

    // Update is called once per frame
    protected void Update()
    {
        animator.SetFloat("Speed", xSpeed);
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetBool("IsAir", !controller.isGrounded);

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
        }
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            animator.speed = 1.0f;
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
            currentLane--;
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
