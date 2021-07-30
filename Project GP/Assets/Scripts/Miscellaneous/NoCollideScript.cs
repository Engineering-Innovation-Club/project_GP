using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoCollideScript : MonoBehaviour
{
    public Transform thing;
    public Transform bullet;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(thing.GetComponent<BoxCollider2D>(), GetComponent<CompositeCollider2D>());
        Physics2D.IgnoreCollision(thing.GetComponent<BoxCollider2D>(), GetComponent<TilemapCollider2D>());
        try
        {
            Physics2D.IgnoreCollision(thing.GetComponent<CircleCollider2D>(), GetComponent<CompositeCollider2D>());
            Physics2D.IgnoreCollision(thing.GetComponent<CircleCollider2D>(), GetComponent<TilemapCollider2D>());
        } catch
        {
            
        }

        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<CompositeCollider2D>());
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<TilemapCollider2D>());
    }
}
