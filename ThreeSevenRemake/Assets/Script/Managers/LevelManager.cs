using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public delegate void OnLevelUp();
    public static OnLevelUp levelUp;

    public delegate void OnAddingLevelScore();
    public static OnAddingLevelScore addLevelScore;

    public delegate void OnFillUpTheMain();
    public static OnFillUpTheMain fillUpTheMain;
        

    public float CurrentFillupAmount { get { return mCurrentLevelScore * mLevelInfos[mCurrentLevel].UIBarFillingSectionValue; } }

    public int CurrentLevel { get { return mCurrentLevel; } }
    private int mCurrentLevel = 0;



    public LevelData GetCurrentLevelData { get { return GetLevelDataOf(mCurrentLevel); } }
    public LevelData GetNextLevelData { get { return GetLevelDataOf(mCurrentLevel + 1); } }

    private List<LevelData> mLevelInfos = new List<LevelData>();

    private int mCurrentLevelScore = 0;
    private int mCurrentLevelScoreMax = 0;

    public LevelManager()
    {
        Reset();
    }

    public void Reset()
    {
        mCurrentLevel = 0;
        mCurrentLevelScore = 0;
        mCurrentLevelScoreMax = GetLevelDataOf(mCurrentLevel).BreakLevelScore;
        //mLevelInfos.Clear();

        //mLevelInfos.Add(new LevelData(10, 1f / 10f));
    }

    public void AddLevelScore(int aScore)
    {
        mCurrentLevelScore += aScore;
        addLevelScore?.Invoke();

        if(mCurrentLevelScore >= GetLevelDataOf(mCurrentLevel).BreakLevelScore)
        {
            mCurrentLevelScore = 0;
            mCurrentLevel++;
            Debug.Log("Current level: " + mCurrentLevel.ToString());
            Debug.Log("Level " + mCurrentLevel.ToString() + 
                "'s Breakscore: " + GetLevelDataOf(mCurrentLevel).BreakLevelScore.ToString() + 
                ", section value: " + GetLevelDataOf(mCurrentLevel).UIBarFillingSectionValue.ToString());
            levelUp?.Invoke();
        }
    }

    public void FillUpTheMainBar()
    {
        fillUpTheMain?.Invoke();
    }

    private LevelData GetLevelDataOf(int anLevelIndex)
    {
        if(!mLevelInfos.Any())
            mLevelInfos.Add(new LevelData(10, 1f / 10f));

        while (mLevelInfos.Count <= anLevelIndex)
        {
            AddLevelData(mLevelInfos.Count);
        }
        return mLevelInfos[anLevelIndex];
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
