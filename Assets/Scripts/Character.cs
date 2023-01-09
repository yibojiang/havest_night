// #define DEBUG_CHARACTER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpType
{
    SmallJump = 0,
    BigJump = 1
}

public enum CharacterState
{
    Run,
    Jump,
    Attack,
    Hurt,
    Death,
}

public class Character : MonoBehaviour
{
    public static float gravity = 30.0f;
    
    public float xSpeed = 1.0f;
    protected float ySpeed = 0.0f;

    public float stamina = 10;

    public float runSpeed = 1.0f;
    public float slowRunSpeed = 0.6f;
    public float drag = 1.0f;
    
    public float jumpSpeed = 10.0f;
    public float bigJumpSpeed = 15.0f;
    
    public float sprintSpeed = 1.0f;
    public Animator animator;

    public CharacterController controller;
    
    public int currentLane = 0;

    public bool alive = true;

    public float fireRingInvincibleYSpeedRange = 3.0f;
    
    public float standCapsuleHeight = 1.74f;
    
    public float jumpCapsuleHeight = 1.0f;
    
    public float bigJumpCapsuleHeight = 0.5f;

    public JumpType jumpType = JumpType.SmallJump;

    public CharacterState characterState = CharacterState.Run;

    public float attackTimer = 0.0f;
    
    public float attackDuration = 0.2f;

    public float hurtTimer = 0.0f;
    
    public float hurtDuration = 0.4f;

    private float xSpeedMultiplier = 1.0f;

    private SpriteRenderer sprite;

    public Controller playerController;

    
    [SerializeField]
    public Collider attackBox;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        sprite = GetComponent<SpriteRenderer>();

        if (attackBox)
        {
            attackBox.enabled = false;
            Physics.IgnoreCollision(controller, attackBox, true);
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        UpdateLaneCollision();

        xSpeed = runSpeed;
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
        sprite.sortingOrder = currentLane;
    }

    // Update is called once per frame
    protected void Update()
    {
        animator.SetFloat("xSpeed", xSpeed);
        animator.SetFloat("ySpeed", ySpeed);
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetBool("IsAir", !controller.isGrounded);
        animator.SetBool("IsLanding", IsInvincibleToFireRing() == false && ySpeed < 0);

        if (characterState == CharacterState.Attack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0.0f)
            {
                attackTimer = 0.0f;
                characterState = CharacterState.Run;
                animator.SetBool("Attack", false);

                if (attackBox)
                {
                    attackBox.enabled = false;
                }
            }
        }

        if (characterState == CharacterState.Hurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer < 0.0f)
            {
                hurtTimer = 0.0f;
                characterState = CharacterState.Run;
                animator.SetBool("Hurt", false);
            }
        }
    }

    protected void FixedUpdate()
    {
        // Set speed factor to 0 if player is attacking or hurt
        if (characterState == CharacterState.Hurt || characterState == CharacterState.Attack)
        {
            xSpeedMultiplier = 0.0f;
        }
        else
        {
            xSpeedMultiplier = 1.0f;
        }

        // Moving along the x axis
        var movingVector = new Vector3(xSpeed * xSpeedMultiplier, ySpeed, 0.0f);
        CollisionFlags collisionFlags = controller.Move(movingVector * Time.fixedDeltaTime);

        // Moving along the y axis
        if (controller.isGrounded == false)
        {
            ySpeed -= gravity * Time.fixedDeltaTime;
        }

        if (controller.isGrounded)
        {
            // Apply drag if speed large than the normal speed
            if (xSpeed > runSpeed)
            {
                xSpeed -= drag * Time.fixedDeltaTime;
            }
        }

        // if player dies, apply drag to make the player stop
        if (characterState == CharacterState.Death)
        {
            if (xSpeed > 0.0f)
            {
                xSpeed -= drag * Time.fixedDeltaTime;
            }
            else
            {
                xSpeed = 0.0f;
            }
        }
        
        if (characterState == CharacterState.Run || characterState == CharacterState.Jump)
        {
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
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            if (xSpeed > runSpeed)
            {
                jumpType = JumpType.BigJump;
                // Also apply sprint speed when big jump
                ySpeed = bigJumpSpeed;
                animator.SetTrigger("BigJump");
            }
            else
            {
                jumpType = JumpType.SmallJump;
                ySpeed = jumpSpeed;
                animator.SetTrigger("Jump");
            }
        }
    }
    public void Sprint()
    {
        if (controller.isGrounded)
        {
            xSpeed = sprintSpeed;
        }
    }

    public void ChangeLaneUp()
    {
        if (controller.isGrounded == false)
        {
            return;
        }

        if (characterState != CharacterState.Run)
        {
            return;
        }
        
        if (currentLane - 1 >= 0)
        {
            currentLane--;
        }
        UpdateLaneCollision();
    }
    
    public void ChangeLaneDown()
    {
        if (controller.isGrounded == false)
        {
            return;
        }
        
        if (characterState != CharacterState.Run)
        {
            return;
        }
        
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
        var interactObj = other.gameObject.GetComponent<InteractObject>();
        
        if (other.CompareTag("SlaughterMachine"))
        {
            sprite.color = Color.white;
            animator.SetBool("Die", true);
            animator.SetBool("DieOnSlaughter", true);
            Die();
        }

        if (other.CompareTag("AttackBox"))
        {
            Hurt();
        }
        
        if (other.CompareTag("Score"))
        {
            var interactParent = other.gameObject.transform.parent.GetComponent<InteractObject>();
            if (interactParent)
            {
                if (currentLane == interactParent.currentLane)
                {
                    if (playerController)
                    {
                        if (xSpeed > runSpeed)
                        {
                            playerController.GetScore(20);
                        }
                        else
                        {
                            playerController.GetScore(10);
                        }
                    }
                }
            }
        }

        if (interactObj != null)
        {
            // ignore if the object and character are in different lane
            if (currentLane != interactObj.currentLane)
            {
                return;
            }
            
            if (other.CompareTag("Banana_Used"))
            {
                Hurt();
                interactObj.gameObject.SetActive(false);
            }

            if (other.CompareTag("Item"))
            {
                GetItem(interactObj);
                interactObj.gameObject.SetActive(false);
            }

            if (other.CompareTag("FireRing"))
            {
                // Check whether the current jump frame is free from fire ring damage
                if (IsInvincibleToFireRing() == false)
                {
                    sprite.color = Color.white;
                    animator.SetBool("Die", true);
                    animator.SetBool("DieOnFire", true);
                    Die();
                }
            }
        }
    }

    public void GetItem(InteractObject item)
    {
        
    }

    public void Hurt()
    {
        // if character is hurt during attack, it will abort the attack action
        if (characterState == CharacterState.Attack)
        {
            animator.SetBool("Attack", false);
        }
        characterState = CharacterState.Hurt;
        hurtTimer = hurtDuration;
        animator.SetBool("Hurt", true);
    }
    public void Die()
    {
        alive = false;
        xSpeed = 0.0f;
        characterState = CharacterState.Death;
    }

    public void Attack()
    {
        if (characterState == CharacterState.Run)
        {
            characterState = CharacterState.Attack;
            attackTimer = attackDuration;
            animator.SetBool("Attack", true);
            if (attackBox)
            {
                attackBox.enabled = true;
            }
        }
    }
}
