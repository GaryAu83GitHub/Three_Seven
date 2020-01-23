using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResultData
{
    /// <summary>
    /// Main subject for Score result
    /// Total score that had gain during the play throught
    /// </summary>
    private int mScore = 0;
    public int TotalScores { get { return mScore; } }
    /// <summary>
    /// Sub issue for Score result
    /// The best score that ever had made among the numbers of rounds
    /// during the play throught
    /// </summary>
    private int mRoundScore = 0;
    public int BestRoundScores { get { return mRoundScore; } }
    /// <summary>
    /// Sub issue for Score result
    /// The highest bonus term ever had reached during the play throught
    /// </summary>
    private int mBonusTerm = 1;
    public int BestBonusTerm { get { return mBonusTerm; } }
    /// <summary>
    /// Sub issue for Score result
    /// Best combo count that ever made in the play throught
    /// </summary>
    private int mComboCount = 0;
    public int BestComboCount { get { return mComboCount; } }
    /// <summary>
    /// Set up the result data for Score
    /// </summary>
    /// <param name="aGainScores">Total score</param>
    /// <param name="aRoundScore">Best round score</param>
    /// <param name="aBonusTerm">Highest bonus term</param>
    /// <param name="aComboCount">Best combo count</param>
    public void SetGainScores(int aGainScores, int aRoundScore, int aBonusTerm, int aComboCount)
    {
        mScore = aGainScores;
        mRoundScore = aRoundScore;
        mBonusTerm = aBonusTerm;
        mComboCount = aComboCount;
    }

    /// <summary>
    /// Main subject for Chain result
    /// The longest chain that ever made during the play throught.
    /// </summary>
    private int mChain = 0;
    public int LongestChains { get { return mChain; } }
    /// <summary>
    /// Sub issue for Chain result
    /// The best consecutive increase chain had ever made during the play throught
    /// </summary>
    private int mConsecutiveIncrease = 0;
    public int BestConsecutiveIncreaseChain { get { return mConsecutiveIncrease; } }
    /// <summary>
    /// Sub issue for Chain result
    /// The worst consecutive decrease chain had ever made during the play throught
    /// </summary>
    private int mConsecutiveDecrease = 0;
    public int WorstConsecutiveDecreaseChain { get { return mConsecutiveDecrease; } }
    /// <summary>
    /// Set up the result data for Chain
    /// </summary>
    /// <param name="aLongestChainCount">Longest chain</param>
    /// <param name="aConsecutiveIncrease">Best consective chain increasment</param>
    /// <param name="aConsecutiveDecrease">Worst consective chain decreasment</param>
    public void SetLongestChain(int aLongestChainCount, int aConsecutiveIncrease, int aConsecutiveDecrease)
    {
        mChain = aLongestChainCount;
        mConsecutiveIncrease = aConsecutiveIncrease;
        mConsecutiveDecrease = aConsecutiveDecrease;
    }

    /// <summary>
    /// Main subject for Task result
    /// The total number of completed tasks
    /// </summary>
    private int mCompletedTaskCount = 0;
    public int TotalCompletedTaskCount { get { return mCompletedTaskCount; } }
    /// <summary>
    /// Sub issue for Task result
    /// The best number of task that was completed in the last round during the play 
    /// throught
    /// </summary>
    private int mRoundCompleteTaskCount = 0;
    public int BestCompletedTaskCount { get { return mRoundCompleteTaskCount; } }
    /// <summary>
    /// Sub issue for Task result
    /// The highest count a single task ever had scored in a single round during the play
    /// throught
    /// </summary>
    private int mSingleTaskScoringCount = 0;
    public int BestSingleTaskScoringCount { get { return mSingleTaskScoringCount; } }
    /// <summary>
    /// Set up result data for Task 
    /// </summary>
    /// <param name="aCompletedTaskCount">Total completed task</param>
    /// <param name="aRoundCompletedTaskCount">Best count of completed tasks in one round</param>
    /// <param name="aSingleTaskScoringCount">Best score count a single task had made</param>
    public void SetCompletedTasks(int aCompletedTaskCount, int aRoundCompletedTaskCount, int aSingleTaskScoringCount)
    {
        mCompletedTaskCount = aCompletedTaskCount;
        mRoundCompleteTaskCount = aRoundCompletedTaskCount;
        mSingleTaskScoringCount = aSingleTaskScoringCount;
    }

    /// <summary>
    /// Main subject for Level result
    /// </summary>
    private int mLevelCount = 0;
    public int GainedLevelCount { get { return mLevelCount; } }
    /// <summary>
    /// Sub issue for Level result
    /// The highest level ever had reached during the play throught
    /// </summary>
    private int mHighestLevel = 0;
    public int HighestReachedLevel { get { return mHighestLevel; } }
    /// <summary>
    /// Sub issue for Level result
    /// The highest odds value on threshold bar had ever reached during the play throught
    /// </summary>
    private float mHighestThresholdOdds = 0f;
    public float HighestThreshold { get { return mHighestThresholdOdds; } }
    /// <summary>
    /// Sub issue for Level result
    /// The lowest odds value on threshold bar had ever fall during the play throught
    /// </summary>
    //private float mLowestThresholdOdds = 0f;
    //public float LowestThreshold { get { return mLowestThresholdOdds; } }
    /// <summary>
    /// Set up result data for Level 
    /// </summary>
    /// <param name="aGainedLevelCount">Total times the level pass its best</param>
    /// <param name="aHighestLevel">Best level had reached</param>
    /// <param name="aHighestOdds">Highest threshold odds value</param>
    /// <param name="aLowestOdds">Lowest threshold odds value</param>
    public void SetGainedLevels(int aGainedLevelCount, int aHighestLevel, float aHighestOdds/*, float aLowestOdds*/)
    {
        mLevelCount = aGainedLevelCount;
        mHighestLevel = aHighestLevel;
        mHighestThresholdOdds = aHighestOdds;
        //mLowestThresholdOdds = aLowestOdds;
    }

    /// <summary>
    /// Main subject for Odds result
    /// Final scoring odds of the play throught
    /// </summary>
    private float mOdds = 0f;
    public float AverageOdds { get { return mOdds; } }
    /// <summary>
    /// Sub issue for Odds result
    /// Total count of times scorings had occured when a block had landed during the play
    /// throught
    /// </summary>
    private int mScoreTimeCount = 0;
    public int ScoreTimes { get { return mScoreTimeCount; } }
    /// <summary>
    /// Sub issue for Odds result
    /// Total number of blocks that had landed during the play throught
    /// </summary>
    private int mLandedBlocksCount = 0;
    public int LandedBlocks { get { return mLandedBlocksCount; } }
    /// <summary>
    /// Set up result data for Odds
    /// </summary>
    /// <param name="aScoreTimeCount"></param>
    /// <param name="aLandedBlockCount"></param>
    public void SetAverageOdds(int aScoreTimeCount, int aLandedBlockCount)
    {
        mScoreTimeCount = aScoreTimeCount;
        mLandedBlocksCount = aLandedBlockCount;

        mOdds = (float)mScoreTimeCount / (float)mLandedBlocksCount;
    }

    /// <summary>
    /// Main subject for Time result
    /// Total of seconds that had been played during the play throught
    /// </summary>
    private int mTime = 0;
    public int PlayTime { get { return mTime; } }
    /// <summary>
    /// Displaying string
    /// </summary>
    private string mTimeString = "";
    public string TimeString { get { return mTimeString; } }
    /// <summary>
    /// Set up result data for Time
    /// </summary>
    /// <param name="aUsedTime">Total seconds had passed</param>
    /// <param name="aTimeLimit">A time limit in seconds in game mode that is time based</param>
    public void SetPlayTime(int aUsedTime, int aTimeLimit)
    {
        if (aTimeLimit > 0)
        {
            mTime = aTimeLimit - aUsedTime;
        }
        else
            mTime = aUsedTime;

        //int seconds = (mTime % 60);
        //int minutes = ((mTime / 60) % 60);
        //int hours = ((mTime / 3600) % 60);

        //if (hours > 0)
        //    mTimeString = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        //else
        //    mTimeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        mTimeString = TimeTool.TimeString(mTime);

    }

    private string mName = "";
    public string PlayerName { get { return mName; } }
    public void SetPlayerNameTheme(string aPlayerName) { mName = aPlayerName; }

    private readonly bool[] mEnableDigits = new bool[4] { false, false, false, false };
    public bool[] EnableDigits { get { return mEnableDigits; } }
    private int mEnableDigitInterger = 0;
    public int EnambleDigitInterger { get { return mEnableDigitInterger; } }
    public void SetEnableDigits(bool[] someEnableDigits)
    {
        for (int i = 0; i < someEnableDigits.Length; i++)
        {
            mEnableDigits[i] = someEnableDigits[i];
            if (i == 0 && mEnableDigits[i])
                mEnableDigitInterger += 1;
            else if (i == 1 && mEnableDigits[i])
                mEnableDigitInterger += 2;
            else if (i == 2 && mEnableDigits[i])
                mEnableDigitInterger += 4;
            else if (i == 3 && mEnableDigits[i])
                mEnableDigitInterger += 8;
        }
    }

    private GameMode mMode = GameMode.CLASSIC;
    public GameMode SelectedMode { get { return mMode; } }
    public void SetGameMode(GameMode aMode) { mMode = aMode; }

    public ResultData()
    {
        mScore = 0;
        mRoundScore = 0;
        mBonusTerm = 0;
        mComboCount = 0;

        mChain = 0;
        mConsecutiveIncrease = 0;
        mConsecutiveDecrease = 0;

        mCompletedTaskCount = 0;
        mRoundCompleteTaskCount = 0;
        mSingleTaskScoringCount = 0;

        mLevelCount = 0;
        mHighestLevel = 0;
        mHighestThresholdOdds = 0f;
        //mLowestThresholdOdds = 0f;

        mTime = 0;

        mOdds = 0f;
        mScoreTimeCount = 0;
        mLandedBlocksCount = 0;

        mName = "";
        mEnableDigits = new bool[4] { false, false, false, false };
        mMode = GameMode.CLASSIC;
    }

    public ResultData(ResultData aData)
    {
        mScore = aData.TotalScores;
        mRoundScore = aData.BestRoundScores;
        mBonusTerm = aData.BestBonusTerm;
        mComboCount = aData.BestComboCount;

        mChain = aData.LongestChains;
        mConsecutiveIncrease = aData.BestConsecutiveIncreaseChain;
        mConsecutiveDecrease = aData.WorstConsecutiveDecreaseChain;

        mCompletedTaskCount = aData.TotalCompletedTaskCount;
        mRoundCompleteTaskCount = aData.BestCompletedTaskCount;
        mSingleTaskScoringCount = aData.BestSingleTaskScoringCount;

        mLevelCount = aData.GainedLevelCount;
        mHighestLevel = aData.HighestReachedLevel;
        mHighestThresholdOdds = aData.HighestThreshold;
        //mLowestThresholdOdds = aData.LowestThreshold;

        mTime = aData.PlayTime;

        mOdds = aData.AverageOdds;
        mScoreTimeCount = aData.ScoreTimes;
        mLandedBlocksCount = aData.LandedBlocks;

        mName = aData.PlayerName;
        mEnableDigits = aData.EnableDigits;
        mMode = aData.SelectedMode;
    }
}
