using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int playerHealth;
    public int playerMaxHealth;

    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public SaveData(PlayerHealthControl pHScript, PlayerMovementControl pMScript) {
        playerHealth = pHScript.health;
        playerMaxHealth = pHScript.maxHealth;
        playerPosX = pMScript.GetPos().x;
        playerPosY = pMScript.GetPos().y;
        playerPosZ = pMScript.GetPos().z;
    }
}
