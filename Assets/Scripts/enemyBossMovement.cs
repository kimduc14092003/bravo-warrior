using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyBossMovement : MonoBehaviour
{
    private bool isIdle;
    private bool isRun;
    private bool isHit;
    private bool isAttack;
    private bool isDelay;
    private string currentAnimaton;
    private float nextFire = 0;
    private bool isFacingRight;
    private float maxHealth;
    private float currentHealth;
    private enemyHealthLogic bossEnemyLogic;
    private Animator animator;

    const string GHOST_BOSS_IDLE = "Ghost_Boss_Idle";
    const string GHOST_BOSS_MOVING = "Ghost_Boss_Moving";
    const string GHOST_BOSS_TAUNT = "Ghost_Boss_Taunt";
    const string GHOST_BOSS_CASTING_SPELL = "Ghost_Boss_Casting_Speell";
    const string GHOST_BOSS_HURT = "Ghost_Boss_Hurt";
    const string GHOST_BOSS_ATTACK = "Ghost_Boss_Attack";
    const string GHOST_BOSS_DYING = "Ghost_Boss_Dying";

    public float fireRate;
    public GameObject bullet;
    public GameObject pos_fire;
    public GameObject bossGameObject;
    public GameObject Player;
    public Slider SliderBossHealth;
    private void Awake()
    {
        bossEnemyLogic = bossGameObject.GetComponent<enemyHealthLogic>();
        isIdle = true;
        isRun = false;
        isAttack = false;
        isDelay = false;
        isFacingRight = false;
        animator = GetComponentInChildren<Animator>();
        maxHealth=bossEnemyLogic.maxHealth;
    }

    private void FixedUpdate()
    {
        hanldeAnimation();
        attackMovement();
        isHit = bossEnemyLogic.isHit;
        currentHealth = bossEnemyLogic.currentHealth;
        SliderBossHealth.value = Mathf.InverseLerp(0, maxHealth, currentHealth);
    }

    void attackMovement()
    {
        if (isAttack)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;

                if (isFacingRight)
                {
                    Instantiate(bullet, pos_fire.transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
                else
                {
                    Instantiate(bullet, pos_fire.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                }

                isDelay = true;
                Invoke("delayToMatchAnimator", fireRate - 0.5f);

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

    void hanldeAnimation()
    {
        if (isHit)
        {
            ChangeAnimationState(GHOST_BOSS_HURT);
        }
        else if (isRun)
        {
            ChangeAnimationState(GHOST_BOSS_MOVING);
        }
        else if (isIdle || isDelay)
        {
            ChangeAnimationState(GHOST_BOSS_IDLE);
        }
        else if (isAttack)
        {
            ChangeAnimationState(GHOST_BOSS_ATTACK);
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
            isIdle = false;
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
