using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementControl : MonoBehaviour
{

    private Vector3 mousePos;
    private Vector3 origin;

    private GameObject player;
    private GameObject rotator;

    // Start is called before the first frame update
    void Start()
    {
        rotator = gameObject.transform.parent.gameObject;
        player = rotator.transform.parent.gameObject;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var pos = mousePos - rotator.transform.position;
        var a = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        rotator.transform.rotation = Quaternion.AngleAxis(a - 90, Vector3.forward);
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
}
