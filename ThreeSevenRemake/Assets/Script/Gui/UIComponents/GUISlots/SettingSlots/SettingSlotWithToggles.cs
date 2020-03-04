using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSlotWithToggles : SettingSlotBase
{
    public List<Toggle> Toggles;

    protected int mCurrentSelectToggleIndex = 0;
    protected int mActiveToggleCount = 0;

    public override void Start()
    {
        base.Start();
        mActiveToggleCount = Toggles.Count;
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void Navigation()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
        {
            NavigateToggles(-1);
        }
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
        {
            NavigateToggles(1);
        }
    }

    protected override void MenuButtonPressed()
    {
        if (ControlManager.Ins.MenuSelectButtonPressed())
        {
            SwitchToggle();
            ChangeGameplaySetting();
        }
    }

    private void NavigateToggles(int aDirection)
    {
        if ((mCurrentSelectToggleIndex + aDirection) >= mActiveToggleCount/*Toggles.Count*/)
            mCurrentSelectToggleIndex = 0;
        else if ((mCurrentSelectToggleIndex + aDirection) < 0)
            mCurrentSelectToggleIndex = mActiveToggleCount - 1;/*Toggles.Count*/
        else
            mCurrentSelectToggleIndex += aDirection;

        ActiveToggle();
    }

    /// <summary>
    /// Switch the toggle of the current selected toggle index
    /// </summary>
    protected virtual void SwitchToggle()
    {
        Toggles[mCurrentSelectToggleIndex].isOn = !Toggles[mCurrentSelectToggleIndex].isOn;
    }

    protected virtual void ActiveToggle()
    {
        for (int i = 0; i < Toggles.Count; i++)
            Toggles[i].interactable = (i == mCurrentSelectToggleIndex) ? true : false;

        //Toggles[mCurrentSelectToggleIndex].interactable = true;
    }
}
