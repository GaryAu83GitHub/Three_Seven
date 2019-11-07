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
    
    private void NavigateButtons(int aDirection)
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
        //if ((mCurrectSelectedButton + aDirection) >= Buttons.Count)
        //    mCurrectSelectedButton = 0;
        //else if ((mCurrectSelectedButton + aDirection) < 0)
        //    mCurrectSelectedButton = Buttons.Count - 1;
        //else
        //    mCurrectSelectedButton += aDirection;

        for (int i = 0; i < Buttons.Count; i++)
        {
            if(i != mCurrectSelectedButton)
                ButtonState(i, true);
            else
                ButtonState(i, false);
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
