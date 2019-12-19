using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum TableCategory
{
    SCORE,
    COMBO,
    TIME,
    USED_BLOCK
};
public class HighScoreManager
{
    public static HighScoreManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new HighScoreManager();
            return mInstance;
        }
    }
    private static HighScoreManager mInstance;

    private List<RoundResultData> mHighScoreList = new List<RoundResultData>();
    public List<RoundResultData> HighScoreList { get { return mHighScoreList; } }

    /// <summary>
    /// new score list
    /// </summary>
    private List<SavingResultData> mScoreList = new List<SavingResultData>();
    public List<SavingResultData> ScoreList { get { return mScoreList; } }

    public HighScoreManager()
    {
        //ScoreTable loadTable = (JsonHelper<ScoreTable>.LoadFromJson(FileIndex.HIGHSCORES) ?? new ScoreTable());
        //foreach(RoundResultData data in loadTable.ScoreList)
        //{
        //    mHighScoreList.Add(data);
        //}

        HighScoreTable loadTable = (JsonHelper<HighScoreTable>.LoadFromJsonNew(FileIndex.HIGHSCORES_NEW) ?? new HighScoreTable());
        foreach(SavingResultData data in loadTable.ScoreList)
        {
            mScoreList.Add(data);
        }
    }

    public void Add(GameRoundData aRoundData)
    {
        RoundResultData newData = new RoundResultData(aRoundData);
        if (mHighScoreList.Contains(newData))
            return;

        mHighScoreList.Add(newData);

        SaveToList();
    }

    public void Add(string aPlayerName, GameRoundData aRoundData)
    {
        RoundResultData newData = new RoundResultData(aPlayerName, aRoundData);
        if (mHighScoreList.Contains(newData))
            return;

        mHighScoreList.Add(newData);

        SaveToList();
    }

    public void AddNewScore(string aPlayerName, ResultData aData)
    {
        SavingResultData newData = new SavingResultData(aPlayerName, aData);
        if (mScoreList.Contains(newData))
            return;

        mScoreList.Add(newData);

        SaveToListNew();
    }

    public List<RoundResultData> GetListSortBy(TableCategory aCategory)
    {
        List<RoundResultData> sortlist = new List<RoundResultData>();

        switch(aCategory)
        {
            case TableCategory.SCORE:
                sortlist = mHighScoreList.OrderByDescending(s => s.TotalScore).ThenBy(s => s.PlayerName).ToList();
                break;
            case TableCategory.COMBO:
                sortlist = mHighScoreList.OrderByDescending(s => s.TotalMaxCombo).ThenBy(s => s.PlayerName).ToList();
                break;
            case TableCategory.TIME:
                sortlist = mHighScoreList.OrderByDescending(s => s.TotalTime).ThenBy(s => s.PlayerName).ToList();
                break;
            case TableCategory.USED_BLOCK:
                sortlist = mHighScoreList.OrderByDescending(s => s.TotalUsedBlock).ThenBy(s => s.PlayerName).ToList();
                break;
        }

        return sortlist;
    }

    private void SaveToList()
    {
        ScoreTable newList = new ScoreTable(mHighScoreList);
        JsonHelper<ScoreTable>.SaveToJson(newList, FileIndex.HIGHSCORES);
    }

    private void SaveToListNew()
    {
        HighScoreTable newList = new HighScoreTable(mScoreList);
        JsonHelper<HighScoreTable>.SaveToJsonNew(newList, FileIndex.HIGHSCORES_NEW);
    }
}

[Serializable]
public class ScoreTable
{
    public List<RoundResultData> ScoreList = new List<RoundResultData>();

    public ScoreTable()
    { }

    public ScoreTable(List<RoundResultData> aScoreList)
    {
        ScoreList = aScoreList ?? throw new ArgumentNullException(nameof(aScoreList));
    }
}

[Serializable]
public class HighScoreTable
{
    public List<SavingResultData> ScoreList = new List<SavingResultData>();

    public HighScoreTable()
    { }

    public HighScoreTable(List<SavingResultData> aScoreList)
    {
        ScoreList = aScoreList ?? throw new ArgumentNullException(nameof(aScoreList));
    }
}


[Serializable]
public class SavingResultData
{
    public string PlayerName = "";

    public int TotalScores = 0;
    public int BestRoundScore = 0;
    public int BestBonus = 0;
    public int BestCombo = 0;

    public int LongestChains = 0;
    public int BestConsecutiveIncreaseChain = 0;
    public int WorstConsecutiveDecreaseChain = 0;

    public int CompletedTaskCount = 0;
    public int BestCompleteCount = 0;
    public int BestSingleTaskScore = 0;

    public int GainedLevel = 0;
    public int ReachedLevels = 0;
    public float HighestThreshold = 0;
    public float LowestThreshold = 0;

    public float PlayTime = 0f;

    public float AverageOdds = 0f;
    public int ScoreTimes = 0;
    public int LandedBlocks = 0;


    public SavingResultData() { }

    public SavingResultData(SavingResultData aData)
    {
        this.PlayerName = aData.PlayerName;

        this.TotalScores = aData.TotalScores;
        this.BestRoundScore = aData.BestRoundScore;
        this.BestBonus = aData.BestBonus;
        this.BestCombo = aData.BestCombo;

        this.LongestChains = aData.LongestChains;
        this.BestConsecutiveIncreaseChain = aData.BestConsecutiveIncreaseChain;
        this.WorstConsecutiveDecreaseChain = aData.WorstConsecutiveDecreaseChain;

        this.CompletedTaskCount = aData.CompletedTaskCount;
        this.BestCompleteCount = aData.BestCompleteCount;
        this.BestSingleTaskScore = aData.BestSingleTaskScore;

        this.GainedLevel = aData.GainedLevel;
        this.ReachedLevels = aData.ReachedLevels;
        this.HighestThreshold = aData.HighestThreshold;
        this.LowestThreshold = aData.LowestThreshold;

        this.AverageOdds = aData.AverageOdds;
        this.ScoreTimes = aData.ScoreTimes;
        this.LandedBlocks = aData.LandedBlocks;

        this.PlayTime = aData.PlayTime;
    }

    public SavingResultData(string aPlayerName, ResultData aData)
    {
        this.PlayerName = aPlayerName;

        this.TotalScores = aData.TotalScores;
        this.BestRoundScore = aData.BestRoundScores;
        this.BestBonus = aData.BestBonusTerm;
        this.BestCombo = aData.BestComboCount;

        this.LongestChains = aData.LongestChains;
        this.BestConsecutiveIncreaseChain = aData.BestConsecutiveIncreaseChain;
        this.WorstConsecutiveDecreaseChain = aData.WorstConsecutiveDecreaseChain;

        this.CompletedTaskCount = aData.TotalCompletedTaskCount;
        this.BestCompleteCount = aData.BestCompletedTaskCount;
        this.BestSingleTaskScore = aData.BestSingleTaskScoringCount;

        this.GainedLevel = aData.GainedLevelCount;
        this.ReachedLevels = aData.GainedLevelCount;
        this.HighestThreshold = aData.HighestThreshold;
        this.LowestThreshold = aData.LowestThreshold;

        this.AverageOdds = aData.AverageOdds;
        this.ScoreTimes = aData.ScoreTimes;
        this.LandedBlocks = aData.LandedBlocks;

        this.PlayTime = aData.PlayTime;
    }
}

[Serializable]
public class RoundResultData
{
    public string PlayerName = "";
    public int TotalLevel = 0;
    public int TotalScore = 0;
    public int TotalMaxCombo = 0;
    public int TotalTime = 0;
    public int TotalUsedBlock = 0;
    public List<bool> EnableScoringMethods = new List<bool>() { true, true, true, true };

    public RoundResultData()
    {

    }

    public RoundResultData(GameRoundData aRoundData)
    {
        this.PlayerName = aRoundData.PlayerName;
        this.TotalLevel = aRoundData.CurrentLevel;
        this.TotalScore = aRoundData.CurrentScore;
        this.TotalMaxCombo = aRoundData.MaxCombo;
        this.TotalTime = Mathf.RoundToInt(aRoundData.GameTime);
        this.TotalUsedBlock = aRoundData.LandedBlockCount;
        this.EnableScoringMethods = aRoundData.EnableScoringMethods;
    }

    public RoundResultData(string aPlayerName, GameRoundData aRoundData)
    {
        this.PlayerName = aPlayerName;
        this.TotalLevel = aRoundData.CurrentLevel;
        this.TotalScore = aRoundData.CurrentScore;
        this.TotalMaxCombo = aRoundData.MaxCombo;
        this.TotalTime = Mathf.RoundToInt(aRoundData.GameTime);
        this.TotalUsedBlock = aRoundData.LandedBlockCount;
        this.EnableScoringMethods = aRoundData.EnableScoringMethods;
    }

    public RoundResultData(string aPlayerName, int aTotalLevel, int aTotalScore, int aTotalMaxCombo, int aTotalTime, int aTotalUsedBlock, List<bool> aEnableScoringMethods)
    {
        this.PlayerName = aPlayerName;
        this.TotalLevel = aTotalLevel;
        this.TotalScore = aTotalScore;
        this.TotalMaxCombo = aTotalMaxCombo;
        this.TotalTime = aTotalTime;
        this.TotalUsedBlock = aTotalUsedBlock;
        this.EnableScoringMethods = aEnableScoringMethods;
    }
}
