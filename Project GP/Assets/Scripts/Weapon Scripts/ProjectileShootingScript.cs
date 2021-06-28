using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShootingScript : MonoBehaviour
{
    public float fireRate = 0;
    public int damage = 1;
    public LayerMask whatToHit;
    //public string firePointOfWeapon; 
    //For every weapon the barrel of the gun is the fire point, simce that is where bullets come out. 

    float timeToFire = 0;
    Transform firePoint;
    [SerializeField] private Transform Bullet;
    [SerializeField] private Transform BulletTrail;
    private Transform rotator;

    public DialogueScript dScript;

    Animator anim;
    // Start is called before the first frame 
    void Awake()
    {
        //FirePoint will usually be the child of the gun or weapon. 
        //firePoint = transform.Find(firePointOfWeapon);
        Bullet.gameObject.GetComponent<BulletScript>().setDamage(damage);
        anim = GetComponent<Animator>();
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
        if (!PauseMenu.isPaused && !dScript.hasDialogue)
        {
            // Single Fire-Rate
            if (fireRate == 0)
            {
                if (Input.GetKeyDown("mouse 0") && canShoot())
                {
                    shoot();
                }
            }

            else
            {
                if (Input.GetKeyDown("mouse 0") && Time.time > timeToFire && canShoot())
                {
                    timeToFire = Time.time + 1 / fireRate;
                    shoot();
                }
            }
        }
        
    }

    void shoot()
    {
        //Debug.LogError("Shots fired");
        // Transfer the mouse position from the screen coordinate from the display position to the game world
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        //Vector3 rotation = rotator.rotation.eulerAngles;

        var x = mousePosition.x - firePoint.transform.position.x;
        var y = mousePosition.y - firePoint.transform.position.y;

        var angle = Mathf.Rad2Deg * Mathf.Atan2(y, x);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Vector3 rotation = new Vector3(0, 0, angle);
        Instantiate(Bullet, firePointPosition, rotation);
        Instantiate(BulletTrail, firePointPosition, rotation);

        anim.SetBool("isShooting", true);
        Invoke("doneShoot", 0.167f);
    }

    void doneShoot()
    {
        anim.SetBool("isShooting", false);
    }

    bool canShoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPos = player.transform.position + new Vector3(0, player.GetComponent<CapsuleCollider2D>().bounds.size.y / 2);

        RaycastHit2D rc = Physics2D.Raycast(firePointPosition, -(firePointPosition - playerPos));
        
        if (rc.collider.tag == "Ground" || rc.collider.tag == "Wall" || rc.collider.tag == "Ladder")
        {
            return false;
        }

        float x1 = playerPos.x - firePointPosition.x;
        float y1 = playerPos.y - firePointPosition.y;
        float z1 = Mathf.Sqrt(Mathf.Pow(x1, 2) + Mathf.Pow(y1, 2));

        float x2 = playerPos.x - mousePosition.x;
        float y2 = playerPos.y - mousePosition.y;
        float z2 = Mathf.Sqrt(Mathf.Pow(x2, 2) + Mathf.Pow(y2, 2));

        if (z2 > z1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
