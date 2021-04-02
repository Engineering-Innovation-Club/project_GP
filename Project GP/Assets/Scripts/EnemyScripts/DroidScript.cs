using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidScript : MonoBehaviour
{
    Rigidbody2D rbody;
    BoxCollider2D coll;

    Animator anim;
    Weapon weapon;
    [SerializeField] public LayerMask playerLayer;
    public Transform target;

    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    private const string DROID_IDLE = "droid_idle";
    private const string DROID_WALK = "droid_walk";
    private const string DROID_SHOOT = "droid_shoot";

    private const string DROID_DEATH = "droid_death";

    private const string DROID_ALERT_IDLE = "droid_idle_alerted";
    private const string DROID_ALERT_WALK = "droid_walk_alerted";

    private string currentAnimation;

    public bool isMoving;
    public bool isFacingRight;

    public float moveSpeed;

    public int health;
    public int maxHealth;

    public float detectionRange;
    public float shootingRange;

    private bool isShooting;
    private bool isAlerted;
    private bool playedAlerted;
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();

        isMoving = false;
        moveSpeed = 5f;
        
        // If maxHealth has not been set, make the Default health 1.
        if (maxHealth == null)
        { 
            maxHealth = 1;
        }
        
        health = maxHealth;
        isAlerted = false;
        getAnimationTimes();
    }

    // Update is called once per frame
    void Update()
    {
        // Lock rotation
        rbody.drag = 0f;
        rbody.angularDrag = 0f;
        rbody.angularVelocity = 0f;
        isMoving = false;
        isAlerted = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        // In shooting range
        if (checkRanges(shootingRange) && !isShooting)
        {  
            // shoot
            if(weapon.Shoot())
            {
                isShooting = true;
                Invoke("finishShoot", animationTimes[DROID_SHOOT]);
            } else
            {
                isShooting = false;
            }

        }
        else if (checkRanges(detectionRange) && !isShooting)
        {
            // Droid detects player.
            // Move towards player
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            isMoving = true;
 
        } else
        {
            isAlerted = false;
        }

        if (transform.position.x < target.position.x && !isFacingRight)
        {
            flip();
        }
        else if (transform.position.x > target.position.x && isFacingRight)
        {
            flip();
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
            if (isShooting)
            {
                ChangeAnimationState(DROID_SHOOT);
                return;
            }
            if (isMoving) {
                if (isAlerted && !playedAlerted)
                {
                    ChangeAnimationState(DROID_ALERT_WALK);
                    Invoke("alerted", animationTimes[DROID_ALERT_WALK]);
                } else
                {
                    ChangeAnimationState(DROID_WALK);
                }
            }
            if (!isMoving) {
                if (isAlerted && !playedAlerted)
                {
                    ChangeAnimationState(DROID_IDLE);
                    Invoke("alerted", animationTimes[DROID_ALERT_IDLE]);
                }
                else
                {
                    ChangeAnimationState(DROID_IDLE);
                }
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

    bool checkRanges(float range)
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Vector3.up*coll.bounds.size.y/2, range, playerLayer);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up * coll.bounds.size.y / 2, detectionRange);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * coll.bounds.size.y / 2, shootingRange);

    }

    private void finishShoot()
    {
        isShooting = false;
    }

    private void alerted()
    {
        playedAlerted = true;
    }
}
