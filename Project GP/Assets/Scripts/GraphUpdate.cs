using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GraphUpdate : MonoBehaviour
{
    Vector3 previousLoc;
    Collider2D bounds;
    // Start is called before the first frame update
    void Start()
    {
        previousLoc = transform.position;
        bounds = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (Mathf.Abs(Vector3.Distance(pos, previousLoc)) > 0.01f)
        {
            previousLoc = pos;
            Vector3 size = new Vector3(bounds.bounds.size.x + 2f, bounds.bounds.size.y + 2f);
            Bounds b = new Bounds(bounds.bounds.center, size);
            var guo = new GraphUpdateObject(b);
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);
        }

    }

}
