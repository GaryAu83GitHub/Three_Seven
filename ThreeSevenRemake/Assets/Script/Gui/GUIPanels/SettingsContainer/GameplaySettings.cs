using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplaySettings : SettingsContainerBase
{
    private enum SettingIndex
    {
        DIFFICULTY,
        LIMIT_LINE,
        DROPPING_SPEED,
        ACTIVE_GUIDE,
        CONFIRM_BUTTON,
    }

    public delegate void OnReturnToSettingButtonContainer();
    public static OnReturnToSettingButtonContainer returnToSettingButtonContainer;
    
    private GameplaySettingData mOriginalSettings = new GameplaySettingData();
    private GameplaySettingData mNewSettings = new GameplaySettingData();

    private bool SettingHasBeenChanged { get { return mOriginalSettings.Equals(mNewSettings); } }
 
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SettingSlots[(int)SettingIndex.CONFIRM_BUTTON].gameObject.SetActive(!SettingHasBeenChanged);
        CheckNumberOfActiveSlots();

        SettingSlotBase.gameplaySettingHaveChange += ChangeGameplaySetting;

        SetGameplayConfirmSlot.applyButtonPressed += ApplySettings;
        SetGameplayConfirmSlot.resetButtonPressed += ResetSettings;
    }

    private void OnDestroy()
    {
        SettingSlotBase.gameplaySettingHaveChange -= ChangeGameplaySetting;

        SetGameplayConfirmSlot.applyButtonPressed -= ApplySettings;
        SetGameplayConfirmSlot.resetButtonPressed -= ResetSettings;
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
            mCurrentSelectingSlotIndex = mCurrentDisplaySlotsCount;
        else
            mCurrentSelectingSlotIndex += anIncreame;

        SwitchSelectingSlot(mCurrentSelectingSlotIndex);
        //mCurrentSelectedSlot.Exit();
        //mCurrentSelectedSlot = SettingSlots[mCurrentSelectingSlotIndex];
        //mCurrentSelectedSlot.Enter();
        return;
    }

    private void ChangeGameplaySetting(ref GameplaySettingData data)
    {
        mNewSettings = new GameplaySettingData(data);
        ConfirmButtonsDisplay();
    }

    private void ApplySettings()
    {
        mOriginalSettings = new GameplaySettingData(mNewSettings);
        mCurrentSelectingSlotIndex = 0;
        SwitchSelectingSlot(mCurrentSelectingSlotIndex);
        ConfirmButtonsDisplay();
        GameSettings.Instance.NewGameSettings(mOriginalSettings);
    }

    private void ResetSettings()
    {
        mNewSettings = new GameplaySettingData(mOriginalSettings);
        SetSlotsValue(mNewSettings);
        mCurrentSelectingSlotIndex = 0;
        SwitchSelectingSlot(mCurrentSelectingSlotIndex);
        ConfirmButtonsDisplay();
        //mCurrentSelectedSlot = SettingSlots[mCurrentSelectingSlotIndex];
        //mCurrentSelectedSlot.ActivatingSlot(true);
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

public class GameplaySettingData
{
    public Difficulties SelectDifficulty { get { return mDifficulty; } set { mDifficulty = value; } }
    private Difficulties mDifficulty = Difficulties.EASY;

    public List<bool> SelectEnableDigits { get { return mEnableDigits; } set { mEnableDigits = value; } }
    private List<bool> mEnableDigits = new List<bool>() { true, true, false, false };

    public int SelectLimitLineHeight { get { return mLimitLineHeight; } set { mLimitLineHeight = value; } }
    private int mLimitLineHeight = 0;

    public int SelectStartLevel { get { return mStartLevel; } set { mStartLevel = value; } }
    private int mStartLevel = 0;

    public bool SelectActiveGuide { get { return mActiveGuide; } set { mActiveGuide = value; } }
    private bool mActiveGuide = true;

    public GameplaySettingData()
    {
        mDifficulty = GameSettings.Instance.Difficulty; //GameRoundManager.Instance.Data.SelectedDifficulty;
        mEnableDigits = GameSettings.Instance.EnableScoringMethods;
        mLimitLineHeight = GameSettings.Instance.LimitHigh;//GameRoundManager.Instance.Data.RoofHeightValue;
        mStartLevel = GameSettings.Instance.StartLevel;//GameRoundManager.Instance.Data.CurrentLevel;
        mActiveGuide = GameSettings.Instance.ActiveGuideBlock;//GameRoundManager.Instance.Data.GuideblockActive;
    }

    public GameplaySettingData(GameplaySettingData data)
    {
        mDifficulty = data.SelectDifficulty;
        mEnableDigits = data.SelectEnableDigits;
        mLimitLineHeight = data.SelectLimitLineHeight;
        mStartLevel = data.SelectStartLevel;
        mActiveGuide = data.SelectActiveGuide;
    }

    public bool Equals(GameplaySettingData obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            GameplaySettingData p = (GameplaySettingData)obj;
            if (p.SelectDifficulty != mDifficulty)
                return false;
            if (!p.SelectEnableDigits.SequenceEqual(mEnableDigits))
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
}  
