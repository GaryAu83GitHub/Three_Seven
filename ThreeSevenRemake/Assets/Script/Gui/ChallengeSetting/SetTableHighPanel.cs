using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetTableHighPanel : SettingPanelBase
{
    public Slider CeilingHieghtValueSlider;
    public TextMeshProUGUI CurrentValueText;
    public TextMeshProUGUI MaxSliderValueText;
    public TextMeshProUGUI MinSliderValueText;

    private int mCurrentRoofHeight = 0;
    private bool mRoofValueHasChanged = false;

    private const int MAX_CEILING_HIGH = 18;
    private const int MIN_CEILING_HIGH = 9;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();

        CeilingHieghtValueSlider.onValueChanged.AddListener(delegate { CeilingHieghtValueChange(); });

        MinSliderValueText.text = MIN_CEILING_HIGH.ToString();
        MaxSliderValueText.text = MAX_CEILING_HIGH.ToString();

        CeilingHieghtValueSlider.maxValue = MAX_CEILING_HIGH - MIN_CEILING_HIGH;
        CeilingHieghtValueSlider.minValue = 0;
        CeilingHieghtValueSlider.value = mCurrentRoofHeight;
    }

    public override void InitBaseValue()
    {
        base.InitBaseValue();
        if (!mRoofValueHasChanged)
        {
            GameRoundManager.Instance.Data.RoofHeightValue = Constants.DEFAULT_ROOF_HEIGHT;
            CeilingHieghtValueSlider.value = MAX_CEILING_HIGH - Constants.DEFAULT_ROOF_HEIGHT;
        }
        CurrentValueText.text = GameRoundManager.Instance.Data.RoofHeightValue.ToString();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_DROPPING_RATE);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        if(OnlyTwoDigitLinkIsEnable())
            displaySettingPanel?.Invoke(Setting_Index.SET_LINK);
        else
            displaySettingPanel?.Invoke(Setting_Index.SET_START_TASK_VALUE);
    }

    public void CeilingHieghtValueChange()
    {
        mCurrentRoofHeight = MAX_CEILING_HIGH - (int)CeilingHieghtValueSlider.value;
        CurrentValueText.text = mCurrentRoofHeight.ToString();

        DescriptionText.text = "The ceiling is at the row " + mCurrentRoofHeight.ToString();

        GameRoundManager.Instance.Data.RoofHeightValue = mCurrentRoofHeight;

        mRoofValueHasChanged = true;
    }
}
