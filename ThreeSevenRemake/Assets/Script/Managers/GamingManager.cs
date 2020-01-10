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
public class GamingManager
{
    public static GamingManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new GamingManager();
            return mInstance;
        }
    }
    private static GamingManager mInstance;

    private int mCurrentScore = 0;
    public int CurrentScore { get { return mCurrentScore; } }

    private uint mComboScore = 0;
    public uint ComboScore { get { return mComboScore; } }

    private int mCurrentMaxCombo = 0;
    public int MaxCombo { get { return mCurrentMaxCombo; } set { mCurrentMaxCombo = value; } }

    private float mCurrentDropRate = (float)Constants.DROPPING_INITIAL_SPEED;
    public float DropRate { get { return (float)MathTools.ExponentialFormula(Constants.DROPPING_INITIAL_SPEED, Constants.DROPPING_COEFFICIENT, (double)LevelManager.Instance.CurrentLevel);/*mCurrentDropRate*/; } }
    // this should not be used anymore
    public float BlockNextDropTime { get { return Time.time + DropRate; } }

    public float GameTime { get; set; }

    private string mGameTimeString = "";
    public string GameTimeString { get { return mGameTimeString; } set { mGameTimeString = value; } }

    private int mLandedBlockCount = 0;
    public int LandedBlockCount { get { return mLandedBlockCount; } set { mLandedBlockCount = value;} }

    // delegates
    public delegate void OnScoreChange(int aNewScore, int anAddOnScore);
    public static OnScoreChange scoreChanging;

    public delegate void OnRewinding();
    public static OnRewinding rewinding;

    public delegate void OnNewBlockLandedScores();
    public static OnNewBlockLandedScores newBlockLandedScores;

    public delegate void OnLinkingCubeScores(int aCubeCount);
    public static OnLinkingCubeScores linkingCubeScores;

    public delegate void OnComboScoring(int aComboCount);
    public static OnComboScoring comboScoring;


    public List<int> NextCubeNumbers { get { return mNextBlockNumbers; } }
    private List<int> mNextBlockNumbers = new List<int>();

    public void Reset()
    {
        mCurrentScore = 0;
        mComboScore = 0;
        mCurrentMaxCombo = 0;
        mLandedBlockCount = 0;
        GameTime = 0;
        //LevelManager.Instance.Reset();

        GenerateNewCubeNumber();
    }
    
    public void AddScore(ScoreType anObtainScoreType, int anValue = 0, TaskRank anObjective = TaskRank.X1)
    {
        //int addOn = 0;

        switch(anObtainScoreType)
        {
            case ScoreType.LINKING:
                //addOn = ScoreCalculatorcs.LinkingScoreCalculation(anObjective, anValue);
                linkingCubeScores?.Invoke(anValue);
                break;
            case ScoreType.COMBO:
                //addOn = ScoreCalculatorcs.ComboScoreCalculation(anValue);
                comboScoring?.Invoke(anValue);
                break;
            default:
                //addOn = ScoreCalculatorcs.OriginalBlockLandingScoreCalculation();
                newBlockLandedScores?.Invoke();
                break;
        }

        //if (addOn > 0)
        //{
        //    mCurrentScore += addOn;
        //    scoreChanging?.Invoke(mCurrentScore, addOn);
        //}
    }

    public List<int> GenerateNewCubeNumber()
    {
        mNextBlockNumbers.Clear();

        mNextBlockNumbers.Add(CubeNumberManager.Instance.GetNewRootNumber);
        mNextBlockNumbers.Add(CubeNumberManager.Instance.GetNewSubNumber);

        return mNextBlockNumbers;
    }

    public void SwapWithOriginalNumbers(List<int> anOriginalBlockNumbers)
    {
        mNextBlockNumbers.Clear();
        mNextBlockNumbers = new List<int>(anOriginalBlockNumbers);
    }

    public void RewindNextNumber(List<int> aPreviousNextNumber)
    {
        mNextBlockNumbers.Clear();
        mNextBlockNumbers = new List<int>(aPreviousNextNumber);

        rewinding?.Invoke();
    }

    public GameRoundData GetRoundData()
    {
        GameRoundData data = new GameRoundData
        {
            PlayerName = GameSettings.Instance.PlayerName,
            CurrentLevel = LevelManager.Instance.CurrentLevel,
            CurrentScore = CurrentScore,
            MaxCombo = MaxCombo,
            GameTime = GameTime,
            LandedBlockCount = LandedBlockCount,
            EnableScoringMethods = GameSettings.Instance.EnableScoringMethods,
        };

        return data;
    }
    
    /// <summary>
    /// Get the droprate on the block base on the current level.
    /// The the decreasing droprate falls below the minimal speed, it'll remain on the minimal speed
    /// </summary>
    /// <returns>Return the new droprate</returns>
    public float GetCurrentDroppingRate()
    {
        float droprate = Constants.MAXIMAL_DROPRATE;

        droprate -= ((LevelManager.Instance.CurrentLevel + GameSettings.Instance.StartSpeedMultiply) * Constants.DROPPING_VALUE);

        if (droprate <= Constants.MINIMAL_DROPRATE)
            droprate = Constants.MINIMAL_DROPRATE;

        return droprate;
    }

    public void UpdateDropRateAtLevelUp(int theNewLevelValue)
    {
        if (theNewLevelValue > 100)
            theNewLevelValue = 100;

        mCurrentDropRate = (float)MathTools.ExponentialFormula(Constants.DROPPING_INITIAL_SPEED, Constants.DROPPING_COEFFICIENT, (double)theNewLevelValue);
    }
}
