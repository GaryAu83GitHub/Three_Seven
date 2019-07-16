using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Tools;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    public List<SettingPanelBase> SettingPanels;

    //public Button TwoCubesButton;
    //public Button ThreeCubesButton;
    //public Button FourCubesButton;
    //public Button FiveCubesButton;

    //public Slider MaxSumSlider;
    //public TextMeshProUGUI StartMaxValueText;
    //public TextMeshProUGUI MaxSliderValueText;
    //public TextMeshProUGUI MinSliderValueText;

    //public Toggle DisplayScoringToggle;

    //public Button StartButton;
    //public Button LeaveButton;

    private Dictionary<Setting_Index, SettingPanelBase> mPanelList = new Dictionary<Setting_Index, SettingPanelBase>();

    //private int mCurrentStartValue = 0;

    private Setting_Index mCurrentDisplaySettingPanelIndex = Setting_Index.NONE;

    //private readonly int MINIMAL_MAX_SUM = 18;

    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        MainMenu.displaySettingPanel -= DisplayPanel;
        SettingPanelBase.displaySettingPanel -= DisplayPanel;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.displaySettingPanel += DisplayPanel;
        SettingPanelBase.displaySettingPanel += DisplayPanel;

        //TwoCubesButton.onClick.AddListener(TwoCubesButtonClicked);
        //ThreeCubesButton.onClick.AddListener(ThreeCubesButtonClicked);
        //FourCubesButton.onClick.AddListener(FourCubesButtonClicked);
        //FiveCubesButton.onClick.AddListener(FiveCubesButtonClicked);

        //StartButton.onClick.AddListener(StartButtonOnClick);
        //LeaveButton.onClick.AddListener(LeaveButtonOnClick);

        //mCurrentStartValue = MINIMAL_MAX_SUM;

        //MinSliderValueText.text = MINIMAL_MAX_SUM.ToString();
        //SetMasSum();

        for (int i = 0; i < SettingPanels.Count; i++)
        {
            mPanelList.Add(SettingPanels[i].PanelIndex, SettingPanels[i]);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
    }

    public void DisplayPanel(Setting_Index anIndex)
    {
        if (anIndex == Setting_Index.FINISH_SETTING)
        {
            StartCoroutine(StartGameRound());
        }
        else if(anIndex == Setting_Index.LEAVE_TO_TITLE)
        {
            mPanelList[mCurrentDisplaySettingPanelIndex].SlideOutToRight();
            mCurrentDisplaySettingPanelIndex = Setting_Index.NONE;

            StartCoroutine(ReturnToTitle());
        }
        else
        { 
            if(mCurrentDisplaySettingPanelIndex == Setting_Index.NONE)
            {
                this.gameObject.SetActive(true);
                mPanelList[anIndex].InitBaseValue();
                mPanelList[anIndex].SlideInFromRight();
                mCurrentDisplaySettingPanelIndex = anIndex;
            }
            else if(anIndex > mCurrentDisplaySettingPanelIndex)
            {
                mPanelList[mCurrentDisplaySettingPanelIndex].SlideOutToLeft();
                mPanelList[anIndex].InitBaseValue();
                mPanelList[anIndex].SlideInFromRight();
                mCurrentDisplaySettingPanelIndex = anIndex;
            }
            else if(anIndex < mCurrentDisplaySettingPanelIndex)
            {
                mPanelList[mCurrentDisplaySettingPanelIndex].SlideOutToRight();
                mPanelList[anIndex].SlideInFromLeft();
                mCurrentDisplaySettingPanelIndex = anIndex;
            }
        }
    }

    //public void MaxSumSliderValueChange()
    //{
    //    mCurrentStartValue = MINIMAL_MAX_SUM + (int)MaxSumSlider.value;
    //    Objective.Instance.SetInitialObjectiveValue(mCurrentStartValue);
    //    StartMaxValueText.text = mCurrentStartValue.ToString();
    //}

    //public void ToggleScoringDisplayMode()
    //{
    //    DisplayScoringToggle.isOn = GameSettings.Instance.ToggleScoringDisplayMethod();
    //    DisplayScoringToggle.GetComponentInChildren<TextMeshProUGUI>().text = (GameSettings.Instance.ActiveLongScoringDisplay ? "ON" : "OFF");
    //}

    //private void StartButtonOnClick()
    //{
    //    Objective.Instance.PrepareObjectives();
    //    ScreenTransistor.Instance.FadeToSceneWithIndex(1);
    //}

    //private void LeaveButtonOnClick()
    //{
    //    gameObject.SetActive(false);
    //}

    //private void TwoCubesButtonClicked()
    //{
    //    GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_2_DIGIT);
    //    TwoCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_2_DIGIT];

    //    SetMasSum();
    //}

    //private void ThreeCubesButtonClicked()
    //{
    //    GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_3_DIGIT);
    //    ThreeCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_3_DIGIT];

    //    SetMasSum();
    //}

    //private void FourCubesButtonClicked()
    //{
    //    GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_4_DIGIT);
    //    FourCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_4_DIGIT];

    //    SetMasSum();
    //}

    //private void FiveCubesButtonClicked()
    //{
    //    GameSettings.Instance.SwapScoringCubeCountOn(ScoreingLinks.LINK_5_DIGIT);
    //    FiveCubesButton.GetComponent<Image>().enabled = GameSettings.Instance.EnableScoringMethods[(int)ScoreingLinks.LINK_5_DIGIT];

    //    SetMasSum();
    //}
    
    //private void SetMasSum()
    //{
    //    int maxSum = 0;
    //    if (GameSettings.Instance.EnableScoringMethods[0])
    //    {
    //        maxSum = 9 + 9;
    //    }
    //    if (GameSettings.Instance.EnableScoringMethods[1])
    //    {
    //        maxSum = 9 + 9 + 9;
    //    }
    //    if (GameSettings.Instance.EnableScoringMethods[2])
    //    {
    //        maxSum = 9 + 9 + 9 + 9;
    //    }
    //    if (GameSettings.Instance.EnableScoringMethods[3])
    //    {
    //        maxSum = 9 + 9 + 9 + 9 + 9;
    //    }

    //    if (maxSum < mCurrentStartValue)
    //    {
    //        mCurrentStartValue = maxSum;
    //        MaxSumSlider.value = mCurrentStartValue;
    //    }

    //    MaxSliderValueText.text = maxSum.ToString();
    //    StartMaxValueText.text = mCurrentStartValue.ToString();

    //    MaxSumSlider.maxValue = maxSum - MINIMAL_MAX_SUM;
    //    MaxSumSlider.interactable = !(maxSum == MINIMAL_MAX_SUM);

    //    Objective.Instance.SetInitialObjectiveValue(mCurrentStartValue);
    //    Objective.Instance.SetMaxLimitObjectiveValue(maxSum);

    //}

    private IEnumerator ReturnToTitle()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    private IEnumerator StartGameRound()
    {
        yield return new WaitForSeconds(.5f);
        //Objective.Instance.PrepareObjectives();
        GameRoundManager.Instance.SetUpGameRound();
        ScreenTransistor.Instance.FadeToSceneWithIndex(1);
    }
}
