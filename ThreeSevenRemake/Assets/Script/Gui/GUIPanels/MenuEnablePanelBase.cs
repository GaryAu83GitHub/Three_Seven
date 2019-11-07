using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEnablePanelBase : GUIPanelBase
{
    private Animation mAnimation;
    private Button mCurrentSelectedButton;

    public override void Start()
    {
        mAnimation = GetComponent<Animation>();
        mButtonCount = Buttons.Count;
        if (mButtonCount > 0)
        {
            mCurrentSelectedButton = Buttons[mCurrentSelectButtonIndex];
            SwitchCurrentSelectButton();
        }
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (mIsPanelInputsActive)
            CheckInput();
    }

    public override void Enter()
    {
        base.Enter();
        ActivateButtons(true);
    }

    public override void Exit()
    {
        base.Exit();
        ActivateButtons(false);
    }

    protected virtual void CheckInput()
    {
        //if (!mIsPanelInputsActive)
        //    return;

        NavigateMenuButtons();
        if (ControlManager.Ins.MenuSelectButtonPressed())
            SelectButtonPressed();
    }

    protected virtual void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    {
        if (theIncreaseCommand == theDecreaseCommand)
        {
            Debug.LogError("Increase and Decrease use same command, NavigateMenuButtons in " + this.GetType().Name + " are invalid");
            return;
        }
        int increament = 0;
        int newButtonIndex = mCurrentSelectButtonIndex;

        if (ControlManager.Ins.MenuNavigationPress(theIncreaseCommand))
            increament++;
        else if (ControlManager.Ins.MenuNavigationPress(theDecreaseCommand))
            increament--;

        if ((newButtonIndex + increament) < 0)
            newButtonIndex = mButtonCount - 1;
        else if ((newButtonIndex + increament) >= mButtonCount)
            newButtonIndex = 0;
        else
            newButtonIndex += increament;

        SetSelectedButton(newButtonIndex);
    }

    protected virtual void SelectButtonPressed()
    {
    }

    protected void SetSelectedButton(int aButtonIndex)
    {
        mPreviousSelectedButtonIndex = mCurrentSelectButtonIndex;
        mCurrentSelectButtonIndex = aButtonIndex;
        SwitchCurrentSelectButton();
    }

    protected void SwitchCurrentSelectButton()
    {
        ChangeSelectedButtonSprite(mPreviousSelectedButtonIndex, ButtonSpriteIndex.DEFAULT);
        ChangeSelectedButtonSprite(mCurrentSelectButtonIndex, ButtonSpriteIndex.SELECTED);
        mCurrentSelectedButton = Buttons[mCurrentSelectButtonIndex];
    }

    protected void ChangeSelectedButtonSprite(int aButtonIndex, ButtonSpriteIndex aStateSprite)
    {
        if (aButtonIndex < 0 || aButtonIndex >= mButtonCount)
            return;

        Buttons[aButtonIndex].image.sprite = ButtonStatesSprites[(int)aStateSprite];
    }

    public void ActivateButtons(bool activateInput)
    {
        mIsPanelInputsActive = activateInput;//!mIsPanelInputsActive;

        //for (int i = 0; i < Buttons.Count; i++)
        //    Buttons[i].interactable = mIsPanelInputsActive;
    }
}
