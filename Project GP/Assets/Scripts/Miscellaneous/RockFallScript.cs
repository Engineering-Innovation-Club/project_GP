using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFallScript : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        rbody.velocity = new Vector2(0, -moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
