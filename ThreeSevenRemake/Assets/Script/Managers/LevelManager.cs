using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager
{
    public static LevelManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new LevelManager();
            return mInstance;
        }
    }
    private static LevelManager mInstance;

    public int CurrentLevel { get { return LevelInfos.Count - 1; } }


    public List<LevelData> LevelInfos = new List<LevelData>()
    {
        new LevelData(10, 1f/10f)
    };

    private int mCurrentLevelScore = 0;

    public void AddLevelScore(int aScore)
    {
        mCurrentLevelScore += aScore;
    }

    public int GetNextLevel()
    {
        return LevelInfos.Count - 1;
    }

    private int NextLevelScoreCalc()
    {
        return 10 + LevelInfos.Count;
    }

    private float NextLevelSectionValue()
    {
        return 1f / NextLevelScoreCalc();
    }
}

public class LevelData
{
    public readonly int NextLevelScore = 0;
    public readonly float UIBarFillingSectionValue = 0;

    public LevelData() { }

    public LevelData(int aNextLevelScore, float aSectionValue)
    {
        NextLevelScore = aNextLevelScore;
        UIBarFillingSectionValue = aSectionValue;
    }

}
