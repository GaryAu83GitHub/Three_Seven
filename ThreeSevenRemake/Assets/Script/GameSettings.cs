using System.Collections.Generic;
using UnityEngine;

public enum Difficulties
{
    EASY,
    NORMAL,
    HARD
}

public enum ScoreCubeCount
{
    TWO_CUBES,
    THREE_CUBES,
    FOUR_CUBES,
    FIVE_CUBES,
    MAX
}
/// <summary>
/// This class is for storing the gameplay setting that can be change when the player chose to start a new game.
/// </summary>
public class GameSettings
{
    public static GameSettings Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new GameSettings();
            return mInstance;
        }
    }
    private static GameSettings mInstance;

    private Difficulties mDifficulties = Difficulties.EASY;
    public Difficulties Difficulty { get { return mDifficulties; } }

    private int mLimitRow = 17;
    public int LimitHigh { get { return mLimitRow; } }

    private int mDropSpeedMultiply = 0;
    public int StartSpeedMultiply { get { return mDropSpeedMultiply; } }

    private int mAdditionLimit = 0;
    public int AdditionLimit { get { return mAdditionBaseValue + mAdditionLimit; } }

    private List<bool> mEnableScoringMethods = new List<bool>();
    public List<bool> EnableScoringMethods { get { return mEnableScoringMethods; } }

    private const int mMaxLimitRow = 18;
    private const int mMinLimitRow = 9;
    private const int mAdditionBaseValue = 18;

    public GameSettings()
    {
        for(ScoreCubeCount i = 0; i < (ScoreCubeCount.MAX); i++)
        {
            mEnableScoringMethods.Add(true);
        }
    }

    public void SetDifficulty(Difficulties aDifficulty)
    {
        mDifficulties = aDifficulty;
    }

    public void SetScoringCubesCount(ScoreCubeCount anIndex, bool isActive)
    {
        mEnableScoringMethods[(int)anIndex] = isActive;
    }

    public bool IsScoringMethodActiveTo(ScoreCubeCount anIndex)
    {
        return mEnableScoringMethods[(int)anIndex];
    }

    public void SwapScoringCubeCountOn(ScoreCubeCount anIndex)
    {
        bool isThereAnotherOptionEnable = false;

        for(ScoreCubeCount i = ScoreCubeCount.TWO_CUBES; i < ScoreCubeCount.MAX; i++)
        {
            if (i == anIndex)
                continue;
            if (isThereAnotherOptionEnable == false && mEnableScoringMethods[(int)i] == true)
                isThereAnotherOptionEnable = mEnableScoringMethods[(int)i];

        }
        if(isThereAnotherOptionEnable)
            mEnableScoringMethods[(int)anIndex] = !mEnableScoringMethods[(int)anIndex];
    }

    public bool IsTheseScoringSettingEquals(bool isTwoCubeEnable, bool isThreeCubeEnable, bool isFourCubeEnable, bool isFiveCubeEnable)
    {
        if (mEnableScoringMethods[0] == isTwoCubeEnable &&
            mEnableScoringMethods[1] == isThreeCubeEnable &&
            mEnableScoringMethods[2] == isFourCubeEnable &&
            mEnableScoringMethods[3] == isFiveCubeEnable)
            return true;
        return false;
    }

    public void SetStartDropSpeed(int aSpeed)
    {
        mDropSpeedMultiply = aSpeed;
    }

    /// <summary>
    /// Use to set the high of the limit line in the game.
    /// It's purpose is for future of adding in the setting to let player chose the challenge level
    /// </summary>
    /// <param name="aLimitLineRow">Requesting limit row</param>
    /// <returns>Return the row number that had been clamp between the max and min row number</returns>
    public void SetLimitLineLevel(int aLimitLineRow)
    {
        mLimitRow = Mathf.Clamp(aLimitLineRow, mMinLimitRow, mMaxLimitRow);
    }

    public void SetAdditionLimit(int aLimit)
    {
        mAdditionLimit = aLimit;
    }
}
