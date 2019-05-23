using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the main handler for the scoring, level up handle during the gameplay.
/// It'll be calculating of the scoring from each "soft"landing blocks (blocks that landed yet didn't
/// achieve any scoring combo) and "combo" scoring
/// 
/// Except storing the score, it'll be storing the current level and increase the level for scoring
/// and dropping speed
/// </summary>
public class GameManager
{
    public static GameManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new GameManager();
            return mInstance;
        }
    }
    private static GameManager mInstance;

    private int mCurrentLevel = 0;
    public int CurrentLevel { get { return mCurrentLevel; } }

    private int mCurrentScore = 0;
    public int CurrentScore { get { return mCurrentScore; } }

    private uint mComboScore = 0;
    public uint ComboScore { get { return mComboScore; } }

    public delegate void OnScoreChange(int aNewScore);
    public static OnScoreChange scoreChanging;

    public delegate void OnLevelChange(int aNewLevel);
    public static OnLevelChange levelChanging;

    public delegate void OnComboOccures(int aComboCount, int aComboScore, string aComboText);
    public static OnComboOccures comboOccuring;

    private List<uint> mComboBaseScoreList = new List<uint>() { 0, 50 };

    private int mNextLevelUpScore = 10;
    private int mCurrentLevelPoint = 0;

    private const int mSoftLandingScore = 1;
    private const float mDropRateDecreaseValue = .03f;

    public void AddLevelPoint(int aPoint)
    {
        mCurrentLevelPoint += aPoint;

        if(mCurrentLevelPoint > mNextLevelUpScore)
        {
            mCurrentLevelPoint = 0;
            mNextLevelUpScore = 10 * (mCurrentLevel + 1);
            mCurrentLevel++;
        }
    }

    public void AddSoftScore()
    {
        mCurrentScore += mSoftLandingScore + mCurrentLevel;

        scoreChanging?.Invoke(mCurrentScore);
    }

    public void SetComboScore(int aCombo)
    {
        mComboScore = 0;

        
        mComboScore = (uint)(GetComboBaseScore(aCombo) * (mCurrentLevel + 1));

        mCurrentScore += (int)mComboScore;
        scoreChanging?.Invoke(mCurrentScore);

        if (aCombo > 0)
            comboOccuring?.Invoke(aCombo, (int)mComboScore, "");
    }

    public float GetCurrentDroppingRate()
    {
        float droprate = 1f;

        droprate -= (mCurrentLevel * mDropRateDecreaseValue);

        if (droprate <= 0.1f)
            droprate = 0.1f;

        return droprate;
    }

    /// <summary>
    /// This method is called when cubes scores throught aligning and met up the condition
    /// The function is to get the correct combo score accordingly by the number of combos the new landed
    /// cubes had invoke
    /// This is based on Get funciotn from the Factory pattern by take and seek for the requestedindex in 
    /// the list.
    /// But if the requesting index is out of bound of the list current limit, the list will be upgraded 
    /// through the Create method
    /// </summary>
    /// <param name="aCombo">Requested index</param>
    /// <returns>return the combo score base on the requested index</returns>
    private int GetComboBaseScore(int aCombo)
    {
        if (aCombo >= mComboBaseScoreList.Count)
            CreateNewBaseScore(aCombo);

        return (int)mComboBaseScoreList[aCombo];
    }

    /// <summary>
    /// This method is use to upgrade the list of combo base scores.
    /// By sending in a requested combo limit value, the list will go through a iteration of adding new
    /// base score into it.
    /// </summary>
    /// <param name="aNewComboLimit">the new high combo that the player had made</param>
    private void CreateNewBaseScore(int aNewComboLimit)
    {
        do
        {
            int lastIndex = mComboBaseScoreList.Count - 1;
            uint nextBaseScore = mComboBaseScoreList[lastIndex] * ((uint)lastIndex + 1);
            mComboBaseScoreList.Add(nextBaseScore);

        } while (mComboBaseScoreList.Count <= aNewComboLimit);
    }
}
