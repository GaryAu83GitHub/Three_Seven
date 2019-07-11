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

        mCurrentRoofHeight = MAX_CEILING_HIGH - GameSettings.Instance.LimitHigh;
        CeilingHieghtValueSlider.maxValue = MAX_CEILING_HIGH - MIN_CEILING_HIGH;
        CeilingHieghtValueSlider.minValue = 0;
        CeilingHieghtValueSlider.value = mCurrentRoofHeight;
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        //displaySettingPanel?.Invoke(Setting_Issue.SET_HIGH_LIMIT + 1);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Issue.SET_HIGH_LIMIT - 1);
    }

    public void CeilingHieghtValueChange()
    {
        mCurrentRoofHeight = MAX_CEILING_HIGH - (int)CeilingHieghtValueSlider.value;
        CurrentValueText.text = mCurrentRoofHeight.ToString();

        DescriptionText.text = "The ceiling is at the row " + mCurrentRoofHeight.ToString();
    }
}
