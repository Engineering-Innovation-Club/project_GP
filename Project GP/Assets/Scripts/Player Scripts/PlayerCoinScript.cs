using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoinScript : MonoBehaviour
{
    public int currency;

    // Start is called before the first frame update
    void Start()
    {
        currency = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Add(int amount)
    {
        currency += amount;
    }
}
