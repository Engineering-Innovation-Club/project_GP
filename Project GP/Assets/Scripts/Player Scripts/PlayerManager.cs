using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerHealthControl pHScript;
    PlayerMovementControl pMScript;
    
    // Start is called before the first frame update
    void Start()
    {
        pHScript = GetComponent<PlayerHealthControl>();
        pMScript = GetComponent<PlayerMovementControl>();
    }

    public void SaveStats() {
        SaveManager.SavePlayerStats(pHScript, pMScript);
    }
    public void LoadStats() {
        SaveData saveData = SaveManager.LoadPlayerStats();

        pHScript.health = saveData.playerHealth;
        pHScript.maxHealth = saveData.playerMaxHealth;
        pMScript.Spawn(new Vector3(saveData.playerPosX, saveData.playerPosY, saveData.playerPosZ));
    }
}
