using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetKeybindSlot : SettingSlotForCommandBinding//SettingSlotBase
{
    //public CommandIndex KeybindCommand;

    //public GameObject KeybindContainer;

    public TextMeshProUGUI KeyboardBindText;
    public TextMeshProUGUI GamepadBindText;

    public delegate void OnKeybindingSettingHaveChange(CommandIndex aCommand, KeybindData aNewKeybindingData);
    public static OnKeybindingSettingHaveChange keybindingSettingHaveChange;

    //private CanvasGroup mBindContainerMG;

    //private KeybindData mKeybindingData = new KeybindData();
    //private bool mChangeModeOn = false;

    //private float mChangeModeSuspendCountdown = 0f;
    //private const float mChangeModeSuspendTime = .05f;

    //public override void Start()
    //{
    //    base.Start();
    //    if(KeybindContainer != null)
    //        mBindContainerMG = KeybindContainer.GetComponent<CanvasGroup>();
    //    ActiveChangeMode(false);
    //}

    //public override void Update()
    //{
    //    base.Update();

    //    if (mChangeModeSuspendCountdown > 0)
    //        mChangeModeSuspendCountdown -= Time.deltaTime;

    //    if (mChangeModeOn && mChangeModeSuspendCountdown <= 0f)
    //    {
    //        CheckForKeyboardInput();
    //        CheckForXboxBottonInput();
    //    }
    //}

    //public override void Enter()
    //{
    //    base.Enter();
    //    ActiveChangeMode(false);
    //}

    //public override void Exit()
    //{
    //    base.Exit();
    //    ActiveChangeMode(false);
    //}

    //protected override void MenuButtonPressed()
    //{
    //    if(ControlManager.Ins.MenuSelectButtonPressed() && !mChangeModeOn)
    //        ActiveChangeMode(true);
    //}

    //public void SetKey(KeybindData aData)
    //{
    //    mKeybindingData = new KeybindData(aData);
    //    Display();
    //}

    protected override void Display()
    {
        KeyboardBindText.text = mKeybindingData.BindingKeyCode.ToString();
        GamepadBindText.text = mKeybindingData.BindingXBoxBotton.ToString();
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

    //private void ActiveChangeMode(bool isChangeModeOn)
    //{
    //    if (KeybindContainer == null)
    //        return;


    //    mChangeModeOn = isChangeModeOn;
    //    mLockParentInput = mChangeModeOn;

    //    mChangeModeSuspendCountdown = (isChangeModeOn) ? mChangeModeSuspendTime : 0f;
    //    mBindContainerMG.alpha = (mChangeModeOn ? 1f : .5f);
    //}
}
