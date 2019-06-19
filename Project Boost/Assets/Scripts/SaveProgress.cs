using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveProgress
{

    public static void SavePlayer (int levels)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player.bin";
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayerData data = new PlayerData(levels);

		formatter.Serialize(stream, data);
		stream.Close();
	}
    public static PlayerData RetrieveData ()
	{
		string path = Application.persistentDataPath + "/player.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData levelsCompleted = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return levelsCompleted;

        }
        return new PlayerData(0);
    }
    public static void DestroyData ()
    {
        SavePlayer(0);
    }
}
