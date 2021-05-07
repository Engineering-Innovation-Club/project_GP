using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public float moveSpeed;
    public float speedFactor;
    Rigidbody2D rbody;

    public bool slowingDown;
    public bool speedingUp;

    public float stopTime;
    private float timer;
    public bool stopped;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 15f;

        timer = stopTime;
        stopped = false;

        rbody = GetComponent<Rigidbody2D>();

        Move();

        rbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (slowingDown)
        {
            SlowDown();
        }
        if (speedingUp)
        {
            SpeedUp();
        }

        if (stopped)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = stopTime;
                stopped = false;
                speedingUp = true;
            }
        }
        
    }

    public void Move()
    {
        rbody.velocity = new Vector2(moveSpeed, 0);
    }

    public void Stop()
    {
        rbody.velocity = Vector2.zero;
        stopped = true;
    }

    public void SlowDown()
    {
        if (rbody.velocity.x > 0)
        {
            rbody.velocity = new Vector2(rbody.velocity.x - Time.deltaTime * speedFactor, 0);
        }
        else
        {
            Stop();
            slowingDown = false;
        }
    }

    public void SpeedUp()
    {
        if (rbody.velocity.x < moveSpeed)
        {
            rbody.velocity = new Vector2(rbody.velocity.x + Time.deltaTime * speedFactor, 0);
        }
        else
        {
            Move();
            speedingUp = false;
        }
    }
}
