using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResultData
{
    private int mScore = 0;
    public int GainScores { get { return mScore; } }
    public void SetGainScores(int aGainScores) { mScore = aGainScores; }

    private int mChain = 0;
    public int LongestChains { get { return mChain; } }
    public void SetLongestChain(int aLongestChainCount) { mChain = aLongestChainCount; }

    private int mTask = 0;
    public int CompletedTasks { get { return mTask; } }
    public void SetCompletedTasks(int aCompletedTaskCount) { mTask = aCompletedTaskCount; }

    private int mLevels = 0;
    public int ReachedLevels { get { return mLevels; } }
    public void SetReachedLevels(int aReachedLevels) { mLevels = aReachedLevels; }

    private float mOdds = 0f;
    private int mScoreTimeCount = 0;
    private int mLandedBlocksCount = 0;
    public float AverageOdds { get { return mOdds; } }
    public void SetAverageOdds(int aScoreTimeCount, int aLandedBlockCount)
    {
        mScoreTimeCount = aScoreTimeCount;
        mLandedBlocksCount = aLandedBlockCount;

        mOdds = (float)mScoreTimeCount / (float)mLandedBlocksCount;
    }

    private float mTime = 0f;
    public float PlayTime { get { return mTime; } }
    private string mTimeString = "";
    public string TimeString { get { return mTimeString; } }
    public void SetPlayTime(float aUsedTime, float aTimeLimit)
    {
        if (aTimeLimit > 0)
        {
            mTime = aTimeLimit - aUsedTime;
        }
        else
            mTime = aUsedTime;

        int seconds = (int)(mTime % 60);
        int minutes = (int)((mTime / 60) % 60);
        int hours = (int)((mTime / 3600) % 60);

        if (hours > 0)
            mTimeString = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        else
            mTimeString = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    private string mName = "";
    public string PlayerName { get { return mName; } }
    public void SetPlayerName(string aPlayerName) { mName = aPlayerName; }

    public ResultData()
    {
        mScore = 0;
        mChain = 0;
        mTask = 0;
        mLevels = 0;
        mOdds = 0f;
        mTime = 0f;
        mName = "";
    }

    public ResultData(ResultData aData)
    {
        mScore = aData.GainScores;
        mChain = aData.LongestChains;
        mTask = aData.CompletedTasks;
        mLevels = aData.ReachedLevels;
        mOdds = aData.AverageOdds;
        mTime = aData.PlayTime;
        mName = aData.PlayerName;
    }
}
