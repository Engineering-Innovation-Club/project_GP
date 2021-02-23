using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShootingScript : MonoBehaviour
{
    public float fireRate = 0;
    public float damage = 10;
    public LayerMask whatToHit;
    //public string firePointOfWeapon; 
    //For every weapon the barrel of the gun is the fire point, simce that is where bullets come out. 

    float timeToFire = 0;
    Transform firePoint;
    [SerializeField] private Transform Bullet;
    private Transform rotator;

    // Start is called before the first frame update
    void Awake()
    {
        //FirePoint will usually be the child of the gun or weapon. 
        //firePoint = transform.Find(firePointOfWeapon);

        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("Warning!\n No Fire Point!");
        }
        rotator = gameObject.transform.parent.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Single Fire-Rate
        if (fireRate == 0)
        {
            if (Input.GetKeyDown("mouse 0"))
            {
                shoot();
            }
        }

        else
        {
            if (Input.GetKeyDown("mouse 0") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                shoot();
            }
        }
    }

    void shoot()
    {
        //Debug.LogError("Shots fired");
        // Transfer the mouse position from the screen coordinate from the display position to the game world
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector3 rotation = rotator.rotation.eulerAngles;
        Instantiate(Bullet, firePointPosition, Quaternion.Euler(rotation.x, rotation.y, rotation.z + 90));
    }
}
