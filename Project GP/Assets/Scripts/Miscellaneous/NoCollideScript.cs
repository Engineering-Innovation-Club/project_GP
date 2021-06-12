using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoCollideScript : MonoBehaviour
{
    public GameObject thing;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(thing.GetComponent<BoxCollider2D>(), GetComponent<CompositeCollider2D>());
        Physics2D.IgnoreCollision(thing.GetComponent<BoxCollider2D>(), GetComponent<TilemapCollider2D>());

        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<CompositeCollider2D>());
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<TilemapCollider2D>());
    }
}
