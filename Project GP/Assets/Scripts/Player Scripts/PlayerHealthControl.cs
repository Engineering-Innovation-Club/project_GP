using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthControl : MonoBehaviour
{
    // int that keeps track of player health
    public int health;
    public int maxHealth;

    SpriteRenderer srend;
    public float duration;
    private bool flashing;
    public float frequency;
    private float dTimer;
    private float fTimer;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 20;
        health = maxHealth;

        srend = GetComponent<SpriteRenderer>();
        dTimer = 0f;
        fTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashing)
        {
            fTimer += Time.deltaTime;
            dTimer += Time.deltaTime;

            if (dTimer >= duration)
            {
                flashing = false;
                dTimer = 0f;
                fTimer = 0f;
                srend.color = new Color(1, 1, 1);
            }

            if (fTimer >= frequency)
            {
                if (srend.color == new Color(1, 1, 1))
                {
                    srend.color = new Color(1, 0, 0);
                }
                else
                {
                    srend.color = new Color(1, 1, 1);
                }

                fTimer = 0f;
            }
        }
    }

    // hit function that when called, damages player
    public void hit(int damage)
    {
        health -= damage;

        flashing = true;
    }

    // heal function that when called, heals player
    public void heal(int amount)
    {
        health += amount;
    }

    // heal function that when called, heals player to full HP
    public void healToFull()
    {
        health = maxHealth;
    }
}