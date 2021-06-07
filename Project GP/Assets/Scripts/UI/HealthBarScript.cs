using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    public GameObject player;
    PlayerHealthControl pHScript;

    // Start is called before the first frame update
    void Start()
    {
        pHScript = player.GetComponent<PlayerHealthControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
