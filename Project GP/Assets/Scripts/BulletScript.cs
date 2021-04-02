﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
   
    // Rigidbody variable used to create constant horizontal movement
    Rigidbody2D rbody;

    // Direction to shoot
    Vector3 shootDirection;

    // Speed of bullet
    public int bulletSpeed = 20;

    // target tag
    public string target;

    public bool hasAnim;

    private int damage = 1;

    // Start is called before the first frame update
    void Start()
    {

        // Find rigidbody
        rbody = GetComponent<Rigidbody2D>();

        // Get direction
        if (target == "Enemy")
        {
            shootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            shootDirection.z = 0;
        } else if (target == "Player")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float playerHeight = player.GetComponent<CapsuleCollider2D>().bounds.size.y / 2f;
            Transform playerLocation = player.transform;
            shootDirection = new Vector3(playerLocation.position.x, playerLocation.position.y + playerHeight, 0) - transform.position;
        }

        shootDirection.Normalize();

        // Set velocity to shoot bullet
        rbody.velocity = shootDirection * bulletSpeed;
        if (target == "Player" && shootDirection.x < 0)
        {
            flip();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This is a built in unity function that checks when the object collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the tag of the object it collides with is "Ground"
        // Bullet will still go through "passThroughPlatforms" 
        if (collision.tag == "Ground" || collision.tag == "MovingPlatform" || collision.tag == "Wall")
        {
            // Destroy this specific instance of the bullet
            // Make sure to use this specific way of destroying instances of a prefab
            // If done incorrectly it could destroy the entire prefab, meaning the game wouldn't be able to spawn in any more bullets until the game restarts
            // Destroy this bullet
            rbody.velocity = Vector2.zero;
            if (!hasAnim) // Leave destroy logic to animation portion
                Destroy(this.gameObject);
        }

        // Check if bullet hits enemy
        else if (collision.tag == target)
        {
            // Access script of individual game object
            if (target == "Player")
            {
                PlayerHealthControl playerScript = collision.gameObject.GetComponent<PlayerHealthControl>();
                playerScript.hit(1);
            } else if (target == "Enemy")
            { // Erase this and make it so that it checks for the Enemy type and then damage that enemy.
                //EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();
                // Subtract health
                //enemyScript.health -= 1;

                // Learn how to check if this is the game object, such as droid. Maybe collision.name?
                Debug.LogError("Shot at " + collision.name); 
                if (collision.name == "Droid (new)")
                {
                    DroidScript droidEnemy = collision.gameObject.GetComponent<DroidScript>();
                    // droidEnenmy.hit(damage);
                    droidEnemy.health -= damage;
                    Debug.Log("We hit " + collision.name + " and did " + damage + " damage.");
                }
                else if (collision.name == "Drone")
                {
                    DroneScript droneEnemy = collision.gameObject.GetComponent<DroneScript>();
                    // droidEnenmy.hit(damage);
                    droneEnemy.health -= damage;
                    // Debug.LogError("We hit " + collider.name + " and did " + damage + " damage.");
                }
                else if (collision.name == "Campus Boss")
                {
                    CampusBossAnimations boss = collision.gameObject.GetComponent<CampusBossAnimations>();
 
                    boss.health -= damage;
                }
            }

            // Destroy this bullet
            rbody.velocity = Vector2.zero;
            if (!hasAnim) // Leave destroy logic to animation portion
                Destroy(this.gameObject);
        }
    }

    // This is also a built in unity function that checks if the object is no longer visible by any camera
    // Using this, we will delete the bullet when it is off-screen, to make sure the game doesn't lag with a shit ton of bullets
    // If you're testing the game without the game being maximized on play, the scene preview window will count as a camera
    private void OnBecameInvisible()
    {
        // Destroy this specific instance of the bullet
        Destroy(this.gameObject);
    }

    private void flip()
    {
        transform.localScale = transform.localScale * -1;
    }

    // Set's the bullet Damage
    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
