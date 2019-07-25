using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private int mCurrentMaxCombo = 0;
    public int MaxCombo { get { return mCurrentMaxCombo; } set { mCurrentMaxCombo = value; } }

    public float GameTime { get; set; }

    private string mGameTimeString = "";
    public string GameTimeString { get { return mGameTimeString; } set { mGameTimeString = value; } }

    private int mLandedBlockCount = 0;
    public int LandedBlockCount { get { return mLandedBlockCount; } set { mLandedBlockCount = value;} }

    // delegates
    public delegate void OnScoreChange(int aNewScore, int anAddOnScore);
    public static OnScoreChange scoreChanging;

    public delegate void OnLevelChange(int aNewLevel, int aCurrentLevelScore, int aNextLevelUpScore);
    public static OnLevelChange levelChanging;

    public delegate void OnLevelChangeNew(int anAddOn);
    public static OnLevelChangeNew levelChangingNew;

    public delegate void OnComboOccures(int aComboCount, int aComboScore, string aComboText);
    public static OnComboOccures comboOccuring;
    
    // variable
    private List<uint> mComboBaseScoreList = new List<uint>() { 50 };

    public int NextLevelUpScore { get { return mNextLevelUpScore; } }
    private int mNextLevelUpScore = 10;
    private int mCurrentLevelPoint = 0;

    //private const int mSoftLandingScore = 1;
    //private const float DROPPING_VALUE = .03f;
    //private const float MINIMAL_DROPRATE = .1f;
    //private const float MAXIMAL_DROPRATE = 1f;

    public void Reset()
    {
        mCurrentLevel = 0;
        mCurrentScore = 0;
        mComboScore = 0;
        mCurrentMaxCombo = 0;
        mLandedBlockCount = 0;
        mComboBaseScoreList = new List<uint>() { 50 };
        mNextLevelUpScore = 10;
        mCurrentLevelPoint = 0;
    }

    /// <summary>
    /// Add in point for level up to gain more score and increase the dropping speed
    /// When a certain amount of points are aquired, the current level increase and
    /// the next level up score will be increased based on the
    /// current level.
    /// From here it'll store the current max combo that player had achieved.
    /// </summary>
    /// <param name="aPoint">Points that to add to level up</param>
    public void AddLevelPoint(int aPoint)
    {
        //if (aPoint > mCurrentMaxCombo)
        //    mCurrentMaxCombo = aPoint;

        mCurrentLevelPoint += aPoint;

        if (mCurrentLevelPoint > mNextLevelUpScore)
        {
            int restScore = mCurrentLevelPoint - mNextLevelUpScore;
            mCurrentLevelPoint = restScore;
            mCurrentLevel++;
            mNextLevelUpScore = 10 + (mCurrentLevel);
            Objective.Instance.IncreaseObjectiveValue();
        }
        levelChangingNew?.Invoke(aPoint);
        //levelChanging?.Invoke(mCurrentLevel, mCurrentLevelPoint, mNextLevelUpScore);
    }

    public void AddScore(ScoreType anObtainScoreType, int anValue = 0, TaskRank anObjective = TaskRank.X1)
    {
        int addOn = 0;

        switch(anObtainScoreType)
        {
            case ScoreType.LINKING:
                addOn = ScoreCalculatorcs.LinkingScoreCalculation(anObjective, anValue);
                break;
            case ScoreType.COMBO:
                addOn = ScoreCalculatorcs.ComboScoreCalculation(anValue);
                break;
            default:
                addOn = ScoreCalculatorcs.OriginalBlockLandingScoreCalculation();
                break;
        }

        if (addOn > 0)
        {
            mCurrentScore += addOn;
            scoreChanging?.Invoke(mCurrentScore, addOn);
        }
    }

    public GameRoundData GetRoundData()
    {
        GameRoundData data = new GameRoundData
        {
            PlayerName = GameSettings.Instance.PlayerName,
            CurrentLevel = CurrentLevel,
            CurrentScore = CurrentScore,
            MaxCombo = MaxCombo,
            GameTime = GameTime,
            LandedBlockCount = LandedBlockCount,
            EnableScoringMethods = GameSettings.Instance.EnableScoringMethods,
        };

        return data;
    }

    /// <summary>
    /// Add score based on the current level that is scored by have the current block landed.
    /// </summary>
    public void AddSoftScore()
    {
        int addOn = Constants.ORIGINAL_BLOCK_LANDING_SCORE + mCurrentLevel;
        mCurrentScore += addOn;

        scoreChanging?.Invoke(mCurrentScore, addOn);
    }

    /// <summary>
    /// Add scores based on the current level that was achieved fullfilling the scoring condition.
    /// </summary>
    /// <param name="aCombo"></param>
    public void AddComboScore(int aCombo)
    {
        mComboScore = 0;

        //mComboScore = //(uint)(GetComboBaseScore(aCombo) * (mCurrentLevel + 1));
        int comboScore = ScoreCalculatorcs.ComboScoreCalculation(aCombo);
        mCurrentScore += comboScore;//(int)mComboScore; // * Objective.Instance.ObjectiveAchieveBonus();
        scoreChanging?.Invoke(mCurrentScore, comboScore/*(int)mComboScore*/);

        if (aCombo > 0)
            comboOccuring?.Invoke(aCombo, (int)mComboScore, "");
    }

    public void AddLinkingScore(TaskRank anObjective, int aNumberCount)
    {
        //int objectiveMultiply = 1;
        //if (anObjective == TaskRank.X10)
        //    objectiveMultiply = 10;
        //else if (anObjective == TaskRank.X5)
        //    objectiveMultiply = 5;
        //else
        //    objectiveMultiply = 1;

        //int linkedScore = (mCurrentLevel + 1) + (aNumberCount * objectiveMultiply);
        //mCurrentScore += linkedScore;
        int addOn = ScoreCalculatorcs.LinkingScoreCalculation(anObjective, aNumberCount);
        mCurrentScore += addOn;
        scoreChanging?.Invoke(mCurrentScore, addOn);
    }
    
    /// <summary>
    /// Get the droprate on the block base on the current level.
    /// The the decreasing droprate falls below the minimal speed, it'll remain on the minimal speed
    /// </summary>
    /// <returns>Return the new droprate</returns>
    public float GetCurrentDroppingRate()
    {
        float droprate = Constants.MAXIMAL_DROPRATE;

        droprate -= ((mCurrentLevel + GameSettings.Instance.StartSpeedMultiply) * Constants.DROPPING_VALUE);

        if (droprate <= Constants.MINIMAL_DROPRATE)
            droprate = Constants.MINIMAL_DROPRATE;

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
        if (aCombo == 0)
            return 0;

        if (aCombo >= mComboBaseScoreList.Count)
            CreateNewBaseScore(aCombo);

        return (int)mComboBaseScoreList[aCombo - 1];
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
