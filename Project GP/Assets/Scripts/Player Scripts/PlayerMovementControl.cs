using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovementControl : MonoBehaviour
{
    // Component variables
    Rigidbody2D rbody;
    CapsuleCollider2D coll;

    // Player states
    public bool canMove;
    public bool isCrouching;
    public bool isRoll;
    public bool isGrounded;
    public bool onMovingPlatform;
    public bool isInvincible;
    public bool touchWallSwitch;
    public bool touchSign;
    public bool isOnLadder;
    public bool isTouchingLadder;
    public bool isClimbing;
    public bool isInteracting;
    public bool canMoveLeft;
    public bool canMoveRight;
    public bool touchVent;
    public bool onStairs;
    public bool hidBot;
    public bool onBotShell;
    public bool onTeleporter;

    // Speed on moving platforms and regular move speed
    public float moveSpeed;
    public float mpVel;

    // Variables for animation handling
    Animation anim;
    Animator animator;

    // Delays in certain actions
    private float timeSinceInteract;
    private float rollTimer;
    private float rollDelay;
    const float rollDuration = 0.5f;

    // Collider variables for changing collider on crouch
    float collSizeY;
    float collOffY;

    // ??? All these crouching variables ????
    public bool isCrouchUp;
    public bool isCrouchDown;
    int toggleCrouch = -1;

    // Variables for pass-through platforms
    private GameObject currentPassThroughBlock;
    private float doubleTapDownTimer = 0.3f;
    private int doubleTapDownCount = 0;
    // LayerMask for the ground
    public LayerMask groundLayer;

    // Variables to quickly change gravity and jump velocity
    public float gravity;
    public float jumpVel;

    // for some reason we have a seperate variable just for this one clip??
    // honestly what happened when we were making the first script
    private AnimationClip standingRollClip;

    // Variable to access animation script
    private PlayerAnimation pAnimScript;

    // Public variable to hold the scene it should load
    public int sceneNum;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animation>();
        animator = gameObject.GetComponent<Animator>();

        // Initializing states
        canMove = true;
        isCrouching = false;
        isRoll = false;
        isGrounded = false;
        isInvincible = false;
        isOnLadder = false;
        isTouchingLadder = false;
        canMoveLeft = true;
        canMoveRight = true;
        onStairs = false;
        hidBot = false;
        onBotShell = false;
        onTeleporter = false;

        moveSpeed = 8.5f;
        rollDelay = 0f;

        touchWallSwitch = false;
        touchSign = false;

        // Getting boundaries
        collSizeY = coll.size.y;
        collOffY = coll.offset.y;

        pAnimScript = GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        timers();

        if (!PauseMenu.isPaused && canMove)
        {
            if (isGrounded && (Input.GetKey("d") || Input.GetKey("a") || Input.GetKey("space")))
            {
                rbody.constraints = RigidbodyConstraints2D.None;
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                rbody.velocity = new Vector2(rbody.velocity.x, 0);
            }
            else if (isGrounded && !onMovingPlatform && !isOnLadder && !Input.GetKey("d") && !Input.GetKey("a") && !Input.GetKey("space"))
            {
                rbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else if (isGrounded && onMovingPlatform && !Input.GetKey("d") && !Input.GetKey("a") && !Input.GetKey("space"))
            {
                rbody.constraints = RigidbodyConstraints2D.None;
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                rbody.velocity = new Vector2(mpVel, 0);
            } else if (isGrounded && isOnLadder)
            {
                rbody.constraints = RigidbodyConstraints2D.None;
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else if (!isGrounded)
            {
                rbody.constraints = RigidbodyConstraints2D.None;
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            // Checks if the "d" key is being pressed
            if (Input.GetKey("d") && !isRoll && canMoveRight)
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

                if (!pAnimScript.isFacingRight)
                {
                    pAnimScript.flip();
                }
            }
            // Checks if the "a" key is being pressed
            else if (Input.GetKey("a") && !isRoll && canMoveLeft)
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

                if (pAnimScript.isFacingRight)
                {
                    pAnimScript.flip();
                }
            }
            else if (!Input.anyKey && !onMovingPlatform)
            {
                rbody.velocity = new Vector2(0, rbody.velocity.y);
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
                        isInteracting = true;
                        animator.SetBool("isInteracting", true);
                    }
                }

                // Check if touching vent door
                if (touchVent)
                {
                    GameObject vent = GameObject.FindGameObjectWithTag("TeleportPoint");
                    transform.position = vent.transform.position;
                }

                if (onBotShell)
                {
                    hidBot = true;
                }

                if (onTeleporter)
                {
                    SceneManager.LoadScene(sceneNum);
                }

            }

            // Function to handle all stairs mechanics
            stairs();

            // Handle all ladder mechanics
            checkLadder();

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

            // Call function that handles gravity
            gravityControl();
        }
    }

    private void stairs()
    {
        if (onStairs)
        {
            // Raycast down and side to check if stairs are there
            RaycastHit2D downRay = Physics2D.Raycast(transform.position + new Vector3((coll.bounds.size.x / 2f), 0), Vector2.down, 0.5f, groundLayer);
            RaycastHit2D sideRay = Physics2D.Raycast(transform.position, Vector2.right, 1f, groundLayer);

            if (!GetComponent<PlayerAnimation>().isFacingRight)
            {
                // Facing Left
                downRay = Physics2D.Raycast(transform.position - new Vector3((coll.bounds.size.x / 2f), 0), Vector2.down, 0.5f, groundLayer);
                sideRay = Physics2D.Raycast(transform.position, Vector2.left, 1f, groundLayer);
            }

            Debug.DrawRay(transform.position + new Vector3((coll.bounds.size.x / 2f), 0), Vector3.down, Color.red);
            Debug.DrawRay(transform.position, Vector2.right, Color.red);

            if (downRay.collider != null)
            {
                if (downRay.collider.tag == "Stairs" && sideRay.collider == null)
                {
                    // Going down stairs
                    //Debug.Log("down stairs");
                    if (Input.GetKey("d") && !isRoll)
                    {
                        var stairVel = (moveSpeed / Mathf.Cos(45)) / 1.75f;
                        rbody.velocity = new Vector2(stairVel, -stairVel);
                    }
                    else if (Input.GetKey("a") && !isRoll)
                    {
                        Debug.Log("test");
                        var stairVel = (moveSpeed / Mathf.Cos(45)) / 1.75f;
                        rbody.velocity = new Vector2(-stairVel, -stairVel);
                    }
                }
                else if (downRay.collider.tag == "Stairs" && sideRay.collider.tag == "Stairs")
                {
                    //Debug.Log("up stairs");
                    // Going up stairs
                    if (Input.GetKey("d") && !isRoll)
                    {
                        var stairVel = moveSpeed / Mathf.Cos(45);
                        rbody.velocity = new Vector2(stairVel, rbody.velocity.y);
                    }
                    else if (Input.GetKey("a") && !isRoll)
                    {
                        var stairVel = moveSpeed / Mathf.Cos(45);
                        rbody.velocity = new Vector2(-stairVel, rbody.velocity.y);
                    }
                }
            }
        }
    }

    // Function that makes the player jump, provided certain conditions are ok
    private void jump()
    {
        // Check if player is currently touching the ground
        if (isGrounded && !onStairs && rbody.velocity.y == 0)
        {
            // Retain current x-axis velocity, while adding a bit of y-axis velocity
            rbody.velocity = new Vector2(rbody.velocity.x, jumpVel);
        }

        // Check if player is touching ground and stairs
        if (isGrounded && onStairs && rbody.velocity.y != jumpVel)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, jumpVel);
        }
    }

    // Function that makes the player roll, if certain conditions are ok
    private void roll()
    {
        // If player is not currently rolling and there has been sufficient time since last roll
        if ((Input.GetKey("d") || Input.GetKey("a")) && !isRoll && rollDelay <= 0 && isGrounded)
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

    // Function that handles all gravity manipulation
    private void gravityControl()
    {
        if (isOnLadder && !isGrounded)
        {
            rbody.gravityScale = 0;
        }
        else
        {
            rbody.gravityScale = gravity;
        }
    }

    private void checkLadder()
    {
        if (isTouchingLadder && Input.GetKey("w"))
        {
            isOnLadder = true;
        }
        if (isOnLadder)
        {
            // Checks if the "w" key is being pressed
            if (Input.GetKey("w"))
            {
                isClimbing = true;
                rbody.velocity = new Vector3(0, 5);
            }
            // Checks if the "s" key is being pressed
            else if (Input.GetKey("s"))
            {
                isClimbing = true;
                rbody.velocity = new Vector3(0, -5);
            }
            else
            {
                isClimbing = false;
                rbody.velocity = new Vector3(0, 0);
            }

            if (Input.GetKey("a"))
            {
                rbody.velocity = new Vector2(-moveSpeed / 2, rbody.velocity.y);
            }

            else if (Input.GetKey("d"))
            {
                rbody.velocity = new Vector2(moveSpeed / 2, rbody.velocity.y);
            }
        }
        // If not on ladder
        else if (!isOnLadder)
        {
            isClimbing = false;
        }
    }

    // Function that makes the player crouch, if certain conditions are ok
    private void crouch(bool down)
    {
        if (down)   // crouching down
        {
            isCrouching = true;
            isCrouchDown = true;
            coll.offset = new Vector2(coll.offset.x, (collOffY - (collSizeY / 4f)) * 1.2f);
            coll.size = new Vector2(coll.bounds.size.x / 2, (collSizeY / 2f) * 1.2f);
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
                coll.offset = new Vector2(coll.offset.x, collOffY);
                coll.size = new Vector2(coll.bounds.size.x / 2, collSizeY);
            }
        }
    }

    private bool checkCrouchUp()
    {
        RaycastHit2D upRightRay = Physics2D.Raycast(transform.position + new Vector3((coll.bounds.size.x / 2f), 0), Vector2.up, collSizeY * 2, groundLayer);
        RaycastHit2D upLeftRay = Physics2D.Raycast(transform.position - new Vector3((coll.bounds.size.x / 2f), 0), Vector2.up, collSizeY * 2, groundLayer);

        //Debug.DrawRay(transform.position + new Vector3((coll.bounds.size.x / 2f), 0), Vector3.up * collSizeY * 2, Color.red);
        //Debug.DrawRay(transform.position - new Vector3((coll.bounds.size.x / 2f), 0), Vector3.up * collSizeY * 2, Color.red);

        if (upRightRay.collider != null || upLeftRay.collider != null)
        {
            return false;
        }
        return true;
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
        if (Input.GetKeyDown(KeyCode.LeftControl) && toggleCrouch != 1 && !isCrouching) // if crouch is pressed and toggle crouch not currently active
        {
            crouch(true);
        }
        else if (toggleCrouch == 1 && !isCrouching) // if toggle crouch and not already crouching
        {
            crouch(true);
        }

        // Check if crouch key is no longer being pressed
        if (Input.GetKeyUp(KeyCode.LeftControl) && checkCrouchUp())
        {
            if (toggleCrouch == -1) // if toggle crouch is not on
            {
                crouch(false);
            }
        }
        else if (toggleCrouch == 0 && checkCrouchUp()) // toggle crouch is on and currently crouched
        {
            if (toggleCrouch == 0)
            {
                toggleCrouch = -1;  // set toggle to none
            }
            crouch(false);
        } else if (toggleCrouch == -1 && !Input.GetKey(KeyCode.LeftControl) && checkCrouchUp() && isCrouching)
        {
            crouch(false);
        }
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

    // Function that returns the pass-through platform if there is one
    public GameObject getCurrentPassThroughBlock()
    {
        return currentPassThroughBlock;
    }

    // Check if player is going to land on a pass through block
    private bool isPassThroughBlock()
    {
        Vector3 displacement = new Vector3((coll.bounds.size.x) / 2f, 0, 0) ;
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position - displacement, Vector2.down, 0.5f, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + displacement, Vector2.down, 0.5f, groundLayer);
        //Debug.DrawRay(transform.position - displacement, new Vector3(0, -0.5f, 0), Color.green);
        //Debug.DrawRay(transform.position + displacement, new Vector3(0, -0.5f, 0), Color.green);
        
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
        if (collision.gameObject.tag == "Wall")
        {
            if (collision.GetContact(0).point.x < transform.position.x)
            {
                canMoveLeft = false;
            }
            else if (collision.GetContact(0).point.x > transform.position.y)
            {
                canMoveRight = false;
            }
            rbody.velocity = new Vector2(0, rbody.velocity.y);
        }
        else if (collision.gameObject.tag == "MovingPlatform")
        {
            onMovingPlatform = true;
            mpVel = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
        }
        else if (collision.gameObject.tag == "passThroughBlock")
        {
            currentPassThroughBlock = collision.gameObject;
        }
        else
        {
            currentPassThroughBlock = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            mpVel = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
        }
    }

    // Function that checks when the player collider is no longer hitting something
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            canMoveLeft = true;
            canMoveRight = true;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            currentPassThroughBlock = null;
        }
        else if (collision.gameObject.tag == "MovingPlatform")
        {
            mpVel = 0;
            onMovingPlatform = false;
        }
        else if (collision.gameObject.tag == "passThroughBlock")
        {
            currentPassThroughBlock = null;
        }

    }

    // Function that returns player position
    public Vector3 GetPos()
    {
        return transform.position;
    }

    // Function that sets player position to specific vector
    public void Spawn(Vector3 pos)
    {
        transform.position = pos;
    }
}