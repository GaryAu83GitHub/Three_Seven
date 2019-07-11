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

   

    private Vector3 mCurrentValueTextPosition = new Vector3();

    private int mCurrentStartValue = 0;

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
        mCurrentValueTextPosition = new Vector3(-275f, 0f, 0f);

    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        //displaySettingPanel?.Invoke(Setting_Issue.SET_NAME + 1);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Issue.SET_START_OBJECTIVE - 1);
    }

    public void TaskSliderValueChange()
    {
        mCurrentStartValue = MINIMAL_MAX_SUM + (int)TaskValueSlider.value;
        //Objective.Instance.SetInitialObjectiveValue(mCurrentStartValue);
        CurrentValueText.text = mCurrentStartValue.ToString();

        //CurrentValueText.rectTransform.localPosition = mCurrentValueTextPosition + new Vector3(TaskValueSlider.value, 0f, 0f);

        DescriptionText.text = "The available task value is between " + MINIMAL_MAX_SUM.ToString() + " to " + mCurrentStartValue.ToString();
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
