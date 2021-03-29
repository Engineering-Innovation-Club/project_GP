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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
