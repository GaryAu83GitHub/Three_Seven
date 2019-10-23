using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Script.Tools;

public class OptionMenuPanel : MenuPanelBase
{
    private enum ButtonIndex
    {
        LIMIT_LINE_BUTTON,
        DROPPING_SPEED_BUTTON,
        ACTIVE_GUIDE_BUTTON,
        EXIT_BUTTON,
        RESET_BUTTON,
    }

    private enum SettingComponent
    {
        SET_LIMIT_LINE,
        SET_DROPPING_SPEED,
        SET_ACTIVE_GUIDE_BLOCK,
        MAX_COMPONENT
    }

    public Slider LimitLineHeightSlider;
    public Slider DroppingSpeedSlider;
    public Toggle GuideActiveToggle;
    public TextMeshProUGUI GuideActiveText;

    private int mLimitLineHeight = Constants.DEFAULT_ROOF_HEIGHT;
    private int mSpeedMultiplyer = 0;
    private bool mActiveGuide = GameSettings.Instance.ActiveGuideBlock;
    //private bool mActiveLinkRestriction = GameSettings.Instance.ActivateLinkRestriction;

    private int mPreSetLimitLineHeight = Constants.DEFAULT_ROOF_HEIGHT;
    private int mPreSetSpeedMultiplyer = 0;
    private bool mPreSetActiveGuide = true;

    private List<bool> mActiveComponents = new List<bool>() { true, false, false };
    private int mCurrentSelectedSettingComponent = 0;

    public override void Start()
    {
        mPanelIndex = MenuPanelIndex.OPTION_PANEL;

        Buttons[(int)ButtonIndex.RESET_BUTTON].onClick.AddListener(ExitButtonOnClick);
        Buttons[(int)ButtonIndex.EXIT_BUTTON].onClick.AddListener(ConfirmButtonOnClick);

        LimitLineHeightSlider.maxValue = Constants.MAX_CEILING_HIGH;
        LimitLineHeightSlider.minValue = Constants.MIN_CEILING_HIGH;
        //LimitLineHeightSlider.value = (float)Constants.DEFAULT_ROOF_HEIGHT;
        LimitLineHeightSlider.onValueChanged.AddListener(delegate { LimitLineHieghtSliderOnValueChange(); });

        float speedInterval = (Constants.MAXIMAL_DROPRATE - Constants.MINIMAL_DROPRATE) / Constants.DROPPING_VALUE;
        DroppingSpeedSlider.maxValue = (int)speedInterval;
        DroppingSpeedSlider.minValue = 0f;
        DroppingSpeedSlider.onValueChanged.AddListener(delegate { DroppingSpeedSliderOnValueChange(); });

        //GuideActiveToggle.isOn = mActiveGuide;
        GuideActiveToggle.onValueChanged.AddListener(delegate { GuideActiveToggleOnValueChanged(); });
        GuideActiveText.text = ToggleText(GuideActiveToggle);

        DefaultSetting();

        ActiveAdmitButton();

        base.Start();
    }

    public override void Enter()
    {
        base.Enter();
        SetSelectedButton(0);
        NavigateSettingComponent();
    }

    protected override void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    {
        base.NavigateMenuButtons(CommandIndex.NAVI_DOWN, CommandIndex.NAVI_UP);
        NavigateSettingComponent();

        OnSetLimitHeight();
        OnSetDroppingSpeed();
        OnSetActiveGuideBlock();
    }

    protected override void SelectButtonPressed()
    {
        switch(mCurrentSelectButtonIndex)
        {
            case (int)ButtonIndex.EXIT_BUTTON:
                ExitButtonOnClick();
                break;
            case (int)ButtonIndex.RESET_BUTTON:
                ResetButtonOnClick();
                break;
        }
    }

    private void NavigateSettingComponent()
    {
        if (mCurrentSelectedSettingComponent == mCurrentSelectButtonIndex)
            return;

        mCurrentSelectedSettingComponent = mCurrentSelectButtonIndex;
        if (mCurrentSelectedSettingComponent < (int)SettingComponent.MAX_COMPONENT)
        {
            DeactiveAllSettingComponent();
            mActiveComponents[mCurrentSelectedSettingComponent] = true;
        }
        else
            DeactiveAllSettingComponent();

        ActiveComponent();
    }

    private void OnSetLimitHeight()
    {
        if (!mActiveComponents[(int)SettingComponent.SET_LIMIT_LINE])
            return;

        if (ControlManager.Ins.MenuNavigation(CommandIndex.NAVI_LEFT))
            LimitLineHeightSlider.value--;
        if (ControlManager.Ins.MenuNavigation(CommandIndex.NAVI_RIGHT))
            LimitLineHeightSlider.value++;

        mLimitLineHeight = (int)LimitLineHeightSlider.value;
        ActiveAdmitButton();
    }

    private void OnSetDroppingSpeed()
    {
        if (!mActiveComponents[(int)SettingComponent.SET_DROPPING_SPEED])
            return;

        if (ControlManager.Ins.MenuNavigation(CommandIndex.NAVI_LEFT))
            DroppingSpeedSlider.value--;
        if (ControlManager.Ins.MenuNavigation(CommandIndex.NAVI_RIGHT))
            DroppingSpeedSlider.value++;

        mSpeedMultiplyer = (int)DroppingSpeedSlider.value;
        ActiveAdmitButton();
    }

    private void OnSetActiveGuideBlock()
    {
        if (!mActiveComponents[(int)SettingComponent.SET_ACTIVE_GUIDE_BLOCK])
            return;

        if(ControlManager.Ins.MenuSelectButtonPressed())
            GuideActiveToggle.isOn = !GuideActiveToggle.isOn;

        mActiveGuide = GuideActiveToggle.isOn;
        GuideActiveText.text = ToggleText(GuideActiveToggle);
        ActiveAdmitButton();
    }

    private void ActiveComponent()
    {
        LimitLineHeightSlider.interactable = mActiveComponents[(int)SettingComponent.SET_LIMIT_LINE];
        DroppingSpeedSlider.interactable = mActiveComponents[(int)SettingComponent.SET_DROPPING_SPEED];
        GuideActiveToggle.interactable = mActiveComponents[(int)SettingComponent.SET_ACTIVE_GUIDE_BLOCK];
    }

    private void DeactiveAllSettingComponent()
    {
        for (int i = 0; i < mActiveComponents.Count; i++)
            mActiveComponents[i] = false;
    }

    private void ExitButtonOnClick()
    {
        if(SettingsValueHasChanged())
        {
            GameSettings.Instance.SetLimitLineLevel(mLimitLineHeight);
            GameSettings.Instance.SetStartDropSpeed(mSpeedMultiplyer);
            GameSettings.Instance.SetActiveteGuideBlock(mActiveGuide);

            mPreSetLimitLineHeight = mLimitLineHeight;
            mPreSetSpeedMultiplyer = mSpeedMultiplyer;
            mPreSetActiveGuide = mActiveGuide;
        }
        MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
        //MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
        //gameObject.SetActive(false);
    }

    private void ResetButtonOnClick()
    {
        DefaultSetting();
        mCurrentSelectButtonIndex = (int)ButtonIndex.EXIT_BUTTON;
        SwithCurrentSelectButton();
        ActiveComponent();
    }

    // this will be replace
    private void ConfirmButtonOnClick()
    {
        GameSettings.Instance.SetLimitLineLevel(mLimitLineHeight);
        GameSettings.Instance.SetStartDropSpeed(mSpeedMultiplyer);
        GameSettings.Instance.SetActiveteGuideBlock(mActiveGuide);

        mPreSetLimitLineHeight = mLimitLineHeight;
        mPreSetSpeedMultiplyer = mSpeedMultiplyer;
        mPreSetActiveGuide = mActiveGuide;

        //gameObject.SetActive(false);
        //MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
    }

    private void LimitLineHieghtSliderOnValueChange()
    {
        mLimitLineHeight = (int)LimitLineHeightSlider.value;
        ActiveAdmitButton();
    }

    private void DroppingSpeedSliderOnValueChange()
    {
        mSpeedMultiplyer = (int)DroppingSpeedSlider.value;
        ActiveAdmitButton();
    }

    private void GuideActiveToggleOnValueChanged()
    {
        mActiveGuide = GuideActiveToggle.isOn;
        GuideActiveText.text = ToggleText(GuideActiveToggle);
        ActiveAdmitButton();
    }

    private string ToggleText(Toggle aToggle)
    {
        return (aToggle.isOn ? "ON" : "OFF");
    }

    private void ActiveAdmitButton()
    {
        bool activateAdmittButton = SettingsValueHasChanged();

        if (!activateAdmittButton)
            mButtonCount = Buttons.Count - 1;
        else
            mButtonCount = Buttons.Count;

        Buttons[(int)ButtonIndex.RESET_BUTTON].gameObject.SetActive(activateAdmittButton);
        ChangeSelectedButtonSprite((int)ButtonIndex.RESET_BUTTON, ButtonSpriteIndex.DEFAULT);
    }

    private void DefaultSetting()
    {
        mLimitLineHeight = mPreSetLimitLineHeight;
        mSpeedMultiplyer = mPreSetSpeedMultiplyer;
        mActiveGuide = mPreSetActiveGuide;

        LimitLineHeightSlider.value = (float)mLimitLineHeight;
        DroppingSpeedSlider.value = (float)mSpeedMultiplyer;
        GuideActiveToggle.isOn = mActiveGuide;
        GuideActiveText.text = ToggleText(GuideActiveToggle);
    }

    private bool SettingsValueHasChanged()
    {
        return (mLimitLineHeight != mPreSetLimitLineHeight || mSpeedMultiplyer != mPreSetSpeedMultiplyer || mActiveGuide != mPreSetActiveGuide);
    }
}
