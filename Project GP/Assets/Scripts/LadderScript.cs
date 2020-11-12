using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{ 
    private PlayerMovementControl pScript;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            pScript = collision.gameObject.GetComponent<PlayerMovementControl>();
            pScript.isOnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pScript = collision.gameObject.GetComponent<PlayerMovementControl>();
            pScript.isOnLadder = false;
        }
    }
}
