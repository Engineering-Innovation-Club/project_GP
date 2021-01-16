using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampusBossAnimations : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rbody;

    public Transform target;
    public Transform arms;
    public Animator leftArm;
    public Animator rightArm;
    public float moveSpeed = 10f;
    public float detectionRange = 10f;
    public float shootingRange = 5f;
    public float shootWalkRange;

    [SerializeField] public LayerMask playerLayer;
    public float health;
    public float currentHealth;

    public int chanceToRoll;
    public float rollSpeed = 10f;
    public int shotsFiredBeforeRolling;
    public float rollDuration;
    public float rollCoolDown;

    public float shootCoolDown;
    private float timeSinceLastShot;
    private float timeSinceLastRoll;

    private int shotsFired;
    private float currentRollTime;

    private bool startedPrep = false;

    // ANIMATION
    private const string BOSS_IDLE = "boss_idle";
    private const string BOSS_BALL = "boss_ball";
    private const string BOSS_ROLL = "boss_roll";
    private const string BOSS_SHOOT = "boss_shoot";
    private const string BOSS_START = "boss_start";
    private const string BOSS_STOP = "boss_stop";
    private const string BOSS_WALK = "boss_walk";

    private const string BOSS_FRONT_ARM_SHOOT = "boss_front_arm_shoot";
    private const string BOSS_FRONT_ARM_IDLE = "boss_front_arm_idle";
    private const string BOSS_BACK_ARM_IDLE = "boss_back_arm_idle";
    private const string BOSS_BACK_ARM_SHOOT = "boss_back_arm_shoot";

    private string currentAnimation;
    private string currentLeftArmAnimation;
    private string currentRightArmAnimation;
    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    private Vector3 scale;
    private Vector3 flipScale;
    private Vector3 playerHeight;

    private bool isFacingRight;
    private bool prepRolling;
    private bool isRolling;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        scale = transform.localScale;

        flipScale = transform.localScale;
        flipScale.x *= -1;

        isFacingRight = false;
        playerHeight = new Vector3(0, target.gameObject.GetComponent<BoxCollider2D>().bounds.size.y);

        currentHealth = health;
        isRolling = false;
        getAnimationTimes();
        currentRollTime = rollDuration;
        timeSinceLastShot = shootCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkRanges(detectionRange))   // player not detected
        {
            ChangeAnimationState(BOSS_IDLE);
            UpdateArms(BOSS_FRONT_ARM_IDLE, BOSS_BACK_ARM_IDLE);
            return;
        }
        timeSinceLastRoll += Time.deltaTime;
        timeSinceLastShot += Time.deltaTime;
        // condition for rolling: boss under 50% hp and fired x shots and rng chance to roll
        if (currentHealth != 0 && currentHealth/health <= 0.5f && shotsFired >= shotsFiredBeforeRolling 
            && !prepRolling && !isRolling && timeSinceLastRoll > rollCoolDown)
        {
            if (Random.Range(0, 100) < chanceToRoll)
            {
                // Roll
                print("prep roll");
                prepRolling = true;
            }
        }

        // prepare rolling
        // 1. Reset arms
        // 2. Hide Arms
        // 3. Start Ball animation
        // 4. Set rolling true
        if (prepRolling)
        {
            arms.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            arms.gameObject.SetActive(false);
            ChangeAnimationState(BOSS_BALL);
            Invoke("doneRollPrep", animationTimes[BOSS_BALL]);
            return;
        }

        // rolling
        // 1. Start rolling animation
        // 2. Move towards Player
        if (isRolling && currentRollTime > 0f)
        {
            print("rolling");
            ChangeAnimationState(BOSS_ROLL);
            float step = rollSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            currentRollTime -= Time.deltaTime;
            return;
        } else if (isRolling)
        {
            // End roll
            // 1. Start stop animation
            // 2. Set arms to active, and reset bools
            ChangeAnimationState(BOSS_STOP);
            Invoke("finishRoll", animationTimes[BOSS_STOP]);
            return;
        }

        rotateArms();

        // player detected
        // 1. is player in firing range
        // 2. is player moving out of firing range
        // 3. is player not in firing range
        if (checkRanges(shootingRange))
        {
            // in shooting range
            // 1. Fire shot
            // 2. Start shooting animations for arms
            // 3. Body should still be in idle
            if (timeSinceLastShot > shootCoolDown)
            {
                print("Shooting"); // Temp change with bullet instantation
                shotsFired++;
                timeSinceLastShot = 0;
                UpdateArms(BOSS_FRONT_ARM_SHOOT, BOSS_BACK_ARM_SHOOT);
            }
            ChangeAnimationState(BOSS_IDLE);


        }
        else if (checkRanges(shootWalkRange))
        {
            // shoot walk
            // 1. Fire shot
            // 2. Start shooting animations for arms
            // 3. Start walking animation for body
            // 4. Move towards player
            if (timeSinceLastShot > shootCoolDown)
            {
                print("Shooting"); // Temp change with bullet instantation
                shotsFired++;
                timeSinceLastShot = 0;
                UpdateArms(BOSS_FRONT_ARM_SHOOT, BOSS_BACK_ARM_SHOOT);
            }
            ChangeAnimationState(BOSS_WALK);
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        else
        {
            // walk
            // 1. Move towards player
            // 2. Start walking animation
            UpdateArms(BOSS_FRONT_ARM_IDLE, BOSS_BACK_ARM_IDLE);
            ChangeAnimationState(BOSS_WALK);
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }

        if (transform.position.x < target.position.x && !isFacingRight)
        {
            flip();
        } else if (transform.position.x > target.position.x && isFacingRight)
        {
            flip();
        }        
    }

    bool checkRanges(float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, playerLayer);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.DrawWireSphere(transform.position, shootWalkRange);

    }

    private void ChangeAnimationState(string newState)
    {
        if (currentAnimation == newState) return;
        anim.Play(newState);
        currentAnimation = newState;
    }

    private void UpdateArms(string newLeftArm, string newRightArm)
    {
        if (newLeftArm != currentLeftArmAnimation)
        {
            leftArm.Play(newLeftArm);
            currentLeftArmAnimation = newLeftArm;
        }
        if (newRightArm != currentRightArmAnimation)
        {
            rightArm.Play(newRightArm);
            currentRightArmAnimation = newRightArm;
        }
    }
    
    private void rotateArms()
    {
        var pos = target.position - arms.position + playerHeight/2f;
        var a = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        arms.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a + 180));
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        Vector3 armsScale = arms.localScale;
        armsScale.x *= -1;
        armsScale.y *= -1;
        theScale.x *= -1;
        transform.localScale = theScale;
        arms.localScale = armsScale;
        

    }

    private void getAnimationTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            animationTimes.Add(clip.name, clip.length);
        }
    }

    private void doneRollPrep()
    {
        prepRolling = false;
        isRolling = true;
    }

    private void finishRoll()
    {
        isRolling = false;
        arms.gameObject.SetActive(true);
        currentRollTime = rollDuration;
        timeSinceLastRoll = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && isRolling)
        {
            currentRollTime = -1;
        }
    }
}
