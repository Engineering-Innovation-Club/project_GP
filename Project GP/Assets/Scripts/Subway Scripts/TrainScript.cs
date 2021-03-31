using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public bool moving;
    public float moveSpeed;
    Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        moving = true;
        moveSpeed = 15f;

        rbody = GetComponent<Rigidbody2D>();

        Move();

        rbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        rbody.velocity = new Vector2(moveSpeed, 0);
    }

    public void Stop()
    {
        rbody.velocity = Vector2.zero;
    }
}
