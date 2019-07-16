using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Script.Tools;

public class OptionMenu : MonoBehaviour
{
    public Slider LimitLineHeightSlider;
    public Slider DroppingSpeedSlider;
    public Toggle GuideActiveToggle;
    public TextMeshProUGUI GuideActiveText;

    public Button ExitButton;
    public Button AdmitButton;

    private int mLimitLineHeight = Constants.DEFAULT_ROOF_HEIGHT;
    private int mSpeedMultiplyer = 0;
    private bool mActiveGuide = true;

    private int mPreSetLimitLineHeight = Constants.DEFAULT_ROOF_HEIGHT;
    private int mPreSetSpeedMultiplyer = 0;
    private bool mPreSetActiveGuide = true;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu.openOptionPanel += DefaultSetting;

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
        GuideActiveText.text = ToggleText();

        ExitButton.onClick.AddListener(ExitButtonOnClick);
        AdmitButton.onClick.AddListener(AdmitButtonOnClick);

        AdmitButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MainMenu.openOptionPanel -= DefaultSetting;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void LimitLineHieghtSliderOnValueChange()
    {
        mLimitLineHeight = (int)LimitLineHeightSlider.value;
        ActiveAdmitButton();
    }

    private void DroppingSpeedSliderOnValueChange()
    {
        //GameRoundManager.Instance.Data.DroppingSpeedMultiplyValue = (int)DroppingSpeedSlider.value;
        mSpeedMultiplyer = (int)DroppingSpeedSlider.value;
        ActiveAdmitButton();
    }

    private void GuideActiveToggleOnValueChanged()
    {
        mActiveGuide = GuideActiveToggle.isOn;
        GuideActiveText.text = ToggleText();
        ActiveAdmitButton();
    }

    private void ExitButtonOnClick()
    {
        gameObject.SetActive(false);
    }

    private void AdmitButtonOnClick()
    {
        GameSettings.Instance.SetLimitLineLevel(mLimitLineHeight);
        GameSettings.Instance.SetStartDropSpeed(mSpeedMultiplyer);
        GameSettings.Instance.ActiveteGuideBlock(mActiveGuide);

        mPreSetLimitLineHeight = mLimitLineHeight;
        mPreSetSpeedMultiplyer = mSpeedMultiplyer;
        mPreSetActiveGuide = mActiveGuide;

        gameObject.SetActive(false);
    }

    private string ToggleText()
    {
        return (GuideActiveToggle.isOn ? "ON" : "OFF");
    }

    private void ActiveAdmitButton()
    {
        bool activateAdmittButton = false;
        if (mLimitLineHeight != mPreSetLimitLineHeight || mSpeedMultiplyer != mPreSetSpeedMultiplyer || mActiveGuide != mPreSetActiveGuide)
            activateAdmittButton = true;

        AdmitButton.gameObject.SetActive(activateAdmittButton);
    }
}
