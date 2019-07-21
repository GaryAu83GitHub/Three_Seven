using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class JSONTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        List<PlayerData> newScores = new List<PlayerData>
        {
            new PlayerData("Albert", 10),
            new PlayerData("Becky", 5),
            new PlayerData("Carl", 15),
            new PlayerData("David", 4),
            new PlayerData("Xena", 11)
        };

        var table = newScores.OrderByDescending(s => s.Score).ThenBy(s => s.Name).ToList();
        ScoreTable saveTable = new ScoreTable(table);
        JsonTool<ScoreTable>.SaveToJson(saveTable, "highScores");
        ScoreTable loadTable = JsonTool<ScoreTable>.LoadFromJson("highScores");
        foreach (PlayerData d in loadTable.ScoreList)
            Debug.Log($"{d.Name}: {d.Score}");

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static string LoadJsonAsResource(string aPath)
    {
        string jsonFilePath = aPath.Replace(".json", "");
        TextAsset loadedJsonFile = Resources.Load<TextAsset>(aPath);

        return loadedJsonFile.text;
    }
}

[Serializable]
public class ScoreTable
{
    public List<PlayerData> ScoreList = new List<PlayerData>();

    public ScoreTable()
    { }

    public ScoreTable(List<PlayerData> aScoreList)
    {
        ScoreList = aScoreList ?? throw new ArgumentNullException(nameof(aScoreList));
    }
}

[Serializable]
public class PlayerData
{
    public string Name;
    public int Score;

    public PlayerData()
    {
        Name = "";
        Score = 0;
    }

    public PlayerData(string aName, int aScore)
    {
        Name = aName;
        Score = aScore;
    }

}


public class JsonTool<T>
{
    public static void SaveToJson(T anObject, string aFileName)
    {
        string json = JsonUtility.ToJson(anObject, true);
        File.WriteAllText(Application.dataPath + "/TextFiles/" + aFileName + ".json", json);
    }

    public static T LoadFromJson(string aFileName)
    {
        string json = File.ReadAllText(Application.dataPath + "/TextFiles/" + aFileName + ".json");
        T obj = JsonUtility.FromJson<T>(json);
        return obj;
    }
}