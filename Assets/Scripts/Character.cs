#define DEBUG_CHARACTER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpType
{
    SmallJump = 0,
    BigJump = 1
}

public class Character : MonoBehaviour
{
    public static float gravity = 30.0f;

    public float targetXSpeed = 0.0f;
    public float xSpeed = 1.0f;
    protected float ySpeed = 0.0f;

    public float stamina = 10;

    public float runSpeed = 1.0f;
    public float slowRunSpeed = 0.6f;
    public float drag = 1.0f;
    
    public float jumpSpeed = 10.0f;
    public float bigJumpSpeed = 15.0f;
    
    public float sprintSpeed = 1.0f;

    public float sprintTimer = 0.0f;
    public float sprintDuration = 0.5f;

    public Animator animator;

    public CharacterController controller;
    
    public int currentLane = 0;

    public bool alive = true;

    public float fireRingInvincibleYSpeedRange = 3.0f;
    
    public float standCapsuleHeight = 1.74f;
    
    public float jumpCapsuleHeight = 1.0f;
    
    public float bigJumpCapsuleHeight = 0.5f;

    public JumpType jumpType = JumpType.SmallJump;

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
        targetXSpeed = xSpeed;
        controller.height = standCapsuleHeight;
    }

    void UpdateLaneCollision()
    {
        Collider characterCollider = GetComponent<Collider>();
        ySpeed = 0.0f;
        
        // Ignore the collision between character and other lane
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

        // Update character position to the target lane
        Vector3 targetPosition = new Vector3(transform.position.x, LaneManager.instance.Lanes[currentLane].collider.transform.position.y + 0.5f, transform.position.z);
        characterCollider.enabled = false;
        transform.position = targetPosition;
        characterCollider.enabled = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        animator.SetFloat("xSpeed", xSpeed);
        animator.SetFloat("ySpeed", ySpeed);
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetBool("IsAir", !controller.isGrounded);
        animator.SetBool("IsLanding", IsInvincibleToFireRing() == false && ySpeed < 0);

        sprintTimer -= Time.deltaTime;
        if (sprintTimer < 0)
        {
            sprintTimer = 0.0f;
            targetXSpeed = runSpeed;
            animator.speed = 1.0f;
        }
    }

    protected void FixedUpdate()
    {
        // Moving along the x axis
        var movingVector = new Vector3(xSpeed, ySpeed, 0.0f);
        CollisionFlags collisionFlags = controller.Move(movingVector * Time.fixedDeltaTime);

        // Moving along the y axis
        if (controller.isGrounded == false)
        {
            ySpeed -= gravity * Time.fixedDeltaTime;
        }

        if (controller.isGrounded)
        {
            // Apply drag after sprint to normal speed
            if (targetXSpeed < xSpeed)
            {
                xSpeed -= drag * Time.deltaTime;
            }
            else
            {
                xSpeed = targetXSpeed;
            }
        }
        
        if (IsInvincibleToFireRing())
        {
            if (jumpType == JumpType.SmallJump)
            {
                controller.height = jumpCapsuleHeight;
            }
            else
            {
                controller.height = bigJumpCapsuleHeight;
            }
            
        }
        else
        {
            controller.height = standCapsuleHeight;
        }
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            if (xSpeed > runSpeed)
            {
                jumpType = JumpType.BigJump;
                // Also apply sprint speed when big jump
                animator.speed = 1.0f;
                ySpeed = bigJumpSpeed;
                animator.SetTrigger("BigJump");
            }
            else
            {
                jumpType = JumpType.SmallJump;
                animator.speed = 1.0f;
                ySpeed = jumpSpeed;
                animator.SetTrigger("Jump");
            }
        }
    }
    public void Sprint()
    {
        if (controller.isGrounded)
        {
            sprintTimer = sprintDuration;
            xSpeed = sprintSpeed;
            targetXSpeed = sprintSpeed;
            animator.speed = sprintSpeed / runSpeed;
        }
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
        GUILayout.Label($"alive {alive}");
        GUILayout.Label($"invincible {IsInvincibleToFireRing()}");
#endif
    }

    public bool IsInvincibleToFireRing()
    {
        return Mathf.Abs(ySpeed) < fireRingInvincibleYSpeedRange;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireRing"))
        {
            if (IsInvincibleToFireRing() == false)
            {
                animator.SetBool("Die", true);
                animator.SetBool("DieOnFire", true);
                Die();
            }
        }

        if (other.CompareTag("SlaughterMachine"))
        {
            animator.SetBool("Die", true);
            animator.SetBool("DieOnSlaughter", true);
            Die();
        }
    }
    public void Die()
    {
        alive = false;
        targetXSpeed = 0.0f;
        xSpeed = 0.0f;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
