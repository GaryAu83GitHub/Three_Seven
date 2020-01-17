using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveResultButtonsSlot : ResultSlotBase
{
    public List<Button> Buttons;
    public Sprite SelectButtonSprite;
    public Sprite UnselectButtonSprite;

    public delegate void OnSelectSaveResultButton(int aSelectIndex);
    public static OnSelectSaveResultButton selectSaveRusultBotton;

    private int mSelectedButtonIndex = 0;
    private bool mActiveRegrateButton = false;

    public override void Awake()
    {
        ResultPanel.activeRegistrateButtons += ActiveRegistrateButtons;
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        mActiveRegrateButton = false;
    }

    private void OnDestroy()
    {
        ResultPanel.activeRegistrateButtons -= ActiveRegistrateButtons;
    }

    public override void Update()
    {
        base.Update();
        if(mActiveRegrateButton)
        {
            NavigationInput();
            SelectButtonInput();
        }
    }

    private void NavigationInput()
    {
        if(ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
            mSelectedButtonIndex++;
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
            mSelectedButtonIndex--;


        if (mSelectedButtonIndex >= Buttons.Count)
            mSelectedButtonIndex = 0;
        else if (mSelectedButtonIndex < 0)
            mSelectedButtonIndex = Buttons.Count - 1;

        DisplayButton(mSelectedButtonIndex);
    }

    private void SelectButtonInput()
    {
        if (ControlManager.Ins.MenuSelectButtonPressed())
            selectSaveRusultBotton?.Invoke(mSelectedButtonIndex);
    }

    private void DisplayButton(int aButtonIndex)
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i == aButtonIndex)
                Buttons[i].image.sprite = SelectButtonSprite;
            else
                Buttons[i].image.sprite = UnselectButtonSprite;
        }
    }

    private void ActiveRegistrateButtons(bool anActivation)
    {
        mActiveRegrateButton = anActivation;
    }
}
