using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public bool isOpen;
    BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementControl pScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementControl>();
        
        if (pScript.hidBot)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }

        if (isOpen)
        {
            coll.enabled = false;
        }
        else
        {
            coll.enabled = true;
        }
    }
}
