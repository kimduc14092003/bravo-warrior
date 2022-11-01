using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float dame;
    public float bulletSpeed;
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        if (rb2d.transform.rotation.z<0)
        {
            rb2d.AddForce(new Vector2(bulletSpeed, 0), ForceMode2D.Impulse);
        }
        else
        {
            rb2d.AddForce(new Vector2(-bulletSpeed, 0), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth player=collision.gameObject.GetComponent<playerHealth>();
            player.addDamege(dame);
            Destroy(gameObject);
        }
    }

}
