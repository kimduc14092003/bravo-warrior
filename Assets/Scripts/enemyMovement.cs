using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    private string currentAnimaton;
    private Animator animator;
    private Rigidbody2D rb2d;
    private int facingDes=-1;
    private bool isFreeMovement;
    private bool isAttackMovement;
    private bool isMovingToFreeZone;
    private bool isHit_enemy;
    private enemyHealthLogic enemyHealthLogic;
    private bool isStop;

    const string PIG_ENEMY_IDLE = "Pig_Enemy_Idle";
    const string PIG_ENEMY_WALK = "Pig_Enemy_Walk";
    const string PIG_ENEMY_RUN = "Pig_Enemy_Run";
    const string PIG_ENEMY_HIT = "Pig_Enemy_Hit";

    public float nextMovement = 8f;
    public float enemySpeedWalk = 3f;
    public float enemySpeedRun = 5f;
   //public GameObject sensorDefaulPos;
    public GameObject Player;
    public GameObject pigEnemy;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        
        isFreeMovement = true;
        isMovingToFreeZone = false;
        isAttackMovement = false;
        isStop=false;
        enemyHealthLogic = pigEnemy.GetComponent<enemyHealthLogic>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //freeMovement(isFreeMovement);
        //moveToFreeZone(isMovingToFreeZone);
        attackMovement(isAttackMovement);
        hanldeAnimation();
        isHitted();
        isHit_enemy = enemyHealthLogic.isHit;
    }

    /*private void freeMovement(bool isFree)
    {
        float newAxisX = rb2d.transform.localPosition.x;

        if (!isFree) return;
        else
        {
            if ((newAxisX - sensorDefaulPos.transform.position.x) <= -4)
            {
                rb2d.velocity = new Vector2(0, 0);
                facingRight(true);
            }
            else if ((newAxisX - sensorDefaulPos.transform.position.x) >= 4)
            {
                rb2d.velocity = new Vector2(0, 0);
                facingRight(false);

            }

            if (Time.time > nextMovement)
            {
                facingDes *= -1;
                rb2d.AddForce(new Vector2(-enemySpeedWalk * facingDes, rb2d.velocity.y));
                nextMovement += 8;
            }

        }
    }*/

    private void isHitted()
    {
        if (isHit_enemy)
        {
            if (Player.transform.position.x <= transform.position.x)
            {
                transform.position=new Vector2(transform.position.x+0.12f, transform.position.y);
            }
            else if(Player.transform.position.x > transform.position.x)
            {
                transform.position=new Vector2(transform.position.x-0.12f, transform.position.y);
            }
        }
    }

    private void attackMovement(bool isAttacking)
    {
        if (isAttacking)
        {
            if (Vector2.Distance(Player.transform.position, transform.position)>0.8f)
            {
            transform.position = Vector2.MoveTowards(transform.position,
                                                    new Vector2(Player.transform.position.x, transform.position.y),
                                                    enemySpeedRun * Time.deltaTime);
            }

            // quay mat ve Player
            if(rb2d.transform.position.x > Player.transform.position.x) 
            {
                facingRight(false);
            }
            else
            {
                facingRight(true);

            }
        }
    }

    void hanldeAnimation()
    {
        if (isHit_enemy)
        {
            ChangeAnimationState(PIG_ENEMY_HIT);
        }
        else if (isAttackMovement)
        {
            ChangeAnimationState(PIG_ENEMY_RUN);
        }
        else if (isMovingToFreeZone)
        {
            ChangeAnimationState(PIG_ENEMY_WALK);
        }
        else if (isFreeMovement)
        {
            ChangeAnimationState(PIG_ENEMY_IDLE);
        } 
    }

    private void stopVelocity()
    {
        if (isStop)
        {
        rb2d.velocity = Vector2.zero;
            isStop = false;
        }
    }


    /*private void moveToFreeZone(bool isMoving)
    {
        stopVelocity();
        if (isMoving)
        {
            if (transform.position.x > sensorDefaulPos.transform.position.x)
            {
                rb2d.AddForce(new Vector2(-enemySpeedWalk * Time.deltaTime, rb2d.velocity.y));
                facingRight(false);

            }
            else
            {
                rb2d.AddForce(new Vector2(enemySpeedWalk * Time.deltaTime, rb2d.velocity.y));
                facingRight(true);

            }

            if (Math.Abs(transform.position.x - sensorDefaulPos.transform.position.x) < 0.1)
            {
                rb2d.velocity = new Vector2(0, 0);
                //isMovingToFreeZone = false;
                isFreeMovement = true;
            }
        }

    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            isFreeMovement = false;
            //isMovingToFreeZone = false;
            isAttackMovement = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAttackMovement = false;
            //isMovingToFreeZone = true;
            isStop = true;
            isFreeMovement = true;
        }
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        currentAnimaton = newAnimation;
        animator.Play(currentAnimaton);
    }

    void facingRight(bool isfacingRight)
    {
        if (isfacingRight)
        {
            if(transform.localScale.x>0)
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
        else
        {
            if(transform.localScale.x < 0)
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
}
