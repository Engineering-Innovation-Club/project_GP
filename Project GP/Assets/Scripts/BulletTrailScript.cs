using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailScript : MonoBehaviour
{

    Rigidbody2D rbody;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        Vector3 shootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        shootDirection.z = 0;
        shootDirection.Normalize();

        // Set velocity to shoot bullet
        rbody.velocity = shootDirection * 10;
        print(rbody.velocity);
        Invoke("DestroyTrail", 0.5f);

    }

    void DestroyTrail()
    {
        Destroy(this.gameObject);
    }
}
