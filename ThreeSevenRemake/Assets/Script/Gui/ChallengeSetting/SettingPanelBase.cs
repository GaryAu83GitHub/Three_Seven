using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Setting_Issue
{
    SET_NAME,
    SET_LINK,
    SET_START_OBJECTIVE,
    SET_DROPPING_RATE,
    SET_HIGH_LIMIT,
    MAX
}
public class SettingPanelBase : MonoBehaviour
{
    public Text DescriptionText;

    public Button NextButton;
    public Button PreviousButton;
    // Start is called before the first frame update
    protected Animation mAnimation;

    public delegate void OnDisplaySettingPanel(Setting_Issue aPanelIndex);
    public static OnDisplaySettingPanel displaySettingPanel;

    private void Awake()
    {
        mAnimation = GetComponent<Animation>();
    }

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

    }

    public virtual void PreviousButtonOnClick()
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
}
