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
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.TWO_CUBES);
        TwoDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.TWO_CUBES];

        Debug.Log("2 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.TWO_CUBES].ToString());

        SetMasSum();
    }

    private void ThreeCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.THREE_CUBES);
        ThreeDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.THREE_CUBES];

        Debug.Log("3 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.THREE_CUBES].ToString());

        SetMasSum();
    }

    private void FourCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.FOUR_CUBES);
        FourDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.FOUR_CUBES];

        Debug.Log("4 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.FOUR_CUBES].ToString());

        SetMasSum();
    }

    private void FiveCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.FIVE_CUBES);
        FiveDigitButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.FIVE_CUBES];

        Debug.Log("5 digit addition: " + GameSettings.Instance.EnableScoringMethods[(int)ScoreCubeCount.FIVE_CUBES].ToString());

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
}
