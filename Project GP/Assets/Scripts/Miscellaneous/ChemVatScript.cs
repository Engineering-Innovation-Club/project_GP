using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemVatScript : MonoBehaviour
{
    public GameObject bigBubble;
    public GameObject littleBubble;
    public GameObject twoBubble;

    private float timer;
    public float spawnTime;
    private float choice;
    private Vector3 pos;

    public int numSurface;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTime;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            choice = Random.Range(1, 4);
            //Debug.Log(choice);
            spawn((int)choice);

            timer = spawnTime;
        }
    }

    private void spawn(int type)
    {

        pos = new Vector3(Random.Range(gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.min.x + 5f, gameObject.transform.GetChild(numSurface - 1).GetComponent<SpriteRenderer>().bounds.max.x - 5f), pos.y, pos.z);
        if (type == 1)
        {
            // Big Bubble
            Instantiate(bigBubble, pos, Quaternion.identity);
        }
        else if (type == 2)
        {
            // Little Bubble
            Instantiate(littleBubble, pos, Quaternion.identity);
        }
        else if (type == 3)
        {
            // Two Bubble
            Instantiate(twoBubble, pos, Quaternion.identity);
        }
    }
}
