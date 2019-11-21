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

    //public delegate void OnLevelUp();
    //public static OnLevelUp levelUp;

    public delegate void OnAddingLevelScore();
    public static OnAddingLevelScore addLevelScore;

    public delegate void OnFillUpTheMain(bool isLevelUp);
    public static OnFillUpTheMain fillUpTheMain;

    public float CurrentFillupAmount { get { return mCurrentLevelScore * (mLevelInfos.Any() ? GetLevelDataOf(mCurrentLevel).UIBarFillingSectionValue : 0); } }

    public int CurrentLevel { get { return mCurrentLevel; } }
    private int mCurrentLevel = 0;
    
    public LevelData GetCurrentLevelData { get { return GetLevelDataOf(mCurrentLevel); } }
   
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

    /// <summary>
    /// Add in point for level up to gain more score and increase the dropping speed
    /// When a certain amount of points are aquired, the current level increase and
    /// the next level up score will be increased based on the
    /// current level.
    /// From here it'll store the current max combo that player had achieved.
    /// </summary>
    /// <param name="aPoint">Points that to add to level up</param>
    public void AddLevelScore(int aScore)
    {
        mCurrentLevelScore += aScore;
        addLevelScore?.Invoke();

        if(mCurrentLevelScore >= GetLevelDataOf(mCurrentLevel).BreakLevelScore)
        {
            mCurrentLevelScore = 0;
            mCurrentLevel++;
            GameManager.Instance.UpdateDropRateAtLevelUp(mCurrentLevel);
            fillUpTheMain?.Invoke(true);
        }
    }

    public void FillUpTheMainBar()
    {
        fillUpTheMain?.Invoke(false);
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
