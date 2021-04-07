using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
   
    // Rigidbody variable used to create constant horizontal movement
    Rigidbody2D rbody;

    // Direction to shoot
    Vector3 shootDirection;

    // Speed of bullet
    public int bulletSpeed = 20;

    // target tag
    public string target;

    public bool hasAnim;

    // Default Damage for now, this needs to be changed from ProjectileShootingScript.
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        // Find rigidbody
        rbody = GetComponent<Rigidbody2D>();

        // Get direction
        if (target == "Enemy")
        {
            shootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            shootDirection.z = 0;
        } else if (target == "Player")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float playerHeight = player.GetComponent<CapsuleCollider2D>().bounds.size.y / 2f;
            Transform playerLocation = player.transform;
            shootDirection = new Vector3(playerLocation.position.x, playerLocation.position.y + playerHeight, 0) - transform.position;
        }

        shootDirection.Normalize();

        // Set velocity to shoot bullet
        rbody.velocity = shootDirection * bulletSpeed;
        if (target == "Player" && shootDirection.x < 0)
        {
            flip();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This is a built in unity function that checks when the object collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the tag of the object it collides with is "Ground"
        // Bullet will still go through "passThroughPlatforms" 
        if (collision.tag == "Ground" || collision.tag == "MovingPlatform" || collision.tag == "Wall")
        {
            // Destroy this specific instance of the bullet
            // Make sure to use this specific way of destroying instances of a prefab
            // If done incorrectly it could destroy the entire prefab, meaning the game wouldn't be able to spawn in any more bullets until the game restarts
            // Destroy this bullet
            rbody.velocity = Vector2.zero;
            if (!hasAnim) // Leave destroy logic to animation portion
                Destroy(this.gameObject);
        }

        // Check if bullet hits enemy
        else if (collision.tag == "Player")
        {
            PlayerHealthControl playerScript = collision.gameObject.GetComponent<PlayerHealthControl>();
            playerScript.hit(1);
        }
        else if (collision.tag == "Droid")
        {
            DroidScript droidEnemy = collision.gameObject.GetComponent<DroidScript>();
            droidEnemy.hit(damage);
        }
        else if (collision.tag == "Drone")
        {
            DroneScript droneEnemy = collision.gameObject.GetComponent<DroneScript>();
            droneEnemy.hit(damage);
        }
        else if (collision.tag == "CampusBoss")
        {
            CampusBossAnimations boss = collision.gameObject.GetComponent<CampusBossAnimations>();
            boss.hit(damage);
        }
        else if (collision.tag == "SuicideRobot")
        {
            SRScript sr = collision.gameObject.GetComponent<SRScript>();
            sr.hit(damage);
        }

        if (collision.tag != "Bullet" && collision.tag != "RobotPet" && collision.tag != "Droid" && collision.tag != "Drone" && collision.tag != "CampusBoss" && collision.tag != "SuicideRobot")
        {
            // Destroy this bullet
            rbody.velocity = Vector2.zero;
            if (!hasAnim)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // This is also a built in unity function that checks if the object is no longer visible by any camera
    // Using this, we will delete the bullet when it is off-screen, to make sure the game doesn't lag with a shit ton of bullets
    // If you're testing the game without the game being maximized on play, the scene preview window will count as a camera
    private void OnBecameInvisible()
    {
        // Destroy this specific instance of the bullet
        Destroy(this.gameObject);
    }

    private void flip()
    {
        transform.localScale = transform.localScale * -1;
    }

    // Set's the bullet Damage
    public void setDamage(int dam)
    {
        damage = dam;
    }
}
