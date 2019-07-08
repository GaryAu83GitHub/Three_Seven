using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Tools;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    public Button TwoCubesButton;
    public Button ThreeCubesButton;
    public Button FourCubesButton;
    public Button FiveCubesButton;

    public Slider MaxSumSlider;
    public TextMeshProUGUI StartMaxValueText;
    public TextMeshProUGUI MaxSliderValueText;
    public TextMeshProUGUI MinSliderValueText;

    public Toggle DisplayScoringToggle;

    public Button StartButton;
    public Button LeaveButton;

    private int mCurrentStartValue = 0;

    private readonly int MINIMAL_MAX_SUM = 18;
    
    // Start is called before the first frame update
    void Start()
    {
        TwoCubesButton.onClick.AddListener(TwoCubesButtonClicked);
        ThreeCubesButton.onClick.AddListener(ThreeCubesButtonClicked);
        FourCubesButton.onClick.AddListener(FourCubesButtonClicked);
        FiveCubesButton.onClick.AddListener(FiveCubesButtonClicked);

        StartButton.onClick.AddListener(StartButtonOnClick);
        LeaveButton.onClick.AddListener(LeaveButtonOnClick);

        mCurrentStartValue = MINIMAL_MAX_SUM;

        MinSliderValueText.text = MINIMAL_MAX_SUM.ToString();
        SetMasSum();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MaxSumSliderValueChange()
    {
        mCurrentStartValue = MINIMAL_MAX_SUM + (int)MaxSumSlider.value;
        Objective.Instance.SetInitialObjectiveValue(mCurrentStartValue);
        StartMaxValueText.text = mCurrentStartValue.ToString();
    }

    public void ToggleScoringDisplayMode()
    {
        DisplayScoringToggle.isOn = GameSettings.Instance.ToggleScoringDisplayMethod();
        DisplayScoringToggle.GetComponentInChildren<TextMeshProUGUI>().text = (GameSettings.Instance.ActiveLongScoringDisplay ? "ON" : "OFF");
    }

    private void StartButtonOnClick()
    {
        Objective.Instance.PrepareObjectives();
        ScreenTransistor.Instance.FadeToSceneWithIndex(1);
    }

    private void LeaveButtonOnClick()
    {
        gameObject.SetActive(false);
    }

    private void TwoCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.TWO_CUBES);
        TwoCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.TWO_CUBES];

        SetMasSum();
    }

    private void ThreeCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.THREE_CUBES);
        ThreeCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.THREE_CUBES];

        SetMasSum();
    }

    private void FourCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.FOUR_CUBES);
        FourCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.FOUR_CUBES];

        SetMasSum();
    }

    private void FiveCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.FIVE_CUBES);
        FiveCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.FIVE_CUBES];

        SetMasSum();
    }
    
    private void SetMasSum()
    {
        int maxSum = 0;
        if (GameSettings.Instance.EnableScoringMethods[0])
        {
            maxSum = 9 + 9;
        }
        if (GameSettings.Instance.EnableScoringMethods[1])
        {
            maxSum = 9 + 9 + 9;
        }
        if (GameSettings.Instance.EnableScoringMethods[2])
        {
            maxSum = 9 + 9 + 9 + 9;
        }
        if (GameSettings.Instance.EnableScoringMethods[3])
        {
            maxSum = 9 + 9 + 9 + 9 + 9;
        }

        if (maxSum < mCurrentStartValue)
        {
            mCurrentStartValue = maxSum;
            MaxSumSlider.value = mCurrentStartValue;
        }

        MaxSliderValueText.text = maxSum.ToString();
        StartMaxValueText.text = mCurrentStartValue.ToString();

        MaxSumSlider.maxValue = maxSum - MINIMAL_MAX_SUM;
        MaxSumSlider.interactable = !(maxSum == MINIMAL_MAX_SUM);

        Objective.Instance.SetInitialObjectiveValue(mCurrentStartValue);
        Objective.Instance.SetMaxLimitObjectiveValue(maxSum);

    }
}
