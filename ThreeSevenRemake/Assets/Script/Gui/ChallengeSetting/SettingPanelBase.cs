using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Setting_Index
{
    NONE,
    SET_DIFFICULTY,
    SET_LINK,
    SET_START_TASK_VALUE,
    SET_NAME,
    SET_ROOF_HEIGH,
    SET_DROPPING_RATE,
    SET_PRESET,
    FINISH_SETTING,
    LEAVE_TO_TITLE,
}
public class SettingPanelBase : MonoBehaviour
{
    public Setting_Index PanelIndex;

    public Text DescriptionText;

    public Button NextButton;
    public Button PreviousButton;

    protected Animation mAnimation;

    public delegate void OnDisplaySettingPanel(Setting_Index aPanelIndex);
    public static OnDisplaySettingPanel displaySettingPanel;

    private void Awake()
    {
        mAnimation = GetComponent<Animation>();
    }
    //public virtual void Start()
    void Start()
    {
        
        //Initialize();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Initialize()
    {
        NextButton.onClick.AddListener(NextButtonOnClick);
        PreviousButton.onClick.AddListener(PreviousButtonOnClick);
    }

    public virtual void NextButtonOnClick()
    {
        //InitBaseValue();
    }

    public virtual void PreviousButtonOnClick()
    {

    }

    public virtual void InitBaseValue()
    {

    }
    
    public void SlideInFromRight()
    {
        mAnimation.Play("SlideInFromRight");
    }

    public void SlideOutToLeft()
    {
        mAnimation.Play("SlideOutToLeft");
    }

    public void SlideInFromLeft()
    {
        mAnimation.Play("SlideInFromLeft");
    }

    public void SlideOutToRight()
    {
        mAnimation.Play("SlideOutToRight");
    }

    protected bool AllLinkNumberIsEnable()
    {
        if (GameRoundManager.Instance.Data.EnableScoringMethods[0] && GameRoundManager.Instance.Data.EnableScoringMethods[1] &&
            GameRoundManager.Instance.Data.EnableScoringMethods[2] && GameRoundManager.Instance.Data.EnableScoringMethods[3])
            return true;
        return false;
    }

    protected bool OnlyTwoDigitLinkIsEnable()
    {
        if (GameRoundManager.Instance.Data.EnableScoringMethods[0] && !GameRoundManager.Instance.Data.EnableScoringMethods[1] &&
            !GameRoundManager.Instance.Data.EnableScoringMethods[2] && !GameRoundManager.Instance.Data.EnableScoringMethods[3])
            return true;
        return false;
    }
}
