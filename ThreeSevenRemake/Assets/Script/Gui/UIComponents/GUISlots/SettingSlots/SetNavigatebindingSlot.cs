using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetNavigatebindingSlot : SettingSlotBase
{
    public List<KeyboardNaviBindBox> KeybordbindBoxes;
    public List<GamepadNaviBindBox> GamepadbindBoxes;

    public NavigatorType NavigatorType;

    public delegate void OnNavigatorSettingHaveChange(NavigatorType aType, KeybindData aNewKeybindingData);
    public static OnNavigatorSettingHaveChange navigatorSettingHaveChange;

    private KeybindData mKeybindingData = new KeybindData();
    private bool mChangeBindingEnable = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if(mChangeBindingEnable)
        { }
    }

    public override void Enter()
    {
        base.Enter();
        mChangeBindingEnable = true;
    }

    public override void Exit()
    {
        base.Exit();
        mChangeBindingEnable = false;
    }

    public void SetKey(KeybindData aData)
    {
        mKeybindingData = new KeybindData(aData);
        Display();
    }

    private void Display()
    {
    }

    private void CheckForKeyboardInput()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (kcode == KeyCode.Menu)
                return;
            if (Input.GetKeyDown(kcode))
            {
                mKeybindingData.ChangeBindingKeyCode(kcode);
                Display();
                //keybindingSettingHaveChange?.Invoke(KeybindCommand, mKeybindingData);
                return;
            }
        }
    }
}
