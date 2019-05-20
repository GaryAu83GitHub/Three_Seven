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

    private int mComboScore = 0;
    public int ComboScore { get { return mComboScore; } }


    private int[] mBaseScoreList = new int[] {50, 100, 300, 1200, 6000};

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
        }
    }

    public void AddSoftScore()
    {
        mCurrentScore += mSoftLandingScore + mCurrentLevel;
    }

    public void SetComboScore(int aCombo)
    {
        mComboScore = 0;

        if(aCombo < mBaseScoreList.Length)
            mComboScore = mBaseScoreList[aCombo] * (mCurrentLevel + 1);
        else
            mComboScore = mBaseScoreList[mBaseScoreList.Length - 1] * (mCurrentLevel + 1);

        mCurrentScore += mComboScore;
    }

    public float GetCurrentDroppingRate()
    {
        float droprate = 1f;

        droprate -= (mCurrentLevel * mDropRateDecreaseValue);

        if (droprate <= 0.1f)
            droprate = 0.1f;

        return droprate;
    }
}
