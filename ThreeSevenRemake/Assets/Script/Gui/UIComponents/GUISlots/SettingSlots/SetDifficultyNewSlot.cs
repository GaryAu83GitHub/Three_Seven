using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetDifficultyNewSlot : SettingSlotWithButtons
{
    private Difficulties mSelectedDifficulty = GameRoundManager.Instance.Data.SelectedDifficulty;
    private int mDifficultButtonSelectIndex = 0;

    public override void Start()
    {
        base.Start();
        ButtonAppearance(mSelectedDifficulty);
        //SetDifficulty(mSelectedDifficulty);
    }

    public override void SetSlotValue(GameplaySettingData aData)
    {
        mSelectedDifficulty = aData.SelectDifficulty;
        //base.SetSlotValue(aData);
    }

    protected override void Navigation()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
        {
            SelectButton(-1);
        }
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
        {
            SelectButton(1);
        }
    }

    protected override void ChangeGameplaySetting()
    {
        mGameplaySettingData.SelectDifficulty = mSelectedDifficulty;
        base.ChangeGameplaySetting();
    }

    private void SelectButton(int aDirection)
    {
        if ((mDifficultButtonSelectIndex + aDirection) >= Buttons.Count)
            mDifficultButtonSelectIndex = (int)Difficulties.EASY;
        else if ((mDifficultButtonSelectIndex + aDirection) < 0)
            mDifficultButtonSelectIndex = (int)Difficulties.CUSTOMIZE;
        else
            mDifficultButtonSelectIndex += aDirection;

        SetDifficulty((Difficulties)mDifficultButtonSelectIndex);

    }

    private void SetDifficulty(Difficulties aDifficulty)
    {
        ButtonAppearance(aDifficulty);
        ChangeGameplaySetting();
    }

    private void ButtonAppearance(Difficulties aSelectedDifficulty)
    {
        mSelectedDifficulty = aSelectedDifficulty;
        bool isInteractable = false;

        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i == (int)aSelectedDifficulty)
                isInteractable = false;
            else
                isInteractable = true;

            ButtonState(i, isInteractable);
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
