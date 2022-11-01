using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    private playerController playerController;
    private float currentHealth;

    public bool isHit;
    public bool isDead;
    public bool isImmortal;
    public float maxHealth;
    public Slider playerHealthSlider;

    // Start is called before the first frame update
    void Awake()
    {
        playerController=gameObject.GetComponent<playerController>();
        isDead =false;
        isHit=false;
        currentHealth = maxHealth;
        playerHealthSlider.maxValue = maxHealth;
        playerHealthSlider.value = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isImmortal = playerController.isImmortal;
    }
    //Chuc nang dinh sat thuong
    public void addDamege(float dame)
    {
        if (isImmortal)
        {
            dame = 0;
        }
        if (dame <= 0)
        {
            return;
        }
        currentHealth-=dame;
        playerHealthSlider.value=currentHealth;
        if(currentHealth <= 0)
        {
            makeDeath();
        }
        isHit=true;
        Invoke("isHittedComplete", 0.5f);
    }
    // chuc nang hoi mau
    public void onHealing(float healthAmount)
    {
        currentHealth += healthAmount;
        if(currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }
        playerHealthSlider.value = currentHealth;
    }

    private void isHittedComplete()
    {
        isHit = false;
    }

    private void makeDeath()
    {
        isDead = true;
        Invoke("freezeScene", 1.02f);
    }

    private void freezeScene()
    {
        Time.timeScale = 0;
    }
}
