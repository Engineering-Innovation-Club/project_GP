using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveCheckpointScript : MonoBehaviour
{
    private PlayerManager pSaveScript;
    private SaveTextScript textScript;

    public GameObject saveText;
    public string message;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        pSaveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        textScript = saveText.GetComponent<SaveTextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (timer <= 0f)
            {
                pSaveScript.SaveStats();
                textScript.FadeAnimation();

                textScript.message = message;

                timer = 5f;
            }
            
        }
    }
}
