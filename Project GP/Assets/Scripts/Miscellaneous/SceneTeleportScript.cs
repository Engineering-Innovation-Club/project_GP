using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleportScript : MonoBehaviour
{
    PlayerMovementControl pScript;

    // Start is called before the first frame update
    void Start()
    {
        pScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pScript.onTeleporter = true;
        }
    }
}
