using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRobotScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementControl>().hidBot)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovementControl pScript = collision.gameObject.GetComponent<PlayerMovementControl>();
            pScript.onBotShell = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag ==  "Player")
        {
            PlayerMovementControl pScript = collision.gameObject.GetComponent<PlayerMovementControl>();
            pScript.onBotShell = false;
        }
    }
}
