using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    private PlayerMovementControl pScript;
    private Animator anim;
    private Rigidbody2D rbody;

    private float timer;

    // Dictionary variable that stores animation times
    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Start()
    {
        pScript = GetComponent<PlayerMovementControl>();
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();

        getAnimationTimes();

        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rbody.velocity.x) > 0)
        {
            anim.SetBool("HasXVel", true);
        }
        else
        {
            anim.SetBool("HasXVel", false);
        }
        if (Mathf.Abs(rbody.velocity.y) > 0)
        {
            anim.SetBool("HasYVel", true);

            if (rbody.velocity.y > 0)
            {
                anim.SetFloat("VelY", rbody.velocity.y);
            }
            else if (rbody.velocity.y  < 0)
            {
                anim.SetFloat("VelY", rbody.velocity.y);
            }
        }
        else
        {
            anim.SetBool("HasYVel", false);
        }

        grounded();
        landing();
        crouching();

        Debug.Log(timer);
    }

    private void landing()
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Playerland")
        {
            anim.SetBool("Landing", true);
            timer += Time.deltaTime;

            if (timer >= animationTimes["Playerland"])
            {
                anim.SetBool("Landing", false);
                timer = 0f;
            }
        }
    }

    private void grounded()
    {
        if (pScript.isGrounded)
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded", false);
        }
    }

    private void crouching()
    {
        if (pScript.isCrouching)
        {
            anim.SetBool("Crouching", true);

            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PlayerCrouchDown")
            {
                anim.SetBool("CrouchingDown", true);
                timer += Time.deltaTime;

                if (timer >= animationTimes["PlayerCrouchDown"])
                {
                    anim.SetBool("CrouchingDown", false);
                    timer = 0f;
                }
            }
        }
        else
        {
            anim.SetBool("Crouching", false);

            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PlayerCrouchUp")
            {
                anim.SetBool("CrouchingUp", true);
                timer += Time.deltaTime;

                if (timer >= animationTimes["PlayerCrouchUp"])
                {
                    anim.SetBool("CrouchingUp", false);
                    timer = 0f;
                }
            }
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
}
