using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{
    public static void SavePlayerStats(PlayerHealthControl pHScript, PlayerMovementControl pMScript) {
        // Create Binary Formatter
        BinaryFormatter bf = new BinaryFormatter();

        // Create path of save data file
        string filePath =  Application.persistentDataPath + "/saveData.txt";

        // Create file stream in create mode to write in file
        FileStream fs = new FileStream(filePath, FileMode.Create);

        // Get save data
        SaveData saveData = new SaveData(pHScript, pMScript);

        // Set data in binary/hexdecimal and close
        bf.Serialize(fs, saveData);
        fs.Close();
    }

    public static SaveData LoadPlayerStats() {
        // Get file path
        string filePath = Application.persistentDataPath + "/saveData.txt";

        // If file exists
        if (File.Exists(filePath)) {

            // Get data and decode
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Open);

            SaveData saveData = bf.Deserialize(fs) as SaveData;
            fs.Close();

            return saveData;
        }
        else {
            return null;
        }
    }
}
