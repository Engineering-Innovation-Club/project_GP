using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementControl : MonoBehaviour
{

    private Vector3 mousePos;
    private Vector3 origin;

    private GameObject player;
    private GameObject rotator;

    private Vector3 scale;
    private Vector3 flipScale;

    Animator anim;
    Vector3 prevLocation;
    // Start is called before the first frame update
    void Start()
    {
        rotator = gameObject.transform.parent.gameObject;
        //player = rotator.transform.parent.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        anim = GetComponent<Animator>();
        scale = transform.localScale;

        flipScale = transform.localScale;
        flipScale.x *= -1;

        prevLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var pos = mousePos - rotator.transform.position;
            var a = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

            rotator.transform.rotation = Quaternion.AngleAxis(a - 90, Vector3.forward);
            transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);

            if (a < 90 && a > -90)
            {
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = flipScale;
            }

            rotator.transform.position = player.transform.position + new Vector3(0, player.GetComponent<CapsuleCollider2D>().bounds.size.y / 2, 0);
            if (transform.position != prevLocation)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
            prevLocation = transform.position;
        }
        
    }
}
