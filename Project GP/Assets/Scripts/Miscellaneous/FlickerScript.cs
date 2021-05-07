using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlickerScript : MonoBehaviour
{
    public float timeOn;
    public float timeOff;
    private float timer;
    private float changeTime;
    private float randNum;

    // Start is called before the first frame update
    void Start()
    {
        changeTime = 0f;

        randNum = Random.Range(0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > changeTime)
        {
            if (GetComponent<Light2D>().intensity != 0f)
            {
                //gameObject.SetActive(false);
                GetComponent<Light2D>().intensity = 0f;

                randNum = Random.Range(-0.15f, 0.25f);
                changeTime = timer + timeOff + randNum;
            }
            else if (GetComponent<Light2D>().intensity == 0f)
            {
                //gameObject.SetActive(true);
                GetComponent<Light2D>().intensity = 0.7f;

                randNum = Random.Range(0f, 0.15f);
                changeTime = timer + timeOn +  randNum;
            }
        }
    }   
}
