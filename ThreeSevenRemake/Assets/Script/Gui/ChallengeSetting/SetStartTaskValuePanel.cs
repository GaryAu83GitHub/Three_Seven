using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetStartTaskValuePanel : SettingPanelBase
{
    public Slider TaskValueSlider;
    public TextMeshProUGUI CurrentValueText;
    public TextMeshProUGUI MaxSliderValueText;
    public TextMeshProUGUI MinSliderValueText;

    private int mCurrentStartValue = 0;
    private int mLastSelectedValue = -1;

    private readonly int MINIMAL_MAX_SUM = 18;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        SetLinkNumberPanel.changeTaskMaskValue -= SetMaxValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();

        SetLinkNumberPanel.changeTaskMaskValue += SetMaxValue;

        TaskValueSlider.onValueChanged.AddListener(delegate { TaskSliderValueChange(); });
        MinSliderValueText.text = MINIMAL_MAX_SUM.ToString();
    }

    public override void InitBaseValue()
    {
        base.InitBaseValue();

        if (mLastSelectedValue == -1)
            mCurrentStartValue = Constants.MINIMAL_TASK_SUM;
        else
            mCurrentStartValue = MINIMAL_MAX_SUM + mLastSelectedValue;

        CurrentValueText.text = mCurrentStartValue.ToString();
        GameRoundManager.Instance.Data.InitialTaskValue = mCurrentStartValue;

        DescriptionText.text = "The available task value is between 0 to " + mCurrentStartValue.ToString();
        //TaskSliderValueChange();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.FINISH_SETTING);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_LINK);
    }

    public void TaskSliderValueChange()
    {
        mLastSelectedValue = (int)TaskValueSlider.value;

        mCurrentStartValue = MINIMAL_MAX_SUM + (int)TaskValueSlider.value;
        CurrentValueText.text = mCurrentStartValue.ToString();

        DescriptionText.text = "The available task value is between 0 to " + mCurrentStartValue.ToString();

        GameRoundManager.Instance.Data.InitialTaskValue = mCurrentStartValue;
        
    }

    private void SetMaxValue(int aMaxValue)
    {
        if (aMaxValue < mCurrentStartValue)
        {
            mCurrentStartValue = aMaxValue;
            TaskValueSlider.value = mCurrentStartValue;
        }

        MaxSliderValueText.text = aMaxValue.ToString();
        CurrentValueText.text = mCurrentStartValue.ToString();

        TaskValueSlider.maxValue = aMaxValue - MINIMAL_MAX_SUM;
        TaskValueSlider.interactable = !(aMaxValue == MINIMAL_MAX_SUM);

        //Objective.Instance.SetInitialObjectiveValue(mCurrentStartValue);
        //Objective.Instance.SetMaxLimitObjectiveValue(aMaxValue);
    }
}
