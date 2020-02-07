using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum TableCategory
{
    SCORE,
    DIGIT,
    CHAIN,
    TASK,
    LEVEL,
    TIME,
    ODDS,
    MAX_COUNT,
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

    /// <summary>
    /// new score list
    /// </summary>
    private List<SavingResultData> mScoreList = new List<SavingResultData>();
    public List<SavingResultData> ScoreList { get { return mScoreList; } }

    public List<int> EnableDigitsList { get { return mEnableDigitsList; } }
    private List<int> mEnableDigitsList = new List<int>();

    private List<SavingResultData> mMainActiveList = new List<SavingResultData>();
    private List<SavingResultData> mSurviveList = new List<SavingResultData>();
    private List<SavingResultData> mClassicList = new List<SavingResultData>();

    public HighScoreManager()
    {
        
    }

    public void AddNewScore(string aPlayerName, ResultData aData)
    {
        SavingResultData newData = new SavingResultData(aPlayerName, aData);
        if (mScoreList.Contains(newData))
            return;

        mScoreList.Clear();

        FileIndex listFileIndex = FileIndex.CLASSIC_LIST;
        switch(GameSettings.Instance.GameMode)
        {
            case GameMode.SURVIVAL:
                listFileIndex = FileIndex.SURVIVE_LIST;
                mScoreList = new List<SavingResultData>(mSurviveList);
                break;
            default:
                listFileIndex = FileIndex.CLASSIC_LIST;
                mScoreList = new List<SavingResultData>(mClassicList);
                break;
        }

        mScoreList.Add(newData);
        SaveToListOf(listFileIndex);
    }

    public void SetActiveList(GameMode aMode = GameMode.CLASSIC)
    {
        FileIndex file = FileIndex.CLASSIC_LIST;
        switch(aMode)
        {
            case GameMode.SURVIVAL:
                file = FileIndex.SURVIVE_LIST;
                break;
            case GameMode.TIME_ATTACK:
                break;
            default:
                file = FileIndex.CLASSIC_LIST;
                break;
        }
        LoadListFrom(file);
    }

    public List<SavingResultData> GetListOfEnableDigit(int aEnableDigitBinary)
    {
        mScoreList.Clear();

        if (aEnableDigitBinary > 0)
        {
            for (int i = 0; i < mMainActiveList.Count; i++)
            {
                if (mMainActiveList[i].EnableDigitInterger == aEnableDigitBinary)
                    mScoreList.Add(mMainActiveList[i]);
            }
        }
        else
            mScoreList = new List<SavingResultData>(mMainActiveList);

        return GetListSortBy(TableCategory.SCORE);
    }
    
    public List<SavingResultData> GetListSortBy(TableCategory aCategory, bool sortByDescending = true)
    {
        if (!sortByDescending)
            return SortListAscending(aCategory);

        return SortListDescending(aCategory);
    }

    public void FillAllList()
    {
        mClassicList.Clear();
        mSurviveList.Clear();
        HighScoreTable classicTable = (JsonHelper<HighScoreTable>.LoadFromJson(FileIndex.CLASSIC_LIST) ?? new HighScoreTable());
        foreach (SavingResultData data in classicTable.ScoreList)
            mClassicList.Add(data);

        HighScoreTable surviveTable = (JsonHelper<HighScoreTable>.LoadFromJson(FileIndex.SURVIVE_LIST) ?? new HighScoreTable());
        foreach (SavingResultData data in surviveTable.ScoreList)
            mSurviveList.Add(data);
        return;
    }

    private List<SavingResultData> SortListDescending(TableCategory aCategory)
    {
        List<SavingResultData> sortList = new List<SavingResultData>();

        switch (aCategory)
        {
            case TableCategory.DIGIT:
                sortList = mScoreList.OrderBy(s => s.EnableDigitInterger)
                    .ThenByDescending(s => s.TotalScores).ToList();
                break;
            case TableCategory.CHAIN:
                sortList = mScoreList.OrderByDescending(s => s.LongestChains)
                    .ThenByDescending(s => s.BestConsecutiveIncreaseChain)
                    .ThenBy(s => s.WorstConsecutiveDecreaseChain).ToList();
                break;
            case TableCategory.TASK:
                sortList = mScoreList.OrderByDescending(s => s.CompletedTaskCount)
                    .ThenByDescending(s => s.BestCompleteTaskCount)
                    .ThenByDescending(s => s.BestSingleTaskScore).ToList();
                break;
            case TableCategory.LEVEL:
                sortList = mScoreList.OrderByDescending(s => s.GainedLevel)
                    .ThenByDescending(s => s.ReachedLevels)
                    .ThenByDescending(s => s.HighestLevelUpgradeThreshold).ToList();
                break;
            case TableCategory.TIME:
                sortList = mScoreList.OrderByDescending(s => s.PlayTime).ToList();
                break;
            case TableCategory.ODDS:
                sortList = mScoreList.OrderByDescending(s => s.AverageOdds)
                    .ThenByDescending(s => s.ScoreTimes)
                    .ThenByDescending(s => s.LandedBlocks).ToList();
                break;
            default:
                sortList = mScoreList.OrderByDescending(s => s.TotalScores)
                    .ThenByDescending(s => s.BestRoundScore)
                    .ThenByDescending(s => s.BestCombo)
                    .ThenByDescending(s => s.BestBonus).ToList();
                break;
        }

        return sortList;
    }

    private List<SavingResultData> SortListAscending(TableCategory aCategory)
    {
        List<SavingResultData> sortList = new List<SavingResultData>();

        switch (aCategory)
        {
            case TableCategory.DIGIT:
                sortList = mScoreList.OrderBy(s => s.EnableDigitInterger)
                    .ThenByDescending(s => s.TotalScores).ToList();
                break;
            case TableCategory.CHAIN:
                sortList = mScoreList.OrderBy(s => s.LongestChains)
                    .ThenBy(s => s.BestConsecutiveIncreaseChain)
                    .ThenByDescending(s => s.WorstConsecutiveDecreaseChain).ToList();
                break;
            case TableCategory.TASK:
                sortList = mScoreList.OrderBy(s => s.CompletedTaskCount)
                    .ThenBy(s => s.BestCompleteTaskCount)
                    .ThenBy(s => s.BestSingleTaskScore).ToList();
                break;
            case TableCategory.LEVEL:
                sortList = mScoreList.OrderBy(s => s.GainedLevel)
                    .ThenBy(s => s.ReachedLevels)
                    .ThenBy(s => s.HighestLevelUpgradeThreshold).ToList();
                break;
            case TableCategory.TIME:
                sortList = mScoreList.OrderBy(s => s.PlayTime).ToList();
                break;
            case TableCategory.ODDS:
                sortList = mScoreList.OrderBy(s => s.AverageOdds)
                    .ThenBy(s => s.ScoreTimes)
                    .ThenBy(s => s.LandedBlocks).ToList();
                break;
            default:
                sortList = mScoreList.OrderBy(s => s.TotalScores)
                    .ThenBy(s => s.BestRoundScore)
                    .ThenBy(s => s.BestCombo)
                    .ThenBy(s => s.BestBonus).ToList();
                break;
        }

        return sortList;
    }

    private void SaveToListOf(FileIndex aFileIndex)
    {
        HighScoreTable newList = new HighScoreTable(mScoreList);
        JsonHelper<HighScoreTable>.SaveToJson(newList, aFileIndex);
    }

    private void LoadListFrom(FileIndex aFileIndex)
    {
        mMainActiveList.Clear();
        mScoreList.Clear();
        mEnableDigitsList.Clear();

        mEnableDigitsList.Add(0);

        //HighScoreTable loadTable = (JsonHelper<HighScoreTable>.LoadFromJson(aFileIndex) ?? new HighScoreTable());
        //foreach (SavingResultData data in loadTable.ScoreList)
        //{
        //    mMainActiveList.Add(data);
        //    if (!mEnableDigitsList.Contains(data.EnableDigitInterger))
        //        mEnableDigitsList.Add(data.EnableDigitInterger);
        //}
        if (aFileIndex == FileIndex.CLASSIC_LIST)
            mMainActiveList = new List<SavingResultData>(mClassicList);
        else if (aFileIndex == FileIndex.SURVIVE_LIST)
            mMainActiveList = new List<SavingResultData>(mSurviveList);

        foreach(SavingResultData data in mMainActiveList)
        {
            if (!EnableDigitsList.Contains(data.EnableDigitInterger))
                mEnableDigitsList.Add(data.EnableDigitInterger);
        }

        mScoreList = new List<SavingResultData>(mMainActiveList);
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

    public bool EnableDigit2 = false;
    public bool EnableDigit3 = false;
    public bool EnableDigit4 = false;
    public bool EnableDigit5 = false;
    public int EnableDigitInterger = 0;

    public int TotalScores = 0;
    public int BestRoundScore = 0;
    public int BestCombo = 0;
    public int BestBonus = 0;

    public int LongestChains = 0;
    public int BestConsecutiveIncreaseChain = 0;
    public int WorstConsecutiveDecreaseChain = 0;

    public int CompletedTaskCount = 0;
    public int BestCompleteTaskCount = 0;
    public int BestSingleTaskScore = 0;

    public int GainedLevel = 0;
    public int ReachedLevels = 0;
    public float HighestLevelUpgradeThreshold = 0;
    //public float LowestLevelDowngradeThreshold = 0;

    public float PlayTime = 0f;

    public float AverageOdds = 0f;
    public int ScoreTimes = 0;
    public int LandedBlocks = 0;


    public SavingResultData() { }

    public SavingResultData(SavingResultData aData)
    {
        this.PlayerName = aData.PlayerName;

        this.EnableDigit2 = aData.EnableDigit2;
        this.EnableDigit3 = aData.EnableDigit3;
        this.EnableDigit4 = aData.EnableDigit4;
        this.EnableDigit5 = aData.EnableDigit5;
        this.EnableDigitInterger = aData.EnableDigitInterger;
        if(this.EnableDigitInterger == 0)
        {
            if (aData.EnableDigit2)
                this.EnableDigitInterger += 1;
            else if (aData.EnableDigit3)
                this.EnableDigitInterger += 2;
            else if (aData.EnableDigit4)
                this.EnableDigitInterger += 4;
            else if (aData.EnableDigit5)
                this.EnableDigitInterger += 8;
        }

        this.TotalScores = aData.TotalScores;
        this.BestRoundScore = aData.BestRoundScore;
        this.BestBonus = aData.BestBonus;
        this.BestCombo = aData.BestCombo;

        this.LongestChains = aData.LongestChains;
        this.BestConsecutiveIncreaseChain = aData.BestConsecutiveIncreaseChain;
        this.WorstConsecutiveDecreaseChain = aData.WorstConsecutiveDecreaseChain;

        this.CompletedTaskCount = aData.CompletedTaskCount;
        this.BestCompleteTaskCount = aData.BestCompleteTaskCount;
        this.BestSingleTaskScore = aData.BestSingleTaskScore;

        this.GainedLevel = aData.GainedLevel;
        this.ReachedLevels = aData.ReachedLevels;
        this.HighestLevelUpgradeThreshold = aData.HighestLevelUpgradeThreshold;
        //this.LowestLevelDowngradeThreshold = aData.LowestLevelDowngradeThreshold;

        this.AverageOdds = aData.AverageOdds;
        this.ScoreTimes = aData.ScoreTimes;
        this.LandedBlocks = aData.LandedBlocks;

        this.PlayTime = aData.PlayTime;
    }

    public SavingResultData(string aPlayerName, ResultData aData)
    {
        this.PlayerName = aPlayerName;

        this.EnableDigit2 = aData.EnableDigits[0];
        this.EnableDigit3 = aData.EnableDigits[1];
        this.EnableDigit4 = aData.EnableDigits[2];
        this.EnableDigit5 = aData.EnableDigits[3];
        this.EnableDigitInterger = aData.EnambleDigitInterger;

        this.TotalScores = aData.TotalScores;
        this.BestRoundScore = aData.BestRoundScores;
        this.BestBonus = aData.BestBonusTerm;
        this.BestCombo = aData.BestComboCount;

        this.LongestChains = aData.LongestChains;
        this.BestConsecutiveIncreaseChain = aData.BestConsecutiveIncreaseChain;
        this.WorstConsecutiveDecreaseChain = aData.WorstConsecutiveDecreaseChain;

        this.CompletedTaskCount = aData.TotalCompletedTaskCount;
        this.BestCompleteTaskCount = aData.BestCompletedTaskCount;
        this.BestSingleTaskScore = aData.BestSingleTaskScoringCount;

        this.GainedLevel = aData.GainedLevelCount;
        this.ReachedLevels = aData.GainedLevelCount;
        this.HighestLevelUpgradeThreshold = aData.HighestThreshold;
        //this.LowestLevelDowngradeThreshold = aData.LowestThreshold;

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
