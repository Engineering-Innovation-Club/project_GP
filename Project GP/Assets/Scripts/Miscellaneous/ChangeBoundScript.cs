using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBoundScript : MonoBehaviour
{
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject topBound;
    public GameObject botBound;

    private Vector2 oldLBPos;
    private Vector2 oldRBPos;
    private Vector2 oldTBPos;
    private Vector2 oldBBPos;

    public Vector2 newLBPos;
    public Vector2 newRBPos;
    public Vector2 newTBPos;
    public Vector2 newBBPos;


    // Start is called before the first frame update
    void Start()
    {
        oldLBPos = leftBound.transform.position;
        oldRBPos = rightBound.transform.position;
        oldTBPos = topBound.transform.position;
        oldBBPos = botBound.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ChangeBoundPos();
        }
    }

    public void ChangeBoundPos()
    {
        leftBound.transform.position = newLBPos;
        rightBound.transform.position = newRBPos;
        topBound.transform.position = newTBPos;
        botBound.transform.position = newBBPos;
    }

    public void ResetBoundPos()
    {
        leftBound.transform.position = oldLBPos;
        rightBound.transform.position = oldRBPos;
        topBound.transform.position = oldTBPos;
        botBound.transform.position = oldBBPos;
    }
}
