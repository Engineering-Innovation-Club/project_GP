﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitchScript : MonoBehaviour
{
    // True = On, False = Off
    public bool state;
    public GameObject wall;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovementControl pScript = collision.gameObject.GetComponent<PlayerMovementControl>();
            pScript.touchWallSwitch = true;
            this.tag = "IsTouching";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovementControl pScript = collision.gameObject.GetComponent<PlayerMovementControl>();
            pScript.touchWallSwitch = false;
            this.tag = "WallSwitch";
        }
    }

    public void OpenDoor()
    {
        animator.SetTrigger("activate");
        wall.GetComponent<LabDoorScript>().open = true;
    }

    public void CloseDoor()
    {
        //wall.SetActive(true);
    }
}
