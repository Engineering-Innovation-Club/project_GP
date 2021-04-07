using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleportScript : MonoBehaviour
{
    PlayerMovementControl pScript;

    // This variable holds the scene number the player will teleport to
    public int num;

    // Start is called before the first frame update
    void Start()
    {
        // Get player script
        pScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player collides
        if (collision.gameObject.tag == "Player")
        {
            // Change player variables
            pScript.onTeleporter = true;
            pScript.sceneNum = num;
        }
    }
}
