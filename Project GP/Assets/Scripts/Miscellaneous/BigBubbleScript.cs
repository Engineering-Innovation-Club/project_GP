using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBubbleScript : MonoBehaviour
{
    private Animator anim;
    private AnimatorClipInfo[] animClip;
    private float clipTime;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animClip = anim.GetCurrentAnimatorClipInfo(0);
        clipTime = animClip[0].clip.length;

        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= clipTime)
        {
            Destroy(gameObject);
        }
    }
}
