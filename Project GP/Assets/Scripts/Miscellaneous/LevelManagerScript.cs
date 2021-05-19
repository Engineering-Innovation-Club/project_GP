using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    public int numGenerate;

    public GameObject startStage;

    public List<GameObject> stages;
    public List<GameObject> connectors;

    private Vector3 exitConnectorPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnStart();

        for (int i = 0; i < stages.Count; i++)
        {
            foreach (Transform child in stages[i].transform.GetChild(0).GetChild(0))
            {
                connectors.Add(child.gameObject);
            }
        }
        
        for (int i = 0; i < numGenerate; i++)
        {

            Debug.Log("Before: " + exitConnectorPos);
            
            // Get Random Stage
            GameObject stage = stages[Random.Range(0, stages.Count)];

            // Calculate distance between center point and connector of stage
            Vector3 enterParentPos = stage.transform.position;
            Vector3 enterConnectorPos = stage.transform.GetChild(0).GetChild(0).GetChild(0).transform.position;
            Vector3 distancePos = enterParentPos - enterConnectorPos;

            // Calculate spawn position of stage as position of exit connector + distance between center of stage and enter connector
            Vector3 spawnPos = exitConnectorPos + distancePos;

            // Spawn Stage
            GameObject spawned = Instantiate(stage, spawnPos, Quaternion.identity);

            // Remove Exit Connector and then Enter Connector
            connectors.RemoveAt(0);
            connectors.RemoveAt(0);
            stages.Remove(stage);

            exitConnectorPos = spawned.transform.GetChild(0).GetChild(0).GetChild(1).transform.position;

            Debug.Log("After: " + exitConnectorPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to spawn the starting lab section first
    // Also adds the lab exit connector to the beginning of the connectors list
    void spawnStart()
    {
        Instantiate(startStage, new Vector3(0, 0, 0), Quaternion.identity);
        connectors.Insert(0, startStage.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);

        // Vector for position of exit connector
        exitConnectorPos = connectors[0].transform.position;
    }
}