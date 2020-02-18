using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingSlotWithButtons : SettingSlotBase
{
    public List<Button> Buttons;
    public Sprite ButtonSelectedSprite;

    protected int mCurrectSelectedButton = 0;

    public override void Start()
    {
        base.Start();
        SelectButton();
    }

    public override void Enter()
    {
        SelectButton();
        base.Enter();
    }

    public override void Exit()
    {
        SelectButton();
        base.Exit();
    }

    protected override void Navigation()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
        {
            NavigateButtons(-1);
        }
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
        {
            NavigateButtons(1);
        }
    }
    
    protected virtual void NavigateButtons(int aDirection)
    {
        if ((mCurrectSelectedButton + aDirection) >= Buttons.Count)
            mCurrectSelectedButton = 0;
        else if ((mCurrectSelectedButton + aDirection) < 0)
            mCurrectSelectedButton = Buttons.Count - 1;
        else
            mCurrectSelectedButton += aDirection;

        SelectButton();
    }

    protected void SelectButton()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            if(i != mCurrectSelectedButton)
                ButtonState(i, true);
            else
                ButtonState(i, false);
        }
    }

    protected void AllButtonUnInteractable()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            Buttons[i].image.sprite = UnSelectedSprite;
        }
    }

    private void ButtonState(int aButtonIndex, bool isButtonInteractable)
    {
        Buttons[aButtonIndex].interactable = isButtonInteractable;
        Color buttonColor = new Color();
        Sprite buttonSprite = Buttons[aButtonIndex].image.sprite;
        if (isButtonInteractable)
        {
            buttonColor = new Color(1, 1, 1, .5f);
            buttonSprite = UnSelectedSprite;
        }
        else
        {
            buttonColor = new Color(1, 1, 1, 1);
            buttonSprite = ButtonSelectedSprite;
        }
        Buttons[aButtonIndex].GetComponentInChildren<TextMeshProUGUI>().color = buttonColor;
        Buttons[aButtonIndex].image.sprite = buttonSprite;
    }
}
