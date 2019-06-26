using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Tools;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    public Button EasyButton;
    public Button NormalButton;
    public Button HardButton;

    public Button TwoCubesButton;
    public Button ThreeCubesButton;
    public Button FourCubesButton;
    public Button FiveCubesButton;

    public Slider MaxSumSlider;
    public TextMeshProUGUI MaxSumText;

    public Button StartButton;
    public Button LeaveButton;

    private List<bool> mDifficultyBools = new List<bool>();

    private int mMaxSum = 18;

    private int MINIMAL_MAX_SUM = 18;
    
    // Start is called before the first frame update
    void Start()
    {
        EasyButton.onClick.AddListener(SetToEasy);
        NormalButton.onClick.AddListener(SetToNormal);
        HardButton.onClick.AddListener(SetToHard);
        TwoCubesButton.onClick.AddListener(TwoCubesButtonClicked);
        ThreeCubesButton.onClick.AddListener(ThreeCubesButtonClicked);
        FourCubesButton.onClick.AddListener(FourCubesButtonClicked);
        FiveCubesButton.onClick.AddListener(FiveCubesButtonClicked);

        StartButton.onClick.AddListener(StartButtonOnClick);
        LeaveButton.onClick.AddListener(LeaveButtonOnClick);

        mDifficultyBools.Add(false);
        mDifficultyBools.Add(false);
        mDifficultyBools.Add(false);

        SetDifficulties(GameSettings.Instance.Difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToEasy()
    {
        SetDifficulties(Difficulties.EASY);
    }

    public void SetToNormal()
    {
        SetDifficulties(Difficulties.NORMAL);
    }

    public void SetToHard()
    {
        SetDifficulties(Difficulties.HARD);
    }

    public void SetMaxSum()
    {
        MaxSumText.text = (MINIMAL_MAX_SUM + MaxSumSlider.value).ToString();
        
        //GameSettings.Instance.SetStartDropSpeed((int)MaxSumSlider.value);
    }

    private void StartButtonOnClick()
    {
        Objective.Instance.PrepareObjectives();
        ScreenTransistor.Instance.FadeToSceneWithIndex(1);
        //gameObject.SetActive(false);
    }

    private void LeaveButtonOnClick()
    {
        gameObject.SetActive(false);
    }

    private void TwoCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.TWO_CUBES);
        TwoCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.ActiveScoringCubeCount[(int)ScoreCubeCount.TWO_CUBES];

        SetMasSum();
    }

    private void ThreeCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.THREE_CUBES);
        ThreeCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.ActiveScoringCubeCount[(int)ScoreCubeCount.THREE_CUBES];

        SetMasSum();
    }

    private void FourCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.FOUR_CUBES);
        FourCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.ActiveScoringCubeCount[(int)ScoreCubeCount.FOUR_CUBES];

        SetMasSum();
    }

    private void FiveCubesButtonClicked()
    {
        GameSettings.Instance.SwapScoringCubeCountOn(ScoreCubeCount.FIVE_CUBES);
        FiveCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.ActiveScoringCubeCount[(int)ScoreCubeCount.FIVE_CUBES];

        SetMasSum();
    }

    private void SetDifficulties(Difficulties aDifficulty)
    {
        ResetDifficulty();
        mDifficultyBools[(int)aDifficulty] = true;

        EasyButton.interactable = !mDifficultyBools[(int)Difficulties.EASY];
        NormalButton.interactable = !mDifficultyBools[(int)Difficulties.NORMAL];
        HardButton.interactable = !mDifficultyBools[(int)Difficulties.HARD];

        GameSettings.Instance.SetDifficulty(aDifficulty);
    }

    private void ResetDifficulty()
    {
        for (int i = 0; i < mDifficultyBools.Count; i++)
            mDifficultyBools[i] = false;
    }

    private void SetMasSum()
    {
        if (GameSettings.Instance.ActiveScoringCubeCount[0])
        {
            mMaxSum = 9 + 9;
        }
        if (GameSettings.Instance.ActiveScoringCubeCount[1])
        {
            mMaxSum = 9 + 9 + 9;
        }
        if (GameSettings.Instance.ActiveScoringCubeCount[2])
        {
            mMaxSum = 9 + 9 + 9 + 9;
        }
        if (GameSettings.Instance.ActiveScoringCubeCount[3])
        {
            mMaxSum = 9 + 9 + 9 + 9 + 9;
        }

        MaxSumSlider.maxValue = mMaxSum - MINIMAL_MAX_SUM;
        MaxSumSlider.value = mMaxSum;
        MaxSumSlider.interactable = !(mMaxSum == MINIMAL_MAX_SUM);
    }
}
