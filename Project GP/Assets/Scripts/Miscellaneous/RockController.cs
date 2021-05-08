using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private float timer;
    public float rockTime;

    public GameObject rock;
    public GameObject leftBound;
    public GameObject rightBound;
    public bool spawnRocks;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnRocks)
        {
            timer += Time.deltaTime;

            if (timer >= rockTime)
            {
                timer = 0f;
                Instantiate(rock, new Vector3(Random.Range(leftBound.transform.position.x, rightBound.transform.position.x), transform.position.y, 0), Quaternion.identity);
            }
        }
    }
}
