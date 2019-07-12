using UnityEngine;
using UnityEngine.UI;

public class SetLinkNumberPanel : SettingPanelBase
{
    public Button TwoDigitButton;
    public Button ThreeDigitButton;
    public Button FourDigitButton;
    public Button FiveDigitButton;

    public delegate void OnChangeTaskMaxValue(int aMaxValue);
    public static OnChangeTaskMaxValue changeTaskMaskValue;

    void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
        TwoDigitButton.onClick.AddListener(TwoCubesButtonClicked);
        ThreeDigitButton.onClick.AddListener(ThreeCubesButtonClicked);
        FourDigitButton.onClick.AddListener(FourCubesButtonClicked);
        FiveDigitButton.onClick.AddListener(FiveCubesButtonClicked);

        //DescriptionText.text = "";
    }

    public override void InitBaseValue()
    {
        base.InitBaseValue();

        TwoDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT];
        ThreeDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT];
        FourDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT];
        FiveDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT];
        SetMasSum();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        SetMasSum();
        if(OnlyTwoDigitLinkIsEnable())
            displaySettingPanel?.Invoke(Setting_Index.SET_ROOF_HEIGH);
        else
            displaySettingPanel?.Invoke(Setting_Index.SET_START_TASK_VALUE);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_NAME);
    }

    private void TwoCubesButtonClicked()
    {
        SwapScoringCubeCountOn(ScoreingLinks.LINK_2_DIGIT);
        TwoDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT];

        DescriptionText.text = "2 digit addition are " + GetEnableText(ScoreingLinks.LINK_2_DIGIT);

        SetMasSum();
    }

    private void ThreeCubesButtonClicked()
    {
        SwapScoringCubeCountOn(ScoreingLinks.LINK_3_DIGIT);
        ThreeDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT];

        DescriptionText.text = "3 digit addition are " + GetEnableText(ScoreingLinks.LINK_3_DIGIT);

        SetMasSum();
    }

    private void FourCubesButtonClicked()
    {
        SwapScoringCubeCountOn(ScoreingLinks.LINK_4_DIGIT);
        FourDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT];

        DescriptionText.text = "4 digit addition are " + GetEnableText(ScoreingLinks.LINK_4_DIGIT);

        SetMasSum();
    }

    private void FiveCubesButtonClicked()
    {
        SwapScoringCubeCountOn(ScoreingLinks.LINK_5_DIGIT);
        FiveDigitButton.GetComponent<Image>().enabled = GameRoundManager.Instance.Data.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT];

        DescriptionText.text = "5 digit addition are " + GetEnableText(ScoreingLinks.LINK_5_DIGIT);

        SetMasSum();
    }

    private void SetMasSum()
    {
        int maxSum = 0;
        if (GameRoundManager.Instance.Data.EnableScoringMethods[0])
        {
            maxSum = 9 + 9;
        }
        if (GameRoundManager.Instance.Data.EnableScoringMethods[1])
        {
            maxSum = 9 + 9 + 9;
        }
        if (GameRoundManager.Instance.Data.EnableScoringMethods[2])
        {
            maxSum = 9 + 9 + 9 + 9;
        }
        if (GameRoundManager.Instance.Data.EnableScoringMethods[3])
        {
            maxSum = 9 + 9 + 9 + 9 + 9;
        }

        if(AllLinkNumberIsEnable()) 
            DescriptionText.text = "All digit addition are enable";
        else if(OnlyTwoDigitLinkIsEnable())
            DescriptionText.text = "Only 2 digit addition are enable. The available task value is between 0 to 18";

        GameRoundManager.Instance.Data.InitialTaskValue = Constants.MINIMAL_TASK_SUM;
        Objective.Instance.SetMaxLimitObjectiveValue(maxSum);
        changeTaskMaskValue?.Invoke(maxSum);
    }

    private string GetEnableText(ScoreingLinks aLink)
    {
        return (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT] ? "enable" : "disable");
    }

    private void SwapScoringCubeCountOn(ScoreingLinks anIndex)
    {
        bool isThereAnotherOptionEnable = false;

        for (ScoreingLinks i = ScoreingLinks.LINK_2_DIGIT; i < ScoreingLinks.MAX; i++)
        {
            if (i == anIndex)
                continue;
            if (isThereAnotherOptionEnable == false && GameRoundManager.Instance.Data.EnableScoringMethods[(int)i] == true)
                isThereAnotherOptionEnable = GameRoundManager.Instance.Data.EnableScoringMethods[(int)i];

        }
        if (isThereAnotherOptionEnable)
            GameRoundManager.Instance.Data.EnableScoringMethods[(int)anIndex] = !GameRoundManager.Instance.Data.EnableScoringMethods[(int)anIndex];
    }
}
