using System.Collections;
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

    private string currentAnimation;

    public bool isMoving;
    public bool isFacingRight;

    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        isMoving = false;
        moveSpeed = 5f;

        getAnimationTimes();
    }

    // Update is called once per frame
    void Update()
    {
        // Lock rotation and y axis movement
        rbody.velocity.Set(rbody.velocity.x, 0);
        rbody.drag = 0f;
        rbody.angularDrag = 0f;
        rbody.angularVelocity = 0f;

        if (Input.GetKey("m")) {
            rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
        }
        else if (Input.GetKey("n")) {
            rbody.velocity = new Vector2(-moveSpeed, rbody.velocity.y);
        }
        else {
            rbody.velocity = new Vector2(0, 0);
        }

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
        if (isMoving) {
            ChangeAnimationState(DROID_WALK);
        }
        if (!isMoving) {
            ChangeAnimationState(DROID_IDLE);
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
}
