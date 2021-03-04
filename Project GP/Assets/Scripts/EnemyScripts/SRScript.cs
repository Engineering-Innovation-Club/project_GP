using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRScript : MonoBehaviour
{
    Rigidbody2D rbody;
    BoxCollider2D coll;

    Animator anim;

    GameObject player;
    Vector3 playerPos;

    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    private const string SR_MOVE = "SR_Move";
    private const string SR_ALARMED = "SR_Alarmed";
    private const string SR_WARNING = "SR_Warning";
    private const string SR_EXPLOSION = "SR_Explosion";

    private string currentAnimation;

    public bool isFacingRight;
    public float moveSpeed;
    public bool isAlerted;
    public float alertDistance;

    //distance you must be within from it to alert/unalert it. Uncreases to longdistance is alerted, viceversa.
    public float shortDistance = 8f;
    public float longDistance = 13f;

    public int health;
    public int maxHealth;

    private float timer;

    private bool hitPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        Physics2D.IgnoreLayerCollision(11, 11); // other enemies
        Physics2D.IgnoreLayerCollision(10, 11); // player

        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;

        moveSpeed = 5f;

        maxHealth = 1;
        health = maxHealth;

        isAlerted = false;

        //as the suicide robot starts unalerted, the distance you must be within to alert it starts short.
        alertDistance = shortDistance;

        getAnimationTimes();

        timer = 0f;
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
            if (!isFacingRight)
            {
                flip();
            }
        }
        else if (rbody.velocity.x < 0)
        {
            if (isFacingRight)
            {
                flip();
            }
        }
        animationStates();

        // SR is alerted if the player is a within a certain distance from it, vice versa. alert distance increases when its alerted and vice versa.
        if (Vector2.Distance(transform.position, player.transform.position) <= alertDistance)
        {
            alertDistance = longDistance;
            isAlerted = true;
        }
        else
        {
            alertDistance = shortDistance;
            isAlerted = false;
        }

        if (isAlerted)
        {
            path();
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                wander();

            }
        }
    }

    // Function that flips the model
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
            if (!isAlerted)
            {
                ChangeAnimationState(SR_MOVE);
            }
            else if (isAlerted)
            {
                ChangeAnimationState(SR_ALARMED);
            }
        }
        else
        {
            // Change to death animation
            ChangeAnimationState(SR_WARNING);

            Invoke("explode", animationTimes["SR_Warning"]);
        }

    }

    private void getAnimationTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
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
        playerPos = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, playerPos, moveSpeed / 50);
    }

    public void wander()
    {
        timer = Random.Range(1.5f, 3f);
        var temp = Random.Range(0, 2);
        if (temp == 0)
        {
            // Move Left
            rbody.velocity = new Vector2(-moveSpeed, 0);
        }
        else
        {
            // Move Right
            rbody.velocity = new Vector2(moveSpeed, 0);
        }

    }

    public void stop()
    {
        rbody.velocity = Vector3.zero;
        rbody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void explode()
    {
        coll.isTrigger = true;
        stop();
        Destroy(this.gameObject, animationTimes["SR_Explosion"]);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hit(999);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthControl pHScript = player.GetComponent<PlayerHealthControl>();
        pHScript.hit(3);
    }


}
