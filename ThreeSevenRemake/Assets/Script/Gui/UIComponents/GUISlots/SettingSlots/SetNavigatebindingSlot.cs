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

    public List<KeyboardNaviBindBox> KeybordbindBoxes;
    public List<GamepadNaviBindBox> GamepadbindBoxes;
    
    public delegate void OnNavigatorSettingHaveChange(NavigatorType aType, KeybindData aNewKeybindingData);
    public static OnNavigatorSettingHaveChange navigatorSettingHaveChange;
    
    private int mCurrentInputIndex = -1;
    private int mCurrentSelectToggleIndex = 0;
    private AxisInput mSelectAxisInput = AxisInput.L_STICK;
    
    public override void Start()
    {
        base.Start();
        ActiveKeyboardBox();
    }

    protected override void Display()
    {
        DisplayBindingContain();

        for (int i = 0; i < KeybordbindBoxes.Count; i++)
            KeybordbindBoxes[i].SetBindingText(mKeybindingData.BindingKeyCodes[i].ToString());
        for(int i = 0; i < GamepadbindBoxes.Count; i++)
            GamepadbindBoxes[i].BindingTrigger(mKeybindingData.BindingAxis);
    }

    protected override void CheckForKeyboardInput()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (kcode == KeyCode.Menu)
                return;
            if (Input.GetKeyDown(kcode))
            {
                mKeybindingData.BindingKeyCodes[mCurrentInputIndex] = kcode;
                navigatorSettingHaveChange?.Invoke(NavigatorType, mKeybindingData);
                KeyboardbindBoxAppearence();
                Display();
                return;
            }
        }
    }

    protected override void CheckForGamepadInput()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
            SelectNavigateToggles(-1);
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
            SelectNavigateToggles(1);

        if (ControlManager.Ins.MenuSelectButtonPressed())
        {
            ActiveSelectedToggle();
            //ActiveChangeMode(false);
        }
        if (ControlManager.Ins.MenuCancelButtonPressed() || ControlManager.Ins.MenuBackButtonPressed())
            ActiveChangeMode(false);
    }

    protected override void ActiveChangeMode(bool isChangeModeOn)
    {
        base.ActiveChangeMode(isChangeModeOn);
        if (isChangeModeOn)
        {
            mCurrentInputIndex = 0;
            ActiveKeyboardBox();
        }
    }

    public override void SetKey(ControlType aDisplayControlType, KeybindData aData)
    {
        base.SetKey(aDisplayControlType, aData);

        for(int i = 0; i < GamepadbindBoxes.Count; i++)
        {
            if (GamepadbindBoxes[i].AxisType == mKeybindingData.BindingAxis)
                mCurrentSelectToggleIndex = i;
        }
        SetSelectedGamepadbindBox();
    }

    private void KeyboardbindBoxAppearence()
    {   
        mCurrentInputIndex++;
        if (mCurrentInputIndex == KeybordbindBoxes.Count)
            ActiveChangeMode(false);
        ActiveKeyboardBox();
    }

    private void ActiveKeyboardBox()
    {
        bool isSelected = false;
        for(int i = 0; i < KeybordbindBoxes.Count; i++)
        {
            isSelected = (i == mCurrentInputIndex);
            KeybordbindBoxes[i].BoxSelected(isSelected);
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
        if (mCurrentSelectToggleIndex + aDirection >= GamepadbindBoxes.Count)
            mCurrentSelectToggleIndex = 0;
        else if (mCurrentSelectToggleIndex + aDirection < 0)
            mCurrentSelectToggleIndex = GamepadbindBoxes.Count - 1;
        else
            mCurrentSelectToggleIndex += aDirection;

        SetSelectedGamepadbindBox();
    }

    /// <summary>
    /// Check on the bindingbox toggle to that has the same axistype as the selected axis
    /// and check off the rest
    /// </summary>
    private void ActiveSelectedToggle()
    {
        GamepadbindBoxes.ForEach(b => b.BindingTrigger(mSelectAxisInput));
        mKeybindingData.ChangeAxis(mSelectAxisInput);
        navigatorSettingHaveChange?.Invoke(NavigatorType, mKeybindingData);
    }

    private void SetSelectedGamepadbindBox()
    {
        for (int i = 0; i < GamepadbindBoxes.Count; i++)
            GamepadbindBoxes[i].BoxSelected(ref mSelectAxisInput, (i == mCurrentSelectToggleIndex) ? true : false);
    }
}
