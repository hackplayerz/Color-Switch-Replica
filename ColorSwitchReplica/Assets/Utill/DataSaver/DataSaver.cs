using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataSaver
{
    public static void SaveData(int data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Data.saved";
        FileStream stream = new FileStream(path,FileMode.Create);
        
        binaryFormatter.Serialize(stream,data);
        stream.Close();
    }

    public static int LoadBestScore()
    {
        string path = Application.persistentDataPath + "/Data.saved";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            var data = (int)binaryFormatter.Deserialize(stream);
            
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("No data");
            return 0;
        }
    }
}
