using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public enum FileIndex
{
    GAMESETTINGS,
    HIGHSCORES,
    HIGHSCORES_NEW,
}

public class JsonHelper<T>
{
    private static readonly Dictionary<FileIndex, string> Files = new Dictionary<FileIndex, string>
    {
        { FileIndex.GAMESETTINGS, "gamesettings" },
        { FileIndex.HIGHSCORES, "highScores" },
        { FileIndex.HIGHSCORES_NEW, "highScoresNew" },
    };

    public static void CreateNewJsonFile(string aNewFileName, T anObject)
    {
        string json = JsonUtility.ToJson(anObject, true);
        File.WriteAllText(Application.dataPath + "/TextFiles/" + aNewFileName + ".json", json);
    }

    public static void SaveToJson(T anObject, FileIndex aFileIndex)
    {
        string json = JsonUtility.ToJson(anObject, true);
        File.WriteAllText(Application.dataPath + "/TextFiles/" + Files[aFileIndex] + ".json", json);
    }

    public static void SaveToJsonNew(T anObject, FileIndex aFileIndex)
    {
        string json = JsonUtility.ToJson(anObject, true);
        File.WriteAllText(Application.persistentDataPath + "/" + Files[aFileIndex] + ".json", json);
    }

    public static T LoadFromJsonNew(FileIndex aFileIndex)
    {
        string filePath = Application.persistentDataPath + "/" + Files[aFileIndex] + ".json";
        if (!File.Exists(filePath))
            return default;

        string json = File.ReadAllText(filePath);
        T obj = JsonUtility.FromJson<T>(json);
        return obj;
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