using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetKeybindSlot : SettingSlotForCommandBinding//SettingSlotBase
{
    public CommandIndex KeybindCommand;
    //public CommandIndex KeybindCommand;

    //public GameObject KeybindContainer;

    public TextMeshProUGUI KeyboardBindText;
    public Image GamepadBindButtonImage;

    public List<Sprite> KeyboadKeysSprites;

    public List<Sprite> XBoxButtonSprites;
    public List<Sprite> XBoxLeftStickSprites;
    public List<Sprite> XBoxRightStickSprites;
    public List<Sprite> XBoxDPadsSprites;
    public List<Sprite> XboxTriggerSpirtes;

    public delegate void OnKeybindingSettingHaveChange(CommandIndex aCommand, KeybindData aNewKeybindingData);
    public static OnKeybindingSettingHaveChange keybindingSettingHaveChange;

    public delegate void OnGetBingingSpriteForKeybord(KeyCode aKeyCode, ref Sprite aSprite);
    public static OnGetBingingSpriteForKeybord changeKeyboardSprite;

    public delegate void OnGetBindingSpriteForXbox(XBoxButton aXBoxInput, ref Sprite aSprite);
    public static OnGetBindingSpriteForXbox changeXBoxControlSprite;

    private Sprite mDisplayXboxButtonSprite;

    protected override void Display()
    {
        if (mDisplayControlType == ControlType.KEYBOARD)
        {
            KeyboardBindText.gameObject.SetActive(true);//text = mKeybindingData.BindingKeyCode.ToString();
            GamepadBindButtonImage.gameObject.SetActive(false);
        }
        else if (mDisplayControlType == ControlType.XBOX_360)
        {
            KeyboardBindText.gameObject.SetActive(false);//text = mKeybindingData.BindingXBoxBotton.ToString();
            GamepadBindButtonImage.gameObject.SetActive(true);
            //GamepadBindText.text = mKeybindingData.BindingXBoxBotton.ToString();
        }
    }

    public override void SetKey(ControlType aDisplayControlType, KeybindData aData)
    {
        base.SetKey(aDisplayControlType, aData);
        GamepadBindButtonImage.sprite = XBoxButtonSprites[(int)mKeybindingData.BindingXBoxBotton];
    }

    protected override void CheckForKeyboardInput()//private void CheckForKeyboardInput()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (kcode == KeyCode.Menu)
                return;
            if (Input.GetKeyDown(kcode))
            {
                Debug.Log((int)kcode);
                mKeybindingData.ChangeBindingKeyCode(kcode);
                KeyboardBindText.text = mKeybindingData.BindingKeyCode.ToString();
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
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[6]))
            SetXboxButtonCodeToData(XBoxButton.BACK);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[7]))
            SetXboxButtonCodeToData(XBoxButton.START);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[8]))
            SetXboxButtonCodeToData(XBoxButton.L_THUMB_PRESSED);
        if (UnityEngine.Input.GetButtonDown(XBox360Constrol.XboxButtonNames[9]))
            SetXboxButtonCodeToData(XBoxButton.R_THUMB_PRESSED);
    }

    private void SetXboxButtonCodeToData(XBoxButton aButtonCode)
    {
        mKeybindingData.ChangeXBoxBotton(aButtonCode);
        GamepadBindButtonImage.sprite = XBoxButtonSprites[(int)mKeybindingData.BindingXBoxBotton];
        //Display();
        keybindingSettingHaveChange?.Invoke(KeybindCommand, mKeybindingData);
        ActiveChangeMode(false);
    }

    private void SetXboxAxisToData(AxisInput anAxis, Vector2Int aCommandDirection)
    {
        mKeybindingData.ChangeAxisCommand(anAxis, aCommandDirection);
    }
}
