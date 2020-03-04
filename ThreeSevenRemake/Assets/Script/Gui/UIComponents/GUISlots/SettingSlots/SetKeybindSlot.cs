using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetKeybindSlot : SettingSlotForCommandBinding//SettingSlotBase
{
    public CommandIndex KeybindCommand;
    //public CommandIndex KeybindCommand;

    //public GameObject KeybindContainer;

    public TextMeshProUGUI KeyboardBindText;
    public TextMeshProUGUI GamepadBindText;

    public delegate void OnKeybindingSettingHaveChange(CommandIndex aCommand, KeybindData aNewKeybindingData);
    public static OnKeybindingSettingHaveChange keybindingSettingHaveChange;

    protected override void Display()
    {
        if(mDisplayControlType == ControlType.KEYBOARD)
            KeyboardBindText.text = mKeybindingData.BindingKeyCode.ToString();
        else if(mDisplayControlType == ControlType.XBOX_360)
            KeyboardBindText.text = mKeybindingData.BindingXBoxBotton.ToString();
        //GamepadBindText.text = mKeybindingData.BindingXBoxBotton.ToString();
    }

    protected override void CheckForKeyboardInput()//private void CheckForKeyboardInput()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (kcode == KeyCode.Menu)
                return;
            if (Input.GetKeyDown(kcode))
            {
                mKeybindingData.ChangeBindingKeyCode(kcode);
                Display();
                keybindingSettingHaveChange?.Invoke(KeybindCommand, mKeybindingData);
                ActiveChangeMode(false);
                return;
            }
        }
    }

    protected override void CheckForGamepadInput()//private void CheckForXboxBottonInput()
    {
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[0]))
            SetXboxButtonCodeToData(XBoxButton.A);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[1]))
            SetXboxButtonCodeToData(XBoxButton.B);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[2]))
            SetXboxButtonCodeToData(XBoxButton.X);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[3]))
            SetXboxButtonCodeToData(XBoxButton.Y);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[4]))
            SetXboxButtonCodeToData(XBoxButton.L_SHOULDER);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[5]))
            SetXboxButtonCodeToData(XBoxButton.R_SHOULDER);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[8]))
            SetXboxButtonCodeToData(XBoxButton.L_THUMB_PRESSED);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[9]))
            SetXboxButtonCodeToData(XBoxButton.R_THUMB_PRESSED);
    }

    private void SetXboxButtonCodeToData(XBoxButton aButtonCode)
    {
        mKeybindingData.ChangeXBoxBotton(aButtonCode);
        Display();
        keybindingSettingHaveChange?.Invoke(KeybindCommand, mKeybindingData);
        ActiveChangeMode(false);
    }
}
