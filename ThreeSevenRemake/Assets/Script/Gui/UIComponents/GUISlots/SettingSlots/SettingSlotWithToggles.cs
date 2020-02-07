using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSlotWithToggles : SettingSlotBase
{
    public List<Toggle> Toggles;

    protected int mCurrentSelectToggleIndex = 0;

    public override void Start()
    {
        base.Start();
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
        if ((mCurrentSelectToggleIndex + aDirection) >= Toggles.Count)
            mCurrentSelectToggleIndex = 0;
        else if ((mCurrentSelectToggleIndex + aDirection) < 0)
            mCurrentSelectToggleIndex = Toggles.Count - 1;
        else
            mCurrentSelectToggleIndex += aDirection;

        ActiveToggle();
    }

    protected virtual void SwitchToggle()
    {
        Toggles[mCurrentSelectToggleIndex].isOn = !Toggles[mCurrentSelectToggleIndex].isOn;
    }

    private void ActiveToggle()
    {
        for (int i = 0; i < Toggles.Count; i++)
            Toggles[i].interactable = false;

        Toggles[mCurrentSelectToggleIndex].interactable = true;
    }
}
