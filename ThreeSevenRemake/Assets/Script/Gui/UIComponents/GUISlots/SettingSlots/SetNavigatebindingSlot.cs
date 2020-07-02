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

    //public List<CommandIndex> CommandIndexes;
    public List<NaviInputBindBox> NaviInputBindBoxes;

    public delegate void OnKeybindingSettingHaveChange(CommandIndex aCommand, KeybindData aNewKeybindingData);
    public static OnKeybindingSettingHaveChange keybindingSettingHaveChange;

    public delegate void OnNavigatorSettingHaveChange(NavigatorType aType, KeybindData aNewKeybindingData);
    public static OnNavigatorSettingHaveChange navigatorSettingHaveChange;
    
    private int mCurrentInputIndex = -1;
    private int mCurrentSelectToggleIndex = 0;
    private AxisInput mSelectAxisInput = AxisInput.L_STICK;

    private Dictionary<CommandIndex, Sprite> mDisplayingSprites = new Dictionary<CommandIndex, Sprite>()
    {

    };

    private bool mSlotIsActive = false;

    public override void Awake()
    {
        base.Awake();

    }

    public override void Start()
    {
        base.Start();
        ActiveKeyboardBox();
    }
    
    protected override void Display()
    {
        DisplayBindingContain();

        //for (int i = 0; i < NaviInputBindBoxes.Count; i++)
        //{
        //    //NaviInputBindBoxes[i].SetBindingText(mKeybindingData.BindingKeyCodes[i].ToString());
        //    if (mDisplayControlType == ControlType.KEYBOARD)
        //        NaviInputBindBoxes[i].SetBindingSprite(InputSpritesManager.Instance.GetKeyboard(mKeybindingData.BindingKeyCodes[i]));
        //    else if(mDisplayControlType == ControlType.XBOX_360)
        //        NaviInputBindBoxes[i].SetBindingSprite(InputSpritesManager.Instance.GetXboxButton(mKeybindingData.BindingXboxButtons[i]));
        //}
        //for(int i = 0; i < GamepadbindBoxes.Count; i++)
        //    GamepadbindBoxes[i].BindingTrigger(mKeybindingData.BindingAxis);

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
                //mKeybindingData.BindingKeyCodes[mCurrentInputIndex] = kcode;
                //navigatorSettingHaveChange?.Invoke(NavigatorType, mKeybindingData);
                mKeybindingDatas[command].ChangeBindingKeyCode(kcode);

                KeyboardbindBoxAppearence();
                keybindingSettingHaveChange?.Invoke(command, mKeybindingDatas[command]);
                Display();
                return;
            }
        }
    }

    protected override void CheckForGamepadInput()
    {
        //if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
        //    SelectNavigateToggles(-1);
        //if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
        //    SelectNavigateToggles(1);

        //if (ControlManager.Ins.MenuSelectButtonPressed())
        //{
        //    //ActiveSelectedToggle();
        //    //ActiveChangeMode(false);
        //}
        //if (ControlManager.Ins.MenuCancelButtonPressed() || ControlManager.Ins.MenuBackButtonPressed())
        //    ActiveChangeMode(false);

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

    /// <summary>
    /// Set active to the display binding container and deactive the rest
    /// </summary>
    private void DisplayBindingContain()
    {
        for(int i = 0; i < BinderContainers.Count; i++)
        {
            BinderContainers[i].SetActive(false);
            if(i == (int)mDisplayControlType)
                BinderContainers[i].SetActive(true);
        }
    }

    /// <summary>
    /// Navigate the selection of the gamepadbindBoxes
    /// </summary>
    /// <param name="aDirection">The direction to where the selecting box moves</param>
    private void SelectNavigateToggles(int aDirection)
    {
        //if (mCurrentSelectToggleIndex + aDirection >= GamepadbindBoxes.Count)
        //    mCurrentSelectToggleIndex = 0;
        //else if (mCurrentSelectToggleIndex + aDirection < 0)
        //    mCurrentSelectToggleIndex = GamepadbindBoxes.Count - 1;
        //else
        //    mCurrentSelectToggleIndex += aDirection;

        //SetSelectedGamepadbindBox();
    }

    /// <summary>
    /// Check on the bindingbox toggle to that has the same axistype as the selected axis
    /// and check off the rest
    /// </summary>
    //private void ActiveSelectedToggle()
    //{
    //    GamepadbindBoxes.ForEach(b => b.BindingTrigger(mSelectAxisInput));
    //    mKeybindingData.ChangeAxis(mSelectAxisInput);
    //    navigatorSettingHaveChange?.Invoke(NavigatorType, mKeybindingData);
    //}

    //private void SetSelectedGamepadbindBox()
    //{
    //    for (int i = 0; i < GamepadbindBoxes.Count; i++)
    //        GamepadbindBoxes[i].BoxSelected(ref mSelectAxisInput, (i == mCurrentSelectToggleIndex) ? true : false);
    //}
}
