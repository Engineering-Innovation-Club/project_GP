﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    Rigidbody2D rbody;
    BoxCollider2D coll;

    Animator anim;

    GameObject player;
    Vector3 playerPos;

    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    private const string DRONE_MOVE = "drone_move";
    private const string DRONE_ALARMED = "drone_alarmed";
    private const string DRONE_ATTACK = "drone_attack";
    private const string DRONE_DEATH = "drone_death";

    private string currentAnimation;

    public bool isMoving;
    public bool isFacingRight;
    public float moveSpeed;
    public bool isAlerted;

    public int health;
    public int maxHealth;

    private bool hitPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;

        isMoving = false;
        moveSpeed = 5f;

        maxHealth = 3;
        health = maxHealth;

        isAlerted = false;
        hitPlayer = false;

        getAnimationTimes();
    }

    // Update is called once per frame
    void Update()
    {
        // Lock rotation
        rbody.drag = 0f;
        rbody.angularDrag = 0f;
        rbody.angularVelocity = 0f;

        if (rbody.velocity.x > 0)
        {
            // Moving right
            isMoving = true;

            if (!isFacingRight)
            {
                flip();
            }
        }
        else if (rbody.velocity.x < 0)
        {
            // Moving left
            isMoving = true;

            if (isFacingRight)
            {
                flip();
            }
        }
        else
        {
            isMoving = false;
        }
        animationStates();

        if (!hitPlayer)
        {
            path();
        }
    }

    // Function that flips the player model
    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void animationStates()
    {
        if (health > 0)
        {
            if (isMoving)
            {
                ChangeAnimationState(DRONE_MOVE);
            }
            if (!isMoving)
            {
                ChangeAnimationState(DRONE_MOVE);
            }
        }
        else
        {
            // Change to death animation
            ChangeAnimationState(DRONE_DEATH);

            // Disable collider and freeze x and y position
            coll.enabled = false;
            rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

    }

    private void getAnimationTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            Debug.Log(clip.name);
            animationTimes.Add(clip.name, clip.length);
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentAnimation == newState) return;
        anim.Play(newState);
        currentAnimation = newState;
    }

    public void hit(int damage)
    {
        health -= damage;
    }

    public void path()
    {
        playerPos = player.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerPos, moveSpeed / 100);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hitPlayer = true;
        }
    }
}