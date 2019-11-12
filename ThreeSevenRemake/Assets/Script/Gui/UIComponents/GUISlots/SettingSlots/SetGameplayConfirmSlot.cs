using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameplayConfirmSlot : SettingSlotWithButtons
{
    private enum ButtonIndex
    {
        APPLY,
        RESET,
    }
    public delegate void OnApplyButtonPressed();
    public static OnApplyButtonPressed applyButtonPressed;

    public delegate void OnResetButtonPressed();
    public static OnApplyButtonPressed resetButtonPressed;

    public override void Start()
    {
        base.Start();
    }

    public override void Exit()
    {
        mCurrectSelectedButton = (int)ButtonIndex.APPLY;
        base.Exit();
    }

    protected override void MenuButtonPressed()
    {
        if (ControlManager.Ins.MenuSelectButtonPressed())
        {
            if (mCurrectSelectedButton == (int)ButtonIndex.APPLY)
                applyButtonPressed?.Invoke();
            if (mCurrectSelectedButton == (int)ButtonIndex.RESET)
                resetButtonPressed?.Invoke();
        }
    }
}
