using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetNavigatebindingSlot : SettingSlotForCommandBinding//SettingSlotBase
{
    public NavigatorType NavigatorType;

    public List<GameObject> BinderContainers;

    public List<NaviInputBindBox> NaviInputBindBoxes;

    public delegate void OnKeybindingSettingHaveChange(CommandIndex aCommand, KeybindData aNewKeybindingData);
    public static OnKeybindingSettingHaveChange keybindingSettingHaveChange;

    public delegate void OnNavigatorSettingHaveChange(NavigatorType aType, KeybindData aNewKeybindingData);
    public static OnNavigatorSettingHaveChange navigatorSettingHaveChange;
    
    private int mCurrentInputIndex = -1;

    private Dictionary<CommandIndex, Sprite> mDisplayingSprites = new Dictionary<CommandIndex, Sprite>();

    private bool mSlotIsActive = false;

    public override void Awake()
    {
        base.Awake();
        if (mDisplayingSprites.Count == 0)
        {
            for (int i = 0; i < CommandIndexes.Count; i++)
            {
                mDisplayingSprites.Add(CommandIndexes[i], InputSpritesManager.Instance.GetKeyboard(KeyCode.Return));
            }
        }
    }

    public override void Start()
    {
        base.Start();
        ActiveKeyboardBox();
    }
    
    protected override void Display()
    {
        for(int i = 0; i < CommandIndexes.Count; i++)
        {
            if (mDisplayControlType == ControlType.KEYBOARD)
                NaviInputBindBoxes[i].SetBindingSprite(InputSpritesManager.Instance.GetKeyboard(mKeybindingDatas[CommandIndexes[i]].BindingKeyCode));
            else if(mDisplayControlType == ControlType.XBOX_360)
                NaviInputBindBoxes[i].SetBindingSprite(InputSpritesManager.Instance.GetXboxButton(mKeybindingDatas[CommandIndexes[i]].BindingXBoxButton));
        }
    }

    protected override void CheckForKeyboardInput()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (kcode == KeyCode.Menu)
                return;
            if (Input.GetKeyDown(kcode))
            {
                CommandIndex command = CommandIndexes[mCurrentInputIndex];
                mKeybindingDatas[command].ChangeBindingKeyCode(kcode);

                KeyboardbindBoxAppearence();
                keybindingSettingHaveChange?.Invoke(command, mKeybindingDatas[command]);
                Display();
                return;
            }
        }
    }

    protected override void ActiveChangeMode(bool isChangeModeOn)
    {
        base.ActiveChangeMode(isChangeModeOn);
        mSlotIsActive = isChangeModeOn;
        if (mSlotIsActive)
        {
            mCurrentInputIndex = 0;
            ActiveKeyboardBox();
        }
    }

    protected override void SetXboxButtonCodeToData(XBoxButton aButtonCode)
    {
        //base.SetXboxButtonCodeToData(aButtonCode);

        CommandIndex command = CommandIndexes[mCurrentInputIndex];
        mKeybindingDatas[command].ChangeXBoxBotton(aButtonCode);

        mDisplayingSprites[command] = InputSpritesManager.Instance.GetXboxButton(aButtonCode);
        keybindingSettingHaveChange?.Invoke(command, mKeybindingDatas[command]);
        KeyboardbindBoxAppearence();
        Display();
    }

    public override void SetKey(ControlType aDisplayControlType, KeybindData aData)
    {
        
        base.SetKey(aDisplayControlType, aData);

        int index = CommandIndexes.IndexOf(aData.Command);

        if(mDisplayControlType == ControlType.KEYBOARD)
            mDisplayingSprites[aData.Command] = InputSpritesManager.Instance.GetKeyboard(mKeybindingDatas[aData.Command].BindingKeyCode);
        else if(mDisplayControlType == ControlType.XBOX_360)
            mDisplayingSprites[aData.Command] = InputSpritesManager.Instance.GetXboxButton(mKeybindingDatas[aData.Command].BindingXBoxButton);

        Display();
    }

    private void KeyboardbindBoxAppearence()
    {   
        mCurrentInputIndex++;
        if (mCurrentInputIndex == NaviInputBindBoxes.Count)
            ActiveChangeMode(false);
        ActiveKeyboardBox();
    }

    private void ActiveKeyboardBox()
    {
        bool isSelected = false;
        for(int i = 0; i < NaviInputBindBoxes.Count; i++)
        {
            isSelected = (i == mCurrentInputIndex);
            NaviInputBindBoxes[i].BoxSelected(isSelected);
        }
    }
}
