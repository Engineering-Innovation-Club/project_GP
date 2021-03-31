using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Get player
    public GameObject player;

    // Used to create a Vector3 position, since you can't individually change the x, y, and z values of an objects position
    Vector3 pos;

    // Used to handle camera size
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // Find camera component
        cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Set pos to player's position
        pos = player.transform.position;

        // Change camera position to pos
        transform.position = pos + new Vector3(0, 3, -1);
    }
}
