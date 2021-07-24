using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePopUpScript : MonoBehaviour
{
    public GameObject guide;

    public bool shouldPop;

    // Start is called before the first frame update
    void Start()
    {
        guide = transform.GetChild(0).gameObject;
        guide.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldPop)
        {
            guide.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && shouldPop)
        {
            // Pop Up
            guide.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && shouldPop)
        {
            // Down
            guide.SetActive(false);
        }
    }
}
