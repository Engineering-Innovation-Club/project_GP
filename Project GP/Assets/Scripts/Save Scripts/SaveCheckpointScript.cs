using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCheckpointScript : MonoBehaviour
{
    private PlayerManager pSaveScript;

    // Start is called before the first frame update
    void Start()
    {
        pSaveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pSaveScript.SaveStats();
        }
    }
}
