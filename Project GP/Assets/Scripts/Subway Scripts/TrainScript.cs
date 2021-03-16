using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    // Variables for gameobjects and train states
    public bool travelling;
    public float timer;
    public float moveSpeed;
    public GameObject train;
    private GameObject newTrain;

    // Start is called before the first frame update
    void Start()
    {
        travelling = false;
        timer = 0f;
        newTrain = null;
    }

    // Update is called once per frame
    void Update()
    {
        // If train is not running, decrease timer
        if (!travelling)
        {
            timer -= Time.deltaTime;
        }

        // If timer hits 0, run train
        if (timer <= 0)
        {
            runTrain();
        }
    }

    // Function to reset the train timer
    void resetTrainTimer()
    {
        timer = Random.Range(-10f, 10f) + 30f;
    }

    // Function to run the train
    void runTrain()
    {
        // Set newTrain variable to the instantiated train prefab
        newTrain = Instantiate(train, new Vector2(-200f, -2.5f), Quaternion.identity);

        // Set travelling to true and call resetTrainTimer()
        travelling = true;
        resetTrainTimer();

        // Get rigidbody2d and change velocity
        Rigidbody2D rbody = newTrain.GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector2(moveSpeed, 0);
    }

    // Unity built in function to when object leaves camera view
    private void OnBecameInvisible()
    {
        // Destroy the newTrain and set travelling to false
        GameObject.Destroy(newTrain);
        travelling = false;
    }
}
