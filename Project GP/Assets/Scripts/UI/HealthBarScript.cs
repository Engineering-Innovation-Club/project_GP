using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    public GameObject player;
    PlayerHealthControl pHScript;

    public GameObject mask;
    public GameObject fill;

    private float maxWidth;
    private float minWidth;
    private float startPos;

    // Start is called before the first frame update
    void Start()
    {
        pHScript = player.GetComponent<PlayerHealthControl>();

        maxWidth = mask.GetComponent<RectTransform>().sizeDelta.x;
        minWidth = 24.4f;
        startPos = mask.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        mask.GetComponent<RectTransform>().sizeDelta = new Vector2((maxWidth * GetPercentage()) + minWidth, mask.GetComponent<RectTransform>().sizeDelta.y);
        mask.transform.position = new Vector2((startPos * GetPercentage()) + minWidth, mask.transform.position.y);
    }

    float GetPercentage()
    {
        return ((float)pHScript.health / (float)pHScript.maxHealth);
    }
}