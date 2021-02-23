using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public GameObject player;
    public GameObject otherVent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovementControl pScript = player.GetComponent<PlayerMovementControl>();
            pScript.touchVent = true;

            otherVent.tag = "TeleportPoint";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovementControl pScript = player.GetComponent<PlayerMovementControl>();
            pScript.touchVent = false;

            otherVent.tag = "Untagged";
        }
    }
}
