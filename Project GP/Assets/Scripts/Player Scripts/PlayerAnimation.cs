using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _animator;
    private PlayerMovementControl _pM;
    private Rigidbody2D rbody;


    private float timeSinceInteract;
    private string currentAnimation;

    // Dictionary variable that stores animation times
    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    // Boolean that tracks which direction the player is facing
    private bool isFacingRight;
    private bool isLanded;

    private const string PLAYER_IDLE = "PlayerIdle";
    private const string PLAYER_GUN_IDLE = "PlayerGunIdle";
    private const string PLAYER_CROUCH_IDLE = "PlayerCrouchIdle";
    private const string PLAYER_RUN = "PlayerRun";
    private const string PLAYER_CROUCH_WALK = "PlayerCrouchWalk";
    private const string PLAYER_CROUCH_ROLL = "PlayerCrouchRoll";
    private const string PLAYER_STANDING_ROLL = "player-standing-roll";
    private const string PLAYER_KNOCKBACK = "player-knockback";
    private const string PLAYER_ONHIT = "PlayerOnHit";
    private const string PLAYER_CROUCHUP = "PlayerCrouchUp";
    private const string PLAYER_CROUCHDOWN = "PlayerCrouchDown";
    private const string PLAYER_LAND = "Playerland";
    private const string PLAYER_JUMP = "PlayerJump";
    private const string PLAYER_FALL = "PlayerFall";
    private const string PLAYER_INTERACT = "PlayerInteract";
    private const string PLAYER_CLIMB = "PlayerClimb";

    private float epsilon = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _pM = GetComponent<PlayerMovementControl>();
        rbody = GetComponent<Rigidbody2D>();
        getAnimationTimes();
        isFacingRight = true;

        float t = animationTimes[PLAYER_STANDING_ROLL] / 0.5f;
        _animator.SetFloat("rollTime", t);
    }

    // Update is called once per frame
    void Update()
    {
        animationStates();
    }

    private void animationStates()
    {
        float vx = rbody.velocity.x - _pM.mpVel;

        // Interacting 
        if (_pM.isInteracting)
        {
            ChangeAnimationState(PLAYER_INTERACT);
            Invoke("FinishInteract", animationTimes[PLAYER_INTERACT]);
            return;
        }

        // Climbing
        if (_pM.isClimbing)
        {
            ChangeAnimationState(PLAYER_CLIMB);

            if (rbody.velocity.y - epsilon > 0 || rbody.velocity.y + epsilon <= 0)
            {
                _animator.enabled = true;
            }
            return;
        } else
        {
            _animator.enabled = true;
        }

        // Player direction
        if (vx + epsilon < 0)
        {
            if (isFacingRight)
            {
                flip();
            }
        }
        else if (vx - epsilon > 0)
        {
            if (!isFacingRight)
            {
                flip();
            }
        }
        
        // Crouching logic
        if (_pM.isGrounded && _pM.isCrouching)
        {
            if (_pM.isCrouchDown)
            {
                if (currentAnimation == PLAYER_STANDING_ROLL)
                {
                    _pM.isCrouchDown = false;
                    return;
                }
                ChangeAnimationState(PLAYER_CROUCHDOWN);
                CancelInvoke("CrouchUpEnd");
                Invoke("CrouchDownEnd", animationTimes[PLAYER_CROUCHDOWN]);
            } else if (_pM.isCrouchUp && !_pM.isRoll)
            {
                ChangeAnimationState(PLAYER_CROUCHUP);
                Invoke("CrouchUpEnd", animationTimes[PLAYER_CROUCHUP]);
            } else
            {
                if (_pM.isRoll && currentAnimation != PLAYER_STANDING_ROLL)
                {
                    ChangeAnimationState(PLAYER_CROUCH_ROLL);
                    return;
                }
                if (Mathf.Abs(vx) - epsilon > 0 && !_pM.isRoll)
                {
                    ChangeAnimationState(PLAYER_CROUCH_WALK);
                }
                else if (!_pM.isRoll)
                {
                    ChangeAnimationState(PLAYER_CROUCH_IDLE);
                }
            }
            return;
        }

        // On ground logic
        if (_pM.isGrounded && isLanded)
        {
            if (_pM.isRoll && currentAnimation != PLAYER_CROUCH_ROLL && currentAnimation != PLAYER_CROUCHUP)
            {
                ChangeAnimationState(PLAYER_STANDING_ROLL);
                return;
            }
            if (Mathf.Abs(vx) - epsilon > 0 && !_pM.isRoll)
            {
                ChangeAnimationState(PLAYER_RUN);
            } else if (!_pM.isRoll)
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        } else if (!_pM.isClimbing) // Off ground logic
        {
            if (rbody.velocity.y - epsilon > 0)
            {
                isLanded = false;
                print("jump");
                ChangeAnimationState(PLAYER_JUMP);
            } else if (rbody.velocity.y + epsilon <= 0)
            {
                ChangeAnimationState(PLAYER_FALL);
            }
        }

    }

    private void getAnimationTimes()
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            animationTimes.Add(clip.name, clip.length);
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

    public void ChangeAnimationState(string newState)
    {
        if (currentAnimation == newState) return;
        _animator.Play(newState);
        currentAnimation = newState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        int layer = collision.gameObject.layer;
        print(layer);
        if (tag.Equals("Ground") || layer == 8 || tag.Equals("Ladder"))
        {
            if (currentAnimation == PLAYER_FALL)
            {
                ChangeAnimationState(PLAYER_LAND);
                Invoke("Landed", animationTimes[PLAYER_LAND]);
            } else
            {
                Landed();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "passThroughBlock")
        {
            if (currentAnimation == PLAYER_FALL)
            {
                ChangeAnimationState(PLAYER_LAND);
                Invoke("Landed", animationTimes[PLAYER_LAND]);
            }
        }
    }

    private void Landed()
    {
        if (Mathf.Abs(rbody.velocity.x - _pM.mpVel) - epsilon > 0)
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
        isLanded = true;
    }

    private void CrouchDownEnd()
    {
        _pM.isCrouchDown = false;
    }

    private void CrouchUpEnd()
    {
        _pM.isCrouchUp = false;
        _pM.isCrouching = false;
    }

    public void FinishInteract()
    {
        _pM.isInteracting = false;
    }
}
