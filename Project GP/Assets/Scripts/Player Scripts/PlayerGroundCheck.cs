using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "MovingPlatform" || collision.gameObject.tag == "passThroughBlock" || collision.gameObject.tag == "Stairs")
        {
            PlayerMovementControl pScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementControl>();
            pScript.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "MovingPlatform" || collision.gameObject.tag == "passThroughBlock" || collision.gameObject.tag == "Stairs")
        {
            PlayerMovementControl pScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementControl>();
            pScript.isGrounded = false;
        }
    }
}
