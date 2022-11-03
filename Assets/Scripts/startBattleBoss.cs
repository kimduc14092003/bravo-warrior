using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBattleBoss : MonoBehaviour
{
    public GameObject Wall;
    public bool isOnSize;
    private void Awake()
    {
        isOnSize = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isOnSize=true;
            Wall.SetActive(true);
        }
    }
}
