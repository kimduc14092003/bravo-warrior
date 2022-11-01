using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    private playerHealth playerHealth;
    private bool isJumpPressed;
    private bool isAttackPressed;
    private bool isGrounded;
    private bool isAttacking=false;
    private bool isDash=false;
    private bool isHit;
    private bool isDead;
    private float inputX;
    private float AirSpeedY;
    private float timeSinceAttack = 0f;
    private float attackDelay = 0.4f;
    private int facingDirection;
    private string currentAnimaton;
    private float currentCountdownSkill1=0;
    private float currentDashCountdown = 0;



    public bool isImmortal=false;
    public float walkSpeed ;
    public float jumpForce;
    public float dashSpeed;
    public float countdownSkill1;
    public float countdownDash;
    public GameObject attackSize ;
    public GameObject skill1;
    public Image uiFillCountdownSkill1;
    public Image uiFillCountdownDash;


    const string PLAYER_RUN = "Player_Run";
    const string PLAYER_JUMP_ATTACK = "Player_Jump_Attack";
    const string PLAYER_JUMP = "Player_Jump";
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_HURT = "Player_Hurt";
    const string PLAYER_FALL = "Player_Fall";
    const string PLAYER_DEAD = "Player_Dead";
    const string PLAYER_ATTACK = "Player_Attack";



    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingDirection = 1;
        playerHealth=gameObject.GetComponent<playerHealth>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        getStatusOfPlayerHealth();
        HanldeInputMovement();
        PlayerAction();
        HanldeAnimation();
        SwapFacingDirection();
        CountdownSkill();
    }

    private void getStatusOfPlayerHealth()
    {
        isHit = playerHealth.isHit;
        isDead = playerHealth.isDead;
    }

    private void HanldeInputMovement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        AirSpeedY = rb2d.velocity.y;
    }

    private void SwapFacingDirection()
    {
        // Swap direction of sprite depending on walk direction
        if (inputX < 0)
        {
            facingDirection = -1;
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }

        }
        else
        if (inputX > 0)
        {
            facingDirection = 1;
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }
    }

    private void PlayerAction()
    {
        if (isHit)
        {
            //Stop velocity when pressed player was hitted
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        //Attack
        if (Input.GetKey(KeyCode.J) && !isHit)
        {
            if (isGrounded)
            {
                rb2d.velocity = new Vector2(0, 0); //Stop velocity when pressed button attack
            }
            isAttackPressed = true;
            attackSize.SetActive(true);
        }
        if (isAttackPressed)
        {
            isAttackPressed = false;
            if (!isAttacking)
            {
                isAttacking = true;

                Invoke("AttackComplete", attackDelay);
            }
        }
        //Skill 1
        if(Input.GetKey(KeyCode.U) && !isHit && currentCountdownSkill1<=0)
        {
            Vector3 posSkill = new Vector3(attackSize.transform.position.x, attackSize.transform.position.y + 0.5f, attackSize.transform.position.z);
            if(facingDirection > 0)
            {
                Instantiate(skill1, posSkill, Quaternion.Euler(0, 0, 180));
            }
            else
            {
                Instantiate(skill1, posSkill, Quaternion.Euler(0, 0, 0));
            }
            currentCountdownSkill1 = countdownSkill1;
        }
        // Move
        if (!isAttacking && !isHit)
        {
            rb2d.velocity = new Vector2(inputX * walkSpeed, rb2d.velocity.y);
        }

        //Jump
        if (Input.GetKey(KeyCode.W) && isGrounded && !isAttacking && !isHit)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        //Dash
        if (Input.GetKey(KeyCode.L) && !isAttacking && !isHit &&currentDashCountdown<=0)
        {
            isDash = true;
            Invoke("DashComplete", 0.2f);
            Debug.Log("Dash");
            currentDashCountdown = countdownDash;
        }
        //Add Force when player dashing
        isDashing();

    }
    //Countdown player skill
    private void CountdownSkill()
    {
        if (currentCountdownSkill1 > 0)
        {
            currentCountdownSkill1-=Time.deltaTime;
            uiFillCountdownSkill1.fillAmount = Mathf.InverseLerp(0,countdownSkill1,currentCountdownSkill1);
        }
        if (currentDashCountdown > 0)
        {
            currentDashCountdown-=Time.deltaTime;
            uiFillCountdownDash.fillAmount= Mathf.InverseLerp(0, countdownDash, currentDashCountdown);
        }
    }

    private void HanldeAnimation()
    {
        if (isGrounded) //Player on the Grounded
        {
            if (isDead)
            {
                ChangeAnimationState(PLAYER_DEAD);
            }
            else if (isHit)
            {
                ChangeAnimationState(PLAYER_HURT);
            }
            else if (isAttacking)
            {
                ChangeAnimationState(PLAYER_ATTACK);
            }
            else if (inputX != 0 || rb2d.velocity.x != 0)
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }
        else // Player above the Grounded
        {
            if (isHit)
            {
                ChangeAnimationState(PLAYER_HURT);
            }
            else if (isAttacking)
            {
                ChangeAnimationState(PLAYER_JUMP_ATTACK);
            }
            else if (AirSpeedY > 0.01)
            {
                ChangeAnimationState(PLAYER_JUMP);
            }
            else if (AirSpeedY < -0.01)
            {
                ChangeAnimationState(PLAYER_FALL);
            }
        }
        //--------------------------------------
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground"||collision.gameObject.tag=="Enemy") // Check if player is on ground
        {
            isGrounded = true;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy") // Check if player is on ground
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy") // Check if player is on ground
        {
            isGrounded=false;
        }
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        currentAnimaton = newAnimation;
        animator.Play(newAnimation);
    }
    void AttackComplete()
    {
        isAttacking = false;
        attackSize.SetActive(false);

    }
    void DashComplete()
    {
        isDash = false;
    }
    void isDashing()
    {
        if (isDash)
        {
            rb2d.velocity = new Vector2(facingDirection * dashSpeed, rb2d.velocity.y); 
        }
    }
}
