using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public enum FileIndex
{
    GAMESETTINGS,
    HIGHSCORES,
}

public class JsonHelper<T>
{
    private static Dictionary<FileIndex, string> Files = new Dictionary<FileIndex, string>
    {
        { FileIndex.GAMESETTINGS, "gamesettings" },
        { FileIndex.HIGHSCORES, "highScores" },
    };

    public static void SaveToJson(T anObject, FileIndex aFileIndex)
    {
        string json = JsonUtility.ToJson(anObject, true);
        File.WriteAllText(Application.dataPath + "/TextFiles/" + Files[aFileIndex] + ".json", json);
    }

    public static T LoadFromJson(FileIndex aFileIndex)
    {
        string filePath = Application.dataPath + "/TextFiles/" + Files[aFileIndex] + ".json";
        if(!File.Exists(filePath))
            return default;

        string json = File.ReadAllText(filePath);
        T obj = JsonUtility.FromJson<T>(json);
        return obj;
    }
}
