using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetKeybindSlot : SettingSlotForCommandBinding//SettingSlotBase
{
    public TextMeshProUGUI KeyboardBindText;
    public Image DisplayInputImage;

    public delegate void OnKeybindingSettingHaveChange(CommandIndex aCommand, KeybindData aNewKeybindingData);
    public static OnKeybindingSettingHaveChange keybindingSettingHaveChange;

    public delegate void OnGetBingingSpriteForKeybord(KeyCode aKeyCode, ref Sprite aSprite);
    public static OnGetBingingSpriteForKeybord changeKeyboardSprite;

    public delegate void OnGetBindingSpriteForXbox(XBoxButton aXBoxInput, ref Sprite aSprite);
    public static OnGetBindingSpriteForXbox changeXBoxControlSprite;

    private Sprite mDisplayingSprite;

    public override void Start()
    {
        base.Start();
        KeyboardBindText.gameObject.SetActive(false);
        DisplayInputImage.gameObject.SetActive(true);
    }

    protected override void Display()
    {
        if(mDisplayingSprite != null)
            DisplayInputImage.sprite = mDisplayingSprite;
    }

    public override void SetKey(ControlType aDisplayControlType, KeybindData aData)
    {
        base.SetKey(aDisplayControlType, aData);
        //DisplayInputImage.sprite = XBoxButtonSprites[(int)mKeybindingData.BindingXBoxBotton];
        if (mDisplayControlType == ControlType.KEYBOARD)
            mDisplayingSprite = InputSpritesManager.Instance.GetKeyboard(mKeybindingData.BindingKeyCode);
        else if (mDisplayControlType == ControlType.XBOX_360)
            mDisplayingSprite = InputSpritesManager.Instance.GetXboxButton(mKeybindingData.BindingXBoxButton);

        Display();
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
                //KeyboardBindText.text = mKeybindingData.BindingKeyCode.ToString();
                mDisplayingSprite = InputSpritesManager.Instance.GetKeyboard(kcode);
                keybindingSettingHaveChange?.Invoke(CommandIndexes[0], mKeybindingData);
                Display();
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
            SetXboxButtonCodeToData(XBoxButton.L_Shoulder);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[5]))
            SetXboxButtonCodeToData(XBoxButton.R_Shoulder);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[6]))
            SetXboxButtonCodeToData(XBoxButton.Back);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[7]))
            SetXboxButtonCodeToData(XBoxButton.Start);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[8]))
            SetXboxButtonCodeToData(XBoxButton.L_Thumb);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[9]))
            SetXboxButtonCodeToData(XBoxButton.R_Thumb);

        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[0]) == -1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Left);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[0]) == 1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Right);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[1]) == -1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Down);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[1]) == 1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Up);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[2]) == -1)
            SetXboxButtonCodeToData(XBoxButton.L_Trigger);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[2]) == 1)
            SetXboxButtonCodeToData(XBoxButton.R_Trigger);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[3]) == -1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Left);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[3]) == 1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Right);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[4]) == -1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Down);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[4]) == 1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Up);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[5]) == -1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Left);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[5]) == 1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Right);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[6]) == -1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Down);
        if (UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[6]) == 1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Up);

    }

    private void SetXboxButtonCodeToData(XBoxButton aButtonCode)
    {
        mKeybindingData.ChangeXBoxBotton(aButtonCode);
        //DisplayInputImage.sprite = XBoxButtonSprites[(int)mKeybindingData.BindingXBoxBotton];
        mDisplayingSprite = InputSpritesManager.Instance.GetXboxButton(aButtonCode);
        Display();
        keybindingSettingHaveChange?.Invoke(CommandIndexes[0], mKeybindingData);
        ActiveChangeMode(false);
    }

    private void SetXboxAxisToData(AxisInput anAxis, Vector2Int aCommandDirection)
    {
        mKeybindingData.ChangeAxisCommand(anAxis, aCommandDirection);
    }
}
