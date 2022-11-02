using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballPowerBoss : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject pos_fire;
    public GameObject Player;
    public float ballPowerSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce ((Player.transform.position-pos_fire.transform.position)*ballPowerSpeed);
    }
}
