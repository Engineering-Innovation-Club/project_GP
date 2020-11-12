﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthControl : MonoBehaviour
{
    // int that keeps track of player health
    public int health;
    public int maxHealth;

    public PlayerUIScript playerUIScript;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 5;
        health = maxHealth;
        playerUIScript.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // hit function that when called, damages player
    public void hit(int damage)
    {
        health -= damage;
        playerUIScript.SetHealth(health);
    }

    // heal function that when called, heals player
    public void heal(int amount)
    {
        health += amount;
        playerUIScript.SetHealth(health);
    }

    // heal function that when called, heals player to full HP
    public void healToFull()
    {
        health = maxHealth;
        playerUIScript.SetHealth(health);
    }
}