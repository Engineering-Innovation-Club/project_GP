using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletAnimation : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rbody;

    //private const string BULLET_LAUNCH = "boss_bullet_start";
    private const string BULLET_TRAVEL = "boss_bullet_travel";
    private const string BULLET_DESTROY = "boss_bullet_impact";

    private string currentAnimation;
    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();

    BulletScript bs;
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        getAnimationTimes();
        bs = GetComponent<BulletScript>();
        //ChangeAnimationState(BULLET_LAUNCH);
        //Invoke("PlayTravel", 0.0f);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentAnimation == newState) return;
        anim.Play(newState);
        currentAnimation = newState;
    }

    private void getAnimationTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            animationTimes.Add(clip.name, clip.length);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "NoEnemyCollide" && (collision.tag == "Ground" || collision.tag == "MovingPlatform" || collision.tag == "Wall" || collision.tag == bs.target))
        {
            rbody.velocity = Vector2.zero;
            ChangeAnimationState(BULLET_DESTROY);
            Invoke("DestroyBullet", animationTimes[BULLET_DESTROY]);
        }
    }

    private void DestroyBullet()
    {
        print("destoying");
        Destroy(this.gameObject);
    }

    //private void PlayTravel()
    //{
    //    ChangeAnimationState(BULLET_TRAVEL);
    //}
}
