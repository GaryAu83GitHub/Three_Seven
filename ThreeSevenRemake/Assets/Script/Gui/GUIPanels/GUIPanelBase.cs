using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanelBase : MonoBehaviour
{
    protected enum ButtonSpriteIndex
    {
        DEFAULT,
        SELECTED,
        //PICKED,
    }

    public Image BackgroundImage;
    public GameObject Container;
    public List<Button> Buttons;
    public List<AnimationClip> AnimationClips;

    public List<Sprite> ButtonStatesSprites;

    public MenuPanelIndex PanelIndex { get { return mPanelIndex; } }
    protected MenuPanelIndex mPanelIndex = MenuPanelIndex.NONE;

    protected int mCurrentSelectButtonIndex = 0;
    protected int mPreviousSelectedButtonIndex = -1;
    protected int mButtonCount = 0;
    protected bool mIsPanelInputsActive = false;

    //private Animation mAnimation;
    //private Button mCurrentSelectedButton;

    private void Awake()
    {
        ControlManager.Ins.DefaultSetting();
    }

    public virtual void Start()
    {
        //mAnimation = GetComponent<Animation>();
        //mButtonCount = Buttons.Count;
        //if (mButtonCount > 0)
        //{
        //    mCurrentSelectedButton = Buttons[mCurrentSelectButtonIndex];
        //    SwitchCurrentSelectButton();
        //}
        //ActivateButtons();
        MenuManager.Instance.AddPanel(mPanelIndex, this);
    }

    public virtual void Update()
    {
        //if(mIsPanelInputsActive)
        //    CheckInput();
    }

    public virtual void Enter()
    {
        this.gameObject.SetActive(true);
        Container.SetActive(true);
        //ActivateButtons(true);
    }

    public virtual void Exit()
    {
        this.gameObject.SetActive(false);
        Container.SetActive(false);
        //ActivateButtons(false);
    }

    //protected virtual void CheckInput()
    //{
    //    //if (!mIsPanelInputsActive)
    //    //    return;

    //    NavigateMenuButtons();
    //    if (ControlManager.Ins.MenuSelectButtonPressed())
    //        SelectButtonPressed();
    //}

    //protected virtual void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    //{
    //    if (theIncreaseCommand == theDecreaseCommand)
    //    {
    //        Debug.LogError("Increase and Decrease use same command, NavigateMenuButtons in " + this.GetType().Name + " are invalid");
    //        return;
    //    }
    //    int increament = 0;
    //    int newButtonIndex = mCurrentSelectButtonIndex;

    //    if (ControlManager.Ins.MenuNavigationPress(theIncreaseCommand))
    //        increament++;
    //    else if (ControlManager.Ins.MenuNavigationPress(theDecreaseCommand))
    //        increament--;

    //    if ((newButtonIndex + increament) < 0)
    //        newButtonIndex = mButtonCount - 1;
    //    else if ((newButtonIndex + increament) >= mButtonCount)
    //        newButtonIndex = 0;
    //    else
    //        newButtonIndex += increament;

    //    SetSelectedButton(newButtonIndex);
    //}

    //protected virtual void SelectButtonPressed()
    //{
    //}

    protected virtual void ChangePanel(MenuPanelIndex aPanelIndex)
    {
        MenuManager.Instance.GoTo(aPanelIndex);
    }

    //protected void SetSelectedButton(int aButtonIndex)
    //{
    //    mPreviousSelectedButtonIndex = mCurrentSelectButtonIndex;
    //    mCurrentSelectButtonIndex = aButtonIndex;
    //    SwitchCurrentSelectButton();
    //}

    //protected void SwitchCurrentSelectButton()
    //{
    //    ChangeSelectedButtonSprite(mPreviousSelectedButtonIndex, ButtonSpriteIndex.DEFAULT);
    //    ChangeSelectedButtonSprite(mCurrentSelectButtonIndex, ButtonSpriteIndex.SELECTED);
    //    mCurrentSelectedButton = Buttons[mCurrentSelectButtonIndex];
    //}

    //protected void ChangeSelectedButtonSprite(int aButtonIndex, ButtonSpriteIndex aStateSprite)
    //{
    //    if (aButtonIndex < 0 || aButtonIndex >= mButtonCount)
    //        return;

    //    Buttons[aButtonIndex].image.sprite = ButtonStatesSprites[(int)aStateSprite];
    //}

    //public void ActivateButtons(bool activateInput)
    //{
    //    mIsPanelInputsActive = activateInput;//!mIsPanelInputsActive;

    //    //for (int i = 0; i < Buttons.Count; i++)
    //    //    Buttons[i].interactable = mIsPanelInputsActive;
    //}
}
