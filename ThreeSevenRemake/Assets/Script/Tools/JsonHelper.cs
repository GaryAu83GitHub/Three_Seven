﻿using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public enum FileIndex
{
    GAMESETTINGS,
    HIGHSCORES,
    HIGHSCORES_NEW,
    CLASSIC_LIST,
    SURVIVE_LIST
}

public class JsonHelper<T>
{
    private static readonly Dictionary<FileIndex, string> Files = new Dictionary<FileIndex, string>
    {
        { FileIndex.GAMESETTINGS, "gamesettings" },
        { FileIndex.HIGHSCORES, "highScores" },
        { FileIndex.HIGHSCORES_NEW, "highScoresNew" },
        { FileIndex.CLASSIC_LIST, "classic" },
        { FileIndex.SURVIVE_LIST, "survive" },
    };

    public static void SaveToJson(T anObject, FileIndex aFileIndex)
    {
        string json = JsonUtility.ToJson(anObject, true);
        File.WriteAllText(Application.persistentDataPath + "/" + Files[aFileIndex] + ".json", json);
    }

    public static T LoadFromJson(FileIndex aFileIndex)
    {
        string filePath = Application.persistentDataPath + "/" + Files[aFileIndex] + ".json";
        if (!File.Exists(filePath))
            return default;

        string json = File.ReadAllText(filePath);
        T obj = JsonUtility.FromJson<T>(json);
        return obj;
    }
}