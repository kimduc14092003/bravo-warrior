using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject player;
    private bool notMoveLeft;
    // Start is called before the first frame update
    void Awake()
    {
        notMoveLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (notMoveLeft)
        {
            if (player.transform.position.x + 4f < transform.position.x)
            {
                return;
            }
        }
        transform.position = new Vector3( player.transform.position.x+4f,transform.position.y,transform.position.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag=="Wall")
        {
            Debug.Log("onStay");
        notMoveLeft=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Debug.Log("onExit");
            notMoveLeft = false;
        }
    }

}
