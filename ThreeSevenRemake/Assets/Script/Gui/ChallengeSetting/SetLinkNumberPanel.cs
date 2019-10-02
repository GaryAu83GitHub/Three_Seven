using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetLinkNumberPanel : SettingPanelBase
{
    public List<Button> LinkButtons;

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

        LinkButtons[(int)LinkIndexes.LINK_2_DIGIT].onClick.AddListener(TwoCubesButtonClicked);
        LinkButtons[(int)LinkIndexes.LINK_3_DIGIT].onClick.AddListener(ThreeCubesButtonClicked);
        LinkButtons[(int)LinkIndexes.LINK_4_DIGIT].onClick.AddListener(FourCubesButtonClicked);
        LinkButtons[(int)LinkIndexes.LINK_5_DIGIT].onClick.AddListener(FiveCubesButtonClicked);

        //DescriptionText.text = "";
    }

    public override void InitBaseValue()
    {
        base.InitBaseValue();

        for(LinkIndexes i = LinkIndexes.LINK_2_DIGIT; i != LinkIndexes.MAX; i++)
            ButtonDisplay(i);
        SetMaxSum();
    }

    public override void NextButtonOnClick()
    {
        base.NextButtonOnClick();
        SetMaxSum();
        displaySettingPanel?.Invoke(Setting_Index.SET_NAME);
        //if(OnlyTwoDigitLinkIsEnable())
        //    displaySettingPanel?.Invoke(Setting_Index.SET_NAME);
        //else
        //    displaySettingPanel?.Invoke(Setting_Index.SET_START_TASK_VALUE);
    }

    public override void PreviousButtonOnClick()
    {
        base.PreviousButtonOnClick();
        displaySettingPanel?.Invoke(Setting_Index.SET_DIFFICULTY);
    }

    private void TwoCubesButtonClicked()
    {
        SwapScoringCubeCountOn(LinkIndexes.LINK_2_DIGIT);
        ButtonDisplay(LinkIndexes.LINK_2_DIGIT);
        DescriptionText.text = "2 digit addition are " + GetEnableText(LinkIndexes.LINK_2_DIGIT);

        SetMaxSum();
    }

    private void ThreeCubesButtonClicked()
    {
        SwapScoringCubeCountOn(LinkIndexes.LINK_3_DIGIT);
        ButtonDisplay(LinkIndexes.LINK_3_DIGIT);
        DescriptionText.text = "3 digit addition are " + GetEnableText(LinkIndexes.LINK_3_DIGIT);

        SetMaxSum();
    }

    private void FourCubesButtonClicked()
    {
        SwapScoringCubeCountOn(LinkIndexes.LINK_4_DIGIT);
        ButtonDisplay(LinkIndexes.LINK_4_DIGIT);
        DescriptionText.text = "4 digit addition are " + GetEnableText(LinkIndexes.LINK_4_DIGIT);

        SetMaxSum();
    }

    private void FiveCubesButtonClicked()
    {
        SwapScoringCubeCountOn(LinkIndexes.LINK_5_DIGIT);
        ButtonDisplay(LinkIndexes.LINK_5_DIGIT);
        DescriptionText.text = "5 digit addition are " + GetEnableText(LinkIndexes.LINK_5_DIGIT);

        SetMaxSum();
    }

    private void ButtonDisplay(LinkIndexes aLink)
    {
        bool onOff = GameRoundManager.Instance.Data.EnableScoringMethods[(int)aLink];
        LinkButtons[(int)aLink].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (onOff ? "ON" : "OFF");
        LinkButtons[(int)aLink].transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = (onOff ? Color.green : Color.red);
    }

    private void SetMaxSum()
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

        GameRoundManager.Instance.Data.InitialTaskValue = Constants.MINIMAL_TASK_VALUE;
        changeTaskMaskValue?.Invoke(maxSum);
    }

    private string GetEnableText(LinkIndexes aLink)
    {
        return (GameSettings.Instance.EnableScoringMethods[(int)LinkIndexes.LINK_5_DIGIT] ? "enable" : "disable");
    }

    private void SwapScoringCubeCountOn(LinkIndexes anIndex)
    {
        bool isThereAnotherOptionEnable = false;

        for (LinkIndexes i = LinkIndexes.LINK_2_DIGIT; i < LinkIndexes.MAX; i++)
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
