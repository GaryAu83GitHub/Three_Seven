using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetKeybindSlot : SettingSlotBase
{
    public TextMeshProUGUI KeyboardBindText;
    public TextMeshProUGUI GamepadBindText;

    public CommandIndex KeybindCommand;

    public delegate void OnKeybindingSettingHaveChange(CommandIndex aCommand, KeybindData aNewKeybindingData);
    public static OnKeybindingSettingHaveChange keybindingSettingHaveChange;

    private KeybindData mKeybindingData = new KeybindData();
    private bool mChangeBindingEnable = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (mChangeBindingEnable)
        {
            CheckForKeyboardInput();
            CheckForXboxBottonInput();
        }
    }

    public void SetKey(KeybindData aData)
    {
        mKeybindingData = new KeybindData(aData);
        Display();
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

    private void Display()
    {
        KeyboardBindText.text = mKeybindingData.BindingKeyCode.ToString();
        GamepadBindText.text = mKeybindingData.BindingXBoxBotton.ToString();
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
                keybindingSettingHaveChange?.Invoke(KeybindCommand, mKeybindingData);
                return;
            }
        }
    }

    private void CheckForXboxBottonInput()
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
    }
}
