using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplaySettings : SettingsContainerBase
{
    private enum SettingIndex
    {
        DIFFICULTY,
        CHALLENGE,
        START_LEVEL,
        ACTIVE_GUIDE,
        CONFIRM_BUTTON,
    }

    public delegate void OnReturnToSettingButtonContainer();
    public static OnReturnToSettingButtonContainer returnToSettingButtonContainer;
    
    private GameplaySettingData mOriginalSettings = new GameplaySettingData();
    private GameplaySettingData mNewSettings = new GameplaySettingData();

    private bool SettingHasBeenChanged { get { return mOriginalSettings.Equals(mNewSettings); } }

    private void Awake()
    {
        SettingSlotBase.gameplaySettingHaveChange += ChangeGameplaySetting;

        SetSettingsConfirmSlot.applyButtonPressed += ApplySettings;
        SetSettingsConfirmSlot.resetButtonPressed += ResetSettings;
    }

    protected override void Start()
    {
        base.Start();
        SettingSlots[(int)SettingIndex.CONFIRM_BUTTON].gameObject.SetActive(!SettingHasBeenChanged);
        CheckNumberOfActiveSlots();
    }

    private void OnDestroy()
    {
        SettingSlotBase.gameplaySettingHaveChange -= ChangeGameplaySetting;

        SetSettingsConfirmSlot.applyButtonPressed -= ApplySettings;
        SetSettingsConfirmSlot.resetButtonPressed -= ResetSettings;
    }

    protected override void Input()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_DOWN))
            SelectingSlot(1);
        else if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_UP))
            SelectingSlot(-1);

        if (ControlManager.Ins.MenuBackButtonPressed() || ControlManager.Ins.MenuCancelButtonPressed())
            returnToSettingButtonContainer?.Invoke();

        base.Input();
    }

    public override void Enter()
    {
        mCurrentSelectingSlotIndex = 0;
        SetSlotsValue(mOriginalSettings);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SelectingSlot(int anIncreame)
    {
        if ((mCurrentSelectingSlotIndex + anIncreame) >= mCurrentDisplaySlotsCount)
            mCurrentSelectingSlotIndex = 0;
        else if ((mCurrentSelectingSlotIndex + anIncreame) < 0)
            mCurrentSelectingSlotIndex = mCurrentDisplaySlotsCount - 1;
        else
            mCurrentSelectingSlotIndex += anIncreame;

        SwitchSelectingSlot(mCurrentSelectingSlotIndex);
    }

    private void ChangeGameplaySetting(ref GameplaySettingData data)
    {
        mNewSettings = new GameplaySettingData(data);
        ConfirmButtonsDisplay();
    }

    protected override void ApplySettings()
    {
        //if (!mActiveContainer)
        //    return;

        //mCurrentSelectingSlotIndex = 0;
        //SwitchSelectingSlot(mCurrentSelectingSlotIndex);
        base.ApplySettings();

        mOriginalSettings = new GameplaySettingData(mNewSettings);
        ConfirmButtonsDisplay();
        GameSettings.Instance.NewGameSettings(mOriginalSettings);
    }

    protected override void ResetSettings()
    {
        //if (!mActiveContainer)
        //    return;

        //mCurrentSelectingSlotIndex = 0;
        //SwitchSelectingSlot(mCurrentSelectingSlotIndex);
        base.ResetSettings();

        mNewSettings = new GameplaySettingData(mOriginalSettings);
        SetSlotsValue(mNewSettings);
        ConfirmButtonsDisplay();
    }

    private void SetSlotsValue(GameplaySettingData valueData)
    {
        for (int i = 0; i < mCurrentDisplaySlotsCount; i++)
            SettingSlots[i].SetSlotValue(valueData);
    }

    private void ConfirmButtonsDisplay()
    {
        bool equalSettings = SettingHasBeenChanged;
        SettingSlots[(int)SettingIndex.CONFIRM_BUTTON].gameObject.SetActive(!equalSettings);
        CheckNumberOfActiveSlots();

        //Debug.Log(equalSettings);
    }

    private void SwitchSelectingSlot(int aNewSelectingSlotIndex)
    {
        mCurrentSelectedSlot.Exit();
        mCurrentSelectedSlot = SettingSlots[aNewSelectingSlotIndex];
        mCurrentSelectedSlot.Enter();
    }
}

/// <summary>
/// This is a data assembling class that temporary store the gameplay data and it
/// use for compareing the gameplay setting with the previous setting and the player 
/// just made
/// </summary>
public class GameplaySettingData
{
    private Difficulties mDifficulty = Difficulties.EASY;
    public Difficulties SelectDifficulty { get { return mDifficulty; } set { mDifficulty = value; } }

    private LevelUpMode mLevelUpMode = LevelUpMode.DYNAMIC;
    public LevelUpMode LevelUpMode { get { return mLevelUpMode; } set { mLevelUpMode = value; } }

    public List<bool> SelectEnableDigits { get { return new List<bool>() { EnableDigit2, EnableDigit3, EnableDigit4, EnableDigit5 }; } }

    private bool mEnableDigit2 = true;
    public bool EnableDigit2 { get { return mEnableDigit2; } }

    private bool mEnableDigit3 = true;
    public bool EnableDigit3 { get { return mEnableDigit3; } }

    private bool mEnableDigit4 = false;
    public bool EnableDigit4 { get { return mEnableDigit4; } }

    private bool mEnableDigit5 = false;
    public bool EnableDigit5 { get { return mEnableDigit5; } }
    
    private int mLimitLineHeight = 0;
    public int SelectLimitLineHeight { get { return mLimitLineHeight; } set { mLimitLineHeight = value; } }

    private int mStartLevel = 0;
    public int SelectStartLevel { get { return mStartLevel; } set { mStartLevel = value; } }

    private int mTimeLimit = 300;
    public int TimeLimit { get { return mTimeLimit; } set { mTimeLimit = value; } }

    public bool SelectActiveGuide { get { return mActiveGuide; } set { mActiveGuide = value; } }
    private bool mActiveGuide = true;

    /// <summary>
    /// This is the default constructor that set the data with the game presetting data
    /// </summary>
    public GameplaySettingData()
    {
        mDifficulty = GameSettings.Instance.Difficulty;
        mLevelUpMode = GameSettings.Instance.LevelUpMode;
        mLimitLineHeight = GameSettings.Instance.LimitHigh;
        mStartLevel = GameSettings.Instance.StartLevel;
        mTimeLimit = GameSettings.Instance.TimeLimit;
        mActiveGuide = GameSettings.Instance.ActiveGuideBlock;

        SetChallengeDigit(GameSettings.Instance.EnableScoringMethods);
    }

    /// <summary>
    /// This is the constructor of recieving new setting data
    /// </summary>
    /// <param name="data">new recieving data</param>
    public GameplaySettingData(GameplaySettingData data)
    {
        mDifficulty = data.SelectDifficulty;
        mLevelUpMode = data.LevelUpMode;
        mLimitLineHeight = data.SelectLimitLineHeight;
        mStartLevel = data.SelectStartLevel;
        mTimeLimit = data.TimeLimit;
        mActiveGuide = data.SelectActiveGuide;

        SetChallengeDigit(data.SelectEnableDigits);
    }

    /// <summary>
    /// Check if the compareing setting data is equal to the original data
    /// </summary>
    /// <param name="obj">comparing data</param>
    /// <returns>result of the comparing data. Return false if any data is not equal</returns>
    public bool Equals(GameplaySettingData obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            GameplaySettingData p = (GameplaySettingData)obj;
            if (p.SelectDifficulty != mDifficulty)
                return false;
            //if (p.LevelUpMode != mLevelUpMode)
            //    return false;
            if (p.EnableDigit2 != mEnableDigit2)
                return false;
            if (p.EnableDigit3 != mEnableDigit3)
                return false;
            if (p.EnableDigit4 != mEnableDigit4)
                return false;
            if (p.EnableDigit5 != mEnableDigit5)
                return false;
            if (p.SelectLimitLineHeight != mLimitLineHeight)
                return false;
            if (p.SelectStartLevel != mStartLevel)
                return false;
            if (p.mActiveGuide != mActiveGuide)
                return false;
        }
        return true;
    }

    /// <summary>
    /// fill the boolian variable of challenge digit settings
    /// </summary>
    /// <param name="someEnableDigits">boolians of the new setting of the 
    /// challenge</param>
    public void SetChallengeDigit(List<bool> someEnableDigits)
    {
        mEnableDigit2 = someEnableDigits[0];
        mEnableDigit3 = someEnableDigits[1];
        mEnableDigit4 = someEnableDigits[2];
        mEnableDigit5 = someEnableDigits[3];
    }
}  
