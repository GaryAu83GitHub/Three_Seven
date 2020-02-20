using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This class is attach to the SetDifficultyNewSlot prefab from the game which will be
/// replacing the previous SetDifficultSlot that had difficulty buttons and challenge
/// digit check boxes in one.
/// This object is simple storing which gameflow the player will play the game until
/// the player apply the changing and send it to the game setting data
/// </summary>
public class SetDifficultyNewSlot : SettingSlotWithButtons
{
    private Difficulties mSelectedDifficulty = GameRoundManager.Instance.Data.SelectedDifficulty;
    
    public override void Start()
    {
        base.Start();
        SelectButton();
        //ButtonAppearance();
    }

    public override void SetSlotValue(GameplaySettingData aData)
    {
        mSelectedDifficulty = aData.SelectDifficulty;
        mCurrectSelectedButton = (int)mSelectedDifficulty;
        SelectButton();
        base.SetSlotValue(aData);
    }

    protected override void NavigateButtons(int aDirection)
    {
        base.NavigateButtons(aDirection);
        SetDifficulty((Difficulties)mCurrectSelectedButton);
    }

    protected override void ChangeGameplaySetting()
    {
        mGameplaySettingData.SelectDifficulty = mSelectedDifficulty;
        base.ChangeGameplaySetting();
    }
    
    private void SetDifficulty(Difficulties aDifficulty)
    {
        mSelectedDifficulty = aDifficulty;
        //ButtonAppearance();
        ChangeGameplaySetting();
    }

    // all belowe here will be removed since these function was already declared
    // on the superior class
    private void ButtonAppearance()
    {
        
        bool isInteractable = false;

        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i == (int)mSelectedDifficulty)
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
