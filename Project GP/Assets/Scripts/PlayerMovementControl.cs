using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
    // Component variables
    Rigidbody2D rbody;
    BoxCollider2D coll;

    // Player states
    public bool isCrouching;
    public bool isRoll;
    public bool isGrounded;
    public bool onMovingPlatform;
    public bool isInvincible;
    public bool touchWallSwitch;
    public bool touchSign;

    // Speed on moving platforms and regular move speed
    public float moveSpeed;
    public float mpVel;

    // Variables for animation handling
    Animation anim;
    Animator animator;

    // Delays in certain actions
    private float timeSinceLanding;
    private float timeSinceInteract;
    private float rollTimer;
    private float rollDelay;
    const float rollDuration = 0.5f;

    // Boolean that tracks which direction the player is facing
    private bool isFacingRight;

    // Collider variables for changing collider on crouch
    float collSizeY;
    float collOffY;

    // ??? All these crouching variables ????
    bool isCrouchUp;
    bool isCrouchDown;
    float crouchUpTimer;
    float crouchDownTimer;
    int toggleCrouch = -1;

    // Variables for pass-through platforms
    private GameObject currentPassThroughBlock;
    private float doubleTapDownTimer = 0.3f;
    private int doubleTapDownCount = 0;
    // LayerMask for the ground
    public LayerMask groundLayer;

    // Dictionary variable that stores animation times
    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    // for some reason we have a seperate variable just for this one clip??
    // honestly what happened when we were making the first script
    private AnimationClip standingRollClip;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animation>();
        animator = gameObject.GetComponent<Animator>();

        // Initializing states
        isCrouching = false;
        isRoll = false;
        isGrounded = false;
        isInvincible = false;

        moveSpeed = 5f;
        rollDelay = 0f;

        isFacingRight = true;
        touchWallSwitch = false;
        touchSign = false;

        // Getting boundaries
        collSizeY = coll.size.y;
        collOffY = coll.offset.y;

        // Fill animation dictionary with times
        getAnimationTimes();

        // Other variables
        timeSinceLanding = animationTimes["Playerland"];
    }

    // Update is called once per frame
    void Update()
    {
        timers();
        faceCrosshair();

        // Checks if the "d" key is being pressed
        if (Input.GetKey("d") && !isRoll)
        {
            // Changes the x-axis velocity of the player while retaining the y-axis velocity
            if (isCrouching)
            {
                rbody.velocity = new Vector3(moveSpeed / 2, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
            }
        }
        // Checks if the "a" key is being pressed
        else if (Input.GetKey("a") && !isRoll)
        {
            // Changes the x-axis velocity of the player while retaining the y-axis velocity
            if (isCrouching)
            {
                rbody.velocity = new Vector3(-(moveSpeed / 2), rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(-(moveSpeed), rbody.velocity.y);
            }
        }
        // This else statement is to set the player's x-axis velocity to 0 if neither "a" nor "d" are being pressed.
        // Without this statement, the player would glide.
        else
        {
            if (!onMovingPlatform && !isRoll)
            {
                // Set player x-axis velocity to 0 while retaining y-axis velocity
                rbody.velocity = new Vector2(0, rbody.velocity.y);
            }

            else if (onMovingPlatform)
            {
                // Set player velocity to moving platform velocity
                rbody.velocity = new Vector2(mpVel, rbody.velocity.y);
            }
        }

        // Check if the space key is pressed
        if (Input.GetKey("space"))
        {
            jump();
        }

        // Check if the "shift" key is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            roll();
        }

        // Check if interaction key is being pressed
        if (Input.GetKeyDown("f"))
        {
            // Check if touching wall switch
            if (touchWallSwitch)
            {
                // Access wall switch script
                WallSwitchScript wsScript = GameObject.FindGameObjectWithTag("IsTouching").GetComponent<WallSwitchScript>();

                // If the switch is currently on, flip off and open door
                if (wsScript.state)
                {
                    wsScript.state = false;
                    wsScript.OpenDoor();
                    animator.SetBool("isInteracting", true);
                }
            }
        }

        // Handle all crouching mechanics
        // Honestly I have no idea how this works and even reading the code doesn't help me
        // Copy pasted from old script and hope it works
        checkCrouch();

        // This is a bunch of stuff to handle going through pass-through platforms
        // Hopefully didn't break when we cleaned up the scripts
        isPassThroughBlock();
        // Double tap down key to go down a pass through block
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (doubleTapDownTimer > 0 && doubleTapDownCount == 1 && currentPassThroughBlock != null/*Number of Taps you want Minus One*/)
            {
                currentPassThroughBlock.GetComponent<BoxCollider2D>().isTrigger = true;
                isGrounded = false;
            }
            else
            {
                doubleTapDownTimer = 0.5f;
                doubleTapDownCount += 1;
            }
        }
        if (doubleTapDownTimer > 0)
        {
            doubleTapDownTimer -= 1 * Time.deltaTime;
        }
        else
        {
            doubleTapDownCount = 0;
        }

        // Handle animation stuff
        animationStates();
    }

    // Function that makes the player jump, provided certain conditions are ok
    private void jump()
    {
        // Check if player is currently touching the ground
        if (isGrounded)
        {
            // Retain current x-axis velocity, while adding a bit of y-axis velocity
            rbody.velocity = new Vector2(rbody.velocity.x, 6f);
        }
    }

    // Function that makes the player roll, if certain conditions are ok
    private void roll()
    {
        // If player is not currently rolling and there has been sufficient time since last roll
        if (!isRoll && rollDelay <= 0)
        {
            isRoll = true;
            isInvincible = true;
            rollTimer = rollDuration;
            rollDelay = 2f;

            // If player is facing towards the right
            if (transform.localScale.x > 0)
            {
                // Roll Right
                rbody.velocity = new Vector2(moveSpeed * 1.5f, rbody.velocity.y);
            }
            // If player is facing towards the left
            else if (transform.localScale.x < 0)
            {
                // Roll Left
                rbody.velocity = new Vector2(-moveSpeed * 1.5f, rbody.velocity.y);
            }
        }
    }

    // Function that makes the player crouch, if certain conditions are ok
    private void crouch(bool down)
    {
        if (down)   // crouching down
        {
            isCrouching = true;
            isCrouchDown = true;
            crouchDownTimer = animationTimes["PlayerCrouchDown"];
            coll.offset = new Vector2(coll.offset.x, (collOffY - (collSizeY / 4f)) * 1.2f);
            coll.size = new Vector2(coll.bounds.size.x, (collSizeY / 2f) * 1.2f);
        }
        else
        { // crouching up
            if (isCrouchDown) // if trying to crouch up while crouching down
            {
                isCrouchDown = false;
                isCrouchUp = true;
                isCrouching = true;
            }
            else // crouch up
            {
                isCrouchUp = true;
                crouchUpTimer = animationTimes["PlayerCrouchUp"];
                coll.offset = new Vector2(coll.offset.x, collOffY);
                coll.size = new Vector2(coll.bounds.size.x, collSizeY);
            }
        }
    }

    // Function called in Update() checking the crouching
    private void checkCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C)) // toggle crouch
        {
            if (toggleCrouch == -1) // toggle off
            {
                toggleCrouch = 1;
            }
            else if (toggleCrouch == 1)
            {
                toggleCrouch = 0;
            }
            else
            {
                toggleCrouch = -1;
            }
        }

        // Check if crouch key is being pressed
        if (Input.GetKeyDown(KeyCode.LeftControl) && toggleCrouch != 1) // if crouch is pressed and toggle crouch not currently active
        {
            crouch(true);
        }
        else if (toggleCrouch == 1 && !isCrouching) // if toggle crouch and not already crouching
        {
            crouch(true);
        }

        // Check if crouch key is no longer being pressed
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (toggleCrouch == -1) // if toggle crouch is not on
            {
                crouch(false);
            }
        }
        else if (toggleCrouch == 0) // toggle crouch is on and currently crouched
        {
            if (toggleCrouch == 0)
            {
                toggleCrouch = -1;  // set toggle to none
            }
            crouch(false);
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

    // Function called every frame in Update() that just has all the timers nicely packaged inside
    private void timers()
    {
        // Delay on rolling so you can't keep rolling
        if (rollDelay > 0)
        {
            rollDelay -= Time.deltaTime;
        }

        // Countdown to keep track of how long player is invincible during rolling
        if (rollTimer >= 0)
        {
            rollTimer -= Time.deltaTime;
        }
        // Not rolling anymore
        else
        {
            isRoll = false;
            rollTimer = rollDuration;
            isInvincible = false;
        }
    }

    // Function that orients the player towards the crosshair
    private void faceCrosshair()
    {
        // Check if crosshair is behind player
        // if crosshair is on the right of model
        if (transform.position.x < Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
        {
            // if not currently facing towards the right
            if (!isFacingRight)
            {
                // make model face right
                flip();
                isFacingRight = true;
            }
        }
        else // if crosshair is behind player model
        {
            // if currently facing towards the right
            if (isFacingRight)
            {
                // make model face left
                flip();
                isFacingRight = false;
            }
        }
    }

    // Function that handles all the animation stuff
    // No idea how this one works either
    private void animationStates()
    {
        // Player interaction
        if (animator.GetBool("isInteracting"))
        {
            timeSinceInteract -= Time.deltaTime;
            if (timeSinceInteract < 0)
            {
                animator.SetBool("isInteracting", false);
                timeSinceInteract = animationTimes["PlayerInteract"];
            }
        }
        else
        {
            timeSinceInteract = animationTimes["PlayerInteract"];

        }

        // Player rolling
        if (isRoll && !animator.GetBool("isRolling"))
        {
            float t = animationTimes["player-standing-roll"] / rollDuration;
            animator.SetFloat("rollTime", t);
            animator.SetBool("isRolling", true);
        }
        else if (!isRoll)
        {

            if (!isCrouching && animator.GetBool("isRolling"))
            {
                animator.SetBool("isRolling", false);
                isCrouching = true;
                crouch(false);
            }
            else
            {
                animator.SetBool("isRolling", false);
            }
        }
        // Player direction
        if (rbody.velocity.x < 0)
        {
            if (isFacingRight)
            {
                flip();
            }
        }
        else if (rbody.velocity.x > 0)
        {
            if (!isFacingRight)
            {
                flip();
            }
        }

        // crouching
        if (isGrounded && isCrouching)
        {
            if (isCrouchDown)
            {
                animator.SetBool("isCrouchDown", true);
                animator.SetBool("isCrouching", true);
            }

            if (isCrouchUp)
            {
                animator.SetBool("isCrouchUp", true);
            }

            if ((crouchDownTimer -= Time.deltaTime) < 0 && isCrouchDown)
            {
                animator.SetBool("isCrouchDown", false);
                isCrouchDown = false;
            }
            else if (!isCrouchDown)
            {
                animator.SetBool("isCrouchDown", false);
            }

            if ((crouchUpTimer -= Time.deltaTime) < 0 && isCrouchUp)
            {
                animator.SetBool("isCrouching", false);
                animator.SetBool("isCrouchUp", false);
                isCrouching = false;
                isCrouchUp = false;
            }

        }


        // On ground actions
        if (isGrounded && !isRoll)
        {
            if (rbody.velocity.x != 0)
            {
                animator.SetBool("isRunning", true);

            }
            else
            {
                animator.SetBool("isRunning", false);

            }

        }
        else
        {
            animator.SetBool("isRunning", false);

        }

        // In air actions
        if (!isGrounded)
        {
            if (rbody.velocity.y > 0)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
                animator.SetBool("isLanding", false);
            }
            else if (rbody.velocity.y <= 0)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                animator.SetBool("isLanding", false);
            }
        }
        else
        {
            if (animator.GetBool("isFalling"))
            {
                animator.SetBool("isLanding", true);
                animator.SetBool("isFalling", false);
            }
            else if (animator.GetBool("isLanding"))
            {
                if (animator.GetBool("isRunning") || animator.GetBool("isJumping"))
                {
                    timeSinceLanding = -1f;
                }
                timeSinceLanding -= Time.deltaTime;

                if (timeSinceLanding < 0)
                {
                    animator.SetBool("isLanding", false);
                    timeSinceLanding = animationTimes["Playerland"];
                }

            }
            else
            {
                timeSinceLanding = animationTimes["Playerland"];
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isLanding", false);

            }
        }
    }

    // Function to get the length of all the animations and stores them in a dictionary
    private void getAnimationTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            animationTimes.Add(clip.name, clip.length);
            if (clip.name == "player-standing-roll")
            {
                standingRollClip = clip;
            }
        }
    }

    // Function that returns the pass-through platform if there is one
    public GameObject getCurrentPassThroughBlock()
    {
        return currentPassThroughBlock;
    }

    // Check if player is going to land on a pass through block
    private bool isPassThroughBlock()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
        Debug.DrawRay(transform.position, new Vector3(0, -0.1f, 0), Color.green);
        Debug.DrawRay(transform.position, new Vector3(0, -0.1f, 0), Color.green);
        // If it collides with something that isn't NULL
        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "passThroughBlock")
            {
                currentPassThroughBlock = leftHit.collider.gameObject;
            }
            else
            {
                currentPassThroughBlock = null;
            }
            return true;
        }
        // If left side isn't, check right side
        else if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "passThroughBlock")
            {
                currentPassThroughBlock = rightHit.collider.gameObject;
            }
            else
            {
                currentPassThroughBlock = null;
            }

            return true;

        }
        else
        {
            currentPassThroughBlock = null;
        }
        return false;
    }

    // Function that checks when the player collider hits something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            // If they player hits a moving platform add velocity to move player along with platfomr
            mpVel = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            onMovingPlatform = true;
        }

        // Check if player is on the ground
        if (transform.position.y >= collision.gameObject.transform.position.y + (collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2))
        {
            isGrounded = true;
        }
    }

    // Function that checks if the player collider is continually hitting something
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // If on top of something
            if (transform.position.y >= collision.gameObject.transform.position.y + (collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2))
            {
                isGrounded = true;
            }

            // If on moving platform
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                // If they player hits a moving platform add velocity to move player along with platform
                // Update moving platform velocity constantly
                mpVel = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            }
        }
    }

    // Function that checks when the player collider is no longer hitting something
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

    }
}

