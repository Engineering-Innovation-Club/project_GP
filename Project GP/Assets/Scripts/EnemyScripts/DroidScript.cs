﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidScript : MonoBehaviour
{
    Rigidbody2D rbody;
    BoxCollider2D coll;

    Animator anim;

    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    private const string DROID_IDLE = "droid_idle";
    private const string DROID_WALK = "droid_walk";
    private const string DROID_SHOOT = "droid_shoot";

    private const string DROID_DEATH = "droid_death";

    private string currentAnimation;

    public bool isMoving;
    public bool isFacingRight;

    public float moveSpeed;

    public int health;
    public int maxHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        isMoving = false;
        moveSpeed = 5f;
        
        maxHealth = 1;
        health = maxHealth;

        getAnimationTimes();
    }

    // Update is called once per frame
    void Update()
    {
        // Lock rotation
        rbody.drag = 0f;
        rbody.angularDrag = 0f;
        rbody.angularVelocity = 0f;

        if (rbody.velocity.x > 0) {
            // Moving right
            isMoving = true;

            if (!isFacingRight) {
                flip();
            }
        }
        else if (rbody.velocity.x < 0) {
            // Moving left
            isMoving = true;

            if (isFacingRight) {
                flip();
            }
        }
        else {
            isMoving = false;
        }

        animationStates();
    }

    // Function that flips the player model
    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void animationStates() {
        if (health > 0) {
            if (isMoving) {
                ChangeAnimationState(DROID_WALK);
            }
            if (!isMoving) {
                ChangeAnimationState(DROID_IDLE);
            }
        }
        else {
            // Change to death animation
            ChangeAnimationState(DROID_DEATH);

            // Change collider bounds and offset to match animation frame
            // this shit is actually useless since the collider gets disabled literally right after
            // but I spent a lot of time geting these numbers so just in case we use it let's leave it here
            coll.offset = new Vector2(0.15f, 0.25f);
            coll.size = new Vector2(2f, 0.49f);

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

    public void hit(int damage) {
        health -= damage;
    }

}
