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

    public int CurrentLevel { get { return mCurrentLevel; } }
    private int mCurrentLevel = 0;


    private List<LevelData> mLevelInfos = new List<LevelData>();

    private int mCurrentLevelScore = 0;

    public LevelManager()
    {
        Reset();
    }

    public void Reset()
    {
        mCurrentLevel = 0;
        mCurrentLevelScore = 0;
        mLevelInfos.Clear();

        mLevelInfos.Add(new LevelData(10, 1f / 10f));
    }

    public void AddLevelScore(int aScore)
    {
        mCurrentLevelScore += aScore;
        if(mCurrentLevelScore >= mLevelInfos[mCurrentLevel].BreakLevelScore)
        {
            mCurrentLevelScore = 0;
            mCurrentLevel++;
            AddLevelData(mCurrentLevel);

        }
    }
    
    private void AddLevelData(int aNewLevelValue)
    {
        int newBreakScore = 10 + aNewLevelValue;
        LevelData data = new LevelData(newBreakScore, 1f / (float)newBreakScore);
        mLevelInfos.Add(data);
    }
}

public class LevelData
{
    public readonly int BreakLevelScore = 0;
    public readonly float UIBarFillingSectionValue = 0;

    public LevelData() { }

    public LevelData(int aBreakLevelScore, float aSectionValue)
    {
        BreakLevelScore = aBreakLevelScore;
        UIBarFillingSectionValue = aSectionValue;
    }

}
