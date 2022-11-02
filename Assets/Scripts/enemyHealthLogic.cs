using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyHealthLogic : MonoBehaviour
{
    public bool isHit;
    public float maxHealth;
    public float currentHealth;

    enemyMovement enemyMovement;

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
            Invoke("hitComplete", 0.5f);
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
