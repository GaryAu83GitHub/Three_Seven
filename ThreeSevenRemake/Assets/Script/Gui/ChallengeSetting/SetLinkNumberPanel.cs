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

        TwoDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT];
        ThreeDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT];
        FourDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT];
        FiveDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT];

        DescriptionText.text = "";
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Issue.SET_LINK + 1);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Issue.SET_LINK - 1);
    }

    private void TwoCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_2_DIGIT);
        TwoDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT];

        //Debug.Log("2 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT].ToString());
        DescriptionText.text = "2 digit addition are " + GetEnableText(ScoreingLinks.LINK_2_DIGIT);

        SetMasSum();
    }

    private void ThreeCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_3_DIGIT);
        ThreeDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT];

        //Debug.Log("3 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT].ToString());
        DescriptionText.text = "3 digit addition are " + GetEnableText(ScoreingLinks.LINK_3_DIGIT);

        SetMasSum();
    }

    private void FourCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_4_DIGIT);
        FourDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT];

        //Debug.Log("4 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT].ToString());
        DescriptionText.text = "4 digit addition are " + GetEnableText(ScoreingLinks.LINK_4_DIGIT);

        SetMasSum();
    }

    private void FiveCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_5_DIGIT);
        FiveDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT];

        //Debug.Log("5 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT].ToString());
        DescriptionText.text = "5 digit addition are " + GetEnableText(ScoreingLinks.LINK_5_DIGIT);

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

        changeTaskMaskValue?.Invoke(maxSum);
    }

    private string GetEnableText(ScoreingLinks aLink)
    {
        return (GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT] ? "enable" : "disable");
    }
}
