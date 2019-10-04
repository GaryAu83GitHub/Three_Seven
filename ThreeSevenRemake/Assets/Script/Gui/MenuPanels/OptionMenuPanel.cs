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
        CONFIRM_BUTTON,
        EXIT_BUTTON
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

    public override void Start()
    {
        base.Start();

        Buttons[(int)ButtonIndex.CONFIRM_BUTTON].onClick.AddListener(ExitButtonOnClick);
        Buttons[(int)ButtonIndex.EXIT_BUTTON].onClick.AddListener(ConfirmButtonOnClick);

        LimitLineHeightSlider.maxValue = Constants.MAX_CEILING_HIGH;
        LimitLineHeightSlider.minValue = Constants.MIN_CEILING_HIGH;
        LimitLineHeightSlider.value = (float)Constants.DEFAULT_ROOF_HEIGHT;
        LimitLineHeightSlider.onValueChanged.AddListener(delegate { LimitLineHieghtSliderOnValueChange(); });

        float speedInterval = (Constants.MAXIMAL_DROPRATE - Constants.MINIMAL_DROPRATE) / Constants.DROPPING_VALUE;
        DroppingSpeedSlider.maxValue = (int)speedInterval;
        DroppingSpeedSlider.minValue = 0f;
        DroppingSpeedSlider.onValueChanged.AddListener(delegate { DroppingSpeedSliderOnValueChange(); });

        GuideActiveToggle.isOn = mActiveGuide;
        GuideActiveToggle.onValueChanged.AddListener(delegate { GuideActiveToggleOnValueChanged(); });
        GuideActiveText.text = ToggleText(GuideActiveToggle);
    }

    protected override void Input()
    {
        base.Input();
    }

    private void ExitButtonOnClick()
    {
        MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
        //gameObject.SetActive(false);
    }

    private void ConfirmButtonOnClick()
    {
        GameSettings.Instance.SetLimitLineLevel(mLimitLineHeight);
        GameSettings.Instance.SetStartDropSpeed(mSpeedMultiplyer);
        GameSettings.Instance.SetActiveteGuideBlock(mActiveGuide);

        mPreSetLimitLineHeight = mLimitLineHeight;
        mPreSetSpeedMultiplyer = mSpeedMultiplyer;
        mPreSetActiveGuide = mActiveGuide;

        //gameObject.SetActive(false);
        MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
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
        bool activateAdmittButton = false;
        if (mLimitLineHeight != mPreSetLimitLineHeight || mSpeedMultiplyer != mPreSetSpeedMultiplyer || mActiveGuide != mPreSetActiveGuide)
            activateAdmittButton = true;

        Buttons[(int)ButtonIndex.CONFIRM_BUTTON].gameObject.SetActive(activateAdmittButton);
    }

    private void DefaultSetting()
    {
        mLimitLineHeight = mPreSetLimitLineHeight;
        mSpeedMultiplyer = mPreSetSpeedMultiplyer;
        mActiveGuide = mPreSetActiveGuide;

        LimitLineHeightSlider.value = (float)mLimitLineHeight;
        DroppingSpeedSlider.value = (float)mSpeedMultiplyer;
        GuideActiveToggle.isOn = mActiveGuide;
    }
}
