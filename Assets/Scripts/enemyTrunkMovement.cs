using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTrunkMovement : MonoBehaviour
{
    private bool isIdle;
    private bool isRun;
    private bool isHit;
    private bool isAttack;
    private bool isDelay;
    private string currentAnimaton;
    private float nextFire=0;
    private bool isFacingRight;
    private pigEnemyLogic trunkEnemyLogic;
    private Animator animator;

    const string TRUNK_ENEMY_IDLE = "Trunk_enemy_Idle";
    const string TRUNK_ENEMY_RUN = "Trunk_enemy_Run";
    const string TRUNK_ENEMY_HIT = "Trunk_enemy_Hit";
    const string TRUNK_ENEMY_ATTACK = "Trunk_enemy_Attack";

    public float fireRate;
    public GameObject bullet;
    public GameObject pos_fire;
    public GameObject trunkGameObject;
    public GameObject Player;

    private void Awake()
    {
        trunkEnemyLogic=trunkGameObject.GetComponent<pigEnemyLogic>();
        isIdle = true;
        isRun = false;
        isAttack = false;
        isDelay = false;
        isFacingRight = false;
        animator = GetComponentInChildren<Animator>();
        
    }

    private void FixedUpdate()
    {
        hanldeAnimation();
        attackMovement();
        isHit = trunkEnemyLogic.isHit;

    }

    void attackMovement()
    {
        if (isAttack)
        {   
            if (Time.time > nextFire)
            {
                nextFire=Time.time+fireRate;

                if (isFacingRight)
                {
                    Instantiate(bullet, pos_fire.transform.position,Quaternion.Euler(new Vector3(0,0,180)));
                }
                else
                {
                    Instantiate(bullet, pos_fire.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                }

                isDelay = true;
                Invoke("delayToMatchAnimator", fireRate-0.5f);

            }

            // facing to Player
            if (transform.position.x > Player.transform.position.x)
            {
                isFacingRight = false;
                facingRight(isFacingRight);
            }
            else
            {
                isFacingRight = true;
                facingRight(isFacingRight);

            }
        }
    }

    void delayToMatchAnimator()
    {
        if (isDelay)
        {
            isDelay = false;
        }
    }

    void hanldeAnimation()
    {
        if (isHit)
        {
            ChangeAnimationState(TRUNK_ENEMY_HIT);
        }
        else if (isRun)
        {
            ChangeAnimationState(TRUNK_ENEMY_RUN);
        }
        else if (isIdle||isDelay)
        {
            ChangeAnimationState(TRUNK_ENEMY_IDLE);
        }
        else if (isAttack)
        {
            ChangeAnimationState(TRUNK_ENEMY_ATTACK);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isIdle=false;
            isAttack = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isIdle = true;
            isAttack = false;
        }
    }

    void facingRight(bool isfacingRight)
    {
        if (isfacingRight)
        {
            if (transform.localScale.x > 0)
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
        else
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        currentAnimaton = newAnimation;
        animator.Play(currentAnimaton);
    }
}
