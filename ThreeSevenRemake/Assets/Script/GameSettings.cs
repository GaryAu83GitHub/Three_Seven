using System.Collections.Generic;
using UnityEngine;

public enum LinkIndexes
{
    LINK_2_DIGIT,
    LINK_3_DIGIT,
    LINK_4_DIGIT,
    LINK_5_DIGIT,
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
    
    private string mPlayerName = "";
    public string PlayerName { get { return mPlayerName; } }

    private Difficulties mDifficulty = Difficulties.EASY;
    public Difficulties Difficulty { get { return mDifficulty; } set { mDifficulty = value; } }

    public List<bool> EnableScoringMethods { get { return mEnableScoringMethods; } }
    private List<bool> mEnableScoringMethods = new List<bool>() { true, true, false, false };

    private int mStartLevel = 0;
    public int StartLevel { get { return mStartLevel; } }

    private int mLimitRow = Constants.DEFAULT_ROOF_HEIGHT;
    public int LimitHigh { get { return mLimitRow; } }

    private int mDropSpeedMultiply = 0;
    public int StartSpeedMultiply { get { return mDropSpeedMultiply; } }
    
    private int mInitialValue = 0;
    public int InitialValue { get { return mInitialValue; } }

    private bool mDisplayingLongScoring = true;
    public bool ActiveLongScoringDisplay { get { return mDisplayingLongScoring; } }

    public bool ActivateLinkRestriction { get { return mActivateLinkRestriction; } }
    private bool mActivateLinkRestriction = true;

    public bool ActiveGuideBlock { get { return mActiveGuideBlock; } }
    private bool mActiveGuideBlock = true;


    private const int mMaxLimitRow = 18;
    private const int mMinLimitRow = 9;
    private const int mAdditionBaseValue = 18;

    public GameSettings()
    {
        //for(LinkIndexes i = 0; i < (LinkIndexes.MAX); i++)
        //{
        //    mEnableScoringMethods.Add(true);
        //}
    }

    public void NewGameSettings(GameplaySettingData aData)
    {
        mDifficulty = aData.SelectDifficulty;
        mEnableScoringMethods = aData.SelectEnableDigits;
        mStartLevel = aData.SelectStartLevel;
        mLimitRow = Mathf.Clamp(aData.SelectLimitLineHeight, Constants.MIN_CEILING_HIGH, Constants.MAX_CEILING_HIGH);
        mActiveGuideBlock = aData.SelectActiveGuide;
    }
    
    public void SetStartLevel(int aStartLevel)
    {
        mStartLevel = aStartLevel;
    }

    public void SetPlayerName(string aName)
    {
        mPlayerName = aName;
    }

    public void SetScoringCubesCount(LinkIndexes anIndex, bool isActive)
    {
        mEnableScoringMethods[(int)anIndex] = isActive;
    }

    public bool IsScoringMethodActiveTo(LinkIndexes anIndex)
    {
        return mEnableScoringMethods[(int)anIndex];
    }

    public void SwapScoringCubeCountOn(LinkIndexes anIndex)
    {
        bool isThereAnotherOptionEnable = false;

        for(LinkIndexes i = LinkIndexes.LINK_2_DIGIT; i < LinkIndexes.MAX; i++)
        {
            if (i == anIndex)
                continue;
            if (isThereAnotherOptionEnable == false && mEnableScoringMethods[(int)i] == true)
                isThereAnotherOptionEnable = mEnableScoringMethods[(int)i];

        }
        if(isThereAnotherOptionEnable)
            mEnableScoringMethods[(int)anIndex] = !mEnableScoringMethods[(int)anIndex];
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
        mLimitRow = Mathf.Clamp(aLimitLineRow, Constants.MIN_CEILING_HIGH, Constants.MAX_CEILING_HIGH);
    }
    
    public void SetInitialValue(int anInitialValue)
    {
        mInitialValue = anInitialValue;
    }

    public bool ToggleScoringDisplayMethod()
    {
        return mDisplayingLongScoring = !mDisplayingLongScoring;
    }

    public void SetActiveteGuideBlock(bool aChangeValue)
    {
        mActiveGuideBlock = aChangeValue;
    }

    public void SetActiveteLinkRestriction(bool aChangeValue)
    {
        mActivateLinkRestriction = aChangeValue;
    }

    public bool GetGuideBlockVisible(bool aSetOn)
    {
        if (!mActiveGuideBlock)
            return false;

        return aSetOn;
    }

    public void Reset()
    {
        mEnableScoringMethods = new List<bool>() { true, false, false, false };
    }
}
