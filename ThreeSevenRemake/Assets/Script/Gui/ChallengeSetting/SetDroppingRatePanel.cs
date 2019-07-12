using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDroppingRatePanel : SettingPanelBase
{
    public Slider DroppingSpeedValueSlider;
    public TextMeshProUGUI CurrentValueText;
    public TextMeshProUGUI MaxSliderValueText;
    public TextMeshProUGUI MinSliderValueText;

    private int mMultiplyValue = 0;
    private float mCurrentDroppingSpeed = 0f;
    private int mMaxMultiplyValue = 1;
    private int mMinMultiplyValue = 0;
    
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

        DroppingSpeedValueSlider.onValueChanged.AddListener(delegate { MultiplyValueChange(); });

        mMaxMultiplyValue = (int)((Constants.MAXIMAL_DROPRATE - Constants.MINIMAL_DROPRATE) / Constants.DROPPING_VALUE);

        MinSliderValueText.text = mMinMultiplyValue.ToString();
        MaxSliderValueText.text = mMaxMultiplyValue.ToString();
        
        DroppingSpeedValueSlider.maxValue = mMaxMultiplyValue;
        DroppingSpeedValueSlider.minValue = mMinMultiplyValue;

        
    }

    public override void InitBaseValue()
    {
        base.InitBaseValue();

        DroppingSpeedCalculator();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.FINISH_SETTING);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_ROOF_HEIGH);
    }

    public void MultiplyValueChange()
    {
        mMultiplyValue = (int)DroppingSpeedValueSlider.value;
        CurrentValueText.text = mMultiplyValue.ToString();

        DescriptionText.text = "The dropping speed now is " + DroppingSpeedCalculator().ToString();

        GameRoundManager.Instance.Data.DroppingSpeedMultiplyValue = mMultiplyValue;
    }

    private float DroppingSpeedCalculator()
    {
        float degreesingValue = (float)mMultiplyValue * Constants.DROPPING_VALUE;

        mCurrentDroppingSpeed = Constants.MAXIMAL_DROPRATE - degreesingValue;
        return mCurrentDroppingSpeed;
    }
}
