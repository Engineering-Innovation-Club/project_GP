using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Get player
    public GameObject player;
    public bool isBounded;
    public Transform leftBound;
    public Transform rightBound;
    public Transform topBound;
    public Transform botBound;

    // Used to create a Vector3 position, since you can't individually change the x, y, and z values of an objects position
    Vector3 pos;

    // Used to handle camera size
    private Camera cam;
    private float xSize;
    private float ySize;

    // Start is called before the first frame update
    void Start()
    {
        // Find camera component
        cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize = 10.0f;
        xSize = Camera.main.orthographicSize * Screen.width / Screen.height;
        ySize = Camera.main.orthographicSize;       
    }

    // Update is called once per frame
    void Update()
    {
        // Set pos to player's position
        pos = player.transform.position;

        // Change camera position to pos
        if (isBounded)
        {
            pos = pos + new Vector3(0, 3, -1);
            pos.x = Mathf.Clamp(pos.x, leftBound.position.x + xSize, rightBound.position.x - xSize);
            pos.y = Mathf.Clamp(pos.y, botBound.position.y + ySize, topBound.position.y - ySize);
            transform.position = pos;
        }
        else
        {
            transform.position = pos + new Vector3(0, 3, -1);
        }
    }
}
