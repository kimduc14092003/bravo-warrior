using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class pigEnemyLogic : MonoBehaviour
{
    public bool isHit;
    public float maxHealth;
    float currentHealth;

    enemyMovement enemyMovement;

    const string PIG_ENEMY_HIT = "Pig_Enemy_Hit";
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        isHit=false;
    }

    public void addDamage(float dame)
    {
        isHit=true;
        currentHealth -= dame;
        if (currentHealth <= 0)
        {
        Invoke("makeDead", 0.5f);
        }
        Invoke("hitComplete", 0.4f);
    }
    
    void hitComplete()
    {
        isHit = false;
    }

    void makeDead()
    {
        Destroy(gameObject);
        Destroy(transform.parent.gameObject);
    }
}
