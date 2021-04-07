using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBulletAnimation : MonoBehaviour
{
    Animator anim;

    //private const string BULLET_LAUNCH = "droid_bullet_launch";
    private const string BULLET_TRAVEL = "RobotBulletTravel";
    private const string BULLET_DESTROY = "RobotBulletImpact";

    private string currentAnimation;
    private Dictionary<string, float> animationTimes = new Dictionary<string, float>();


    public GameObject bulletLight;
    public GameObject explodeLight;

    BulletScript bs;
    void Start()
    {
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
        if (collision.tag == "Ground" || collision.tag == "MovingPlatform" || collision.tag == "Wall" || collision.tag == "Player" || collision.tag == "Drone" || collision.tag == "Droid" || collision.tag == "CampusBoss" || collision.tag == "SuicideRobot")
        {
            ChangeAnimationState(BULLET_DESTROY);
            bulletLight.SetActive(false);
            explodeLight.SetActive(true);
            Invoke("DestroyBullet", animationTimes[BULLET_DESTROY]);
        }
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    //private void PlayTravel()
    //{
    //    ChangeAnimationState(BULLET_TRAVEL);
    //}
}
