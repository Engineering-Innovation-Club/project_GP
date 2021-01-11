using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkeletonScript : MonoBehaviour
{

    public GameObject head;
    public GameObject gun;

    private Vector3 headPos;
    private Vector3 gunPos;

    private Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        headPos = head.transform.position;
        gunPos = gun.transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        GetData();
        CalculateAngles();
    }
    
    void GetData()
    {
        headPos = head.transform.position;
        gunPos = gun.transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void CalculateAngles()
    {
        var x1 = mousePos.x - headPos.x;
        var y1 = mousePos.y - headPos.y;
        var a1 = Mathf.Atan2(y1, x1) * Mathf.Rad2Deg;

        var x2 = mousePos.x - gunPos.x;
        var y2 = mousePos.y - gunPos.y;
        var a2 = Mathf.Atan2(y2, x2) * Mathf.Rad2Deg;

        head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a1 + 90f));
        gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a2));

        
    }
}
