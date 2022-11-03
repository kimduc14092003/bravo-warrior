using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballPowerBoss : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject pos_fire;
    public GameObject Player;
    public float ballPowerSpeed;
    public float dame;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = pos_fire.transform.position;
        rb.AddForce((Player.transform.position-pos_fire.transform.position)*ballPowerSpeed);
        rb.velocity=Vector2.ClampMagnitude(rb.velocity,ballPowerSpeed);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth player = collision.gameObject.GetComponent<playerHealth>();
            player.addDamege(dame);
            Destroy(gameObject);
        }
    }
}
