using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetNavigatebindingSlot : SettingSlotBase
{
    public NavigatorType NavigatorType;

    public GameObject KeybindContainer;

    public List<KeyboardNaviBindBox> KeybordbindBoxes;
    public List<GamepadNaviBindBox> GamepadbindBoxes;
    
    public delegate void OnNavigatorSettingHaveChange(NavigatorType aType, KeybindData aNewKeybindingData);
    public static OnNavigatorSettingHaveChange navigatorSettingHaveChange;

    private CanvasGroup mBindContainerMG;

    private KeybindData mKeybindingData = new KeybindData();
    private bool mChangeBindingEnable = false;

    //private List<KeyCode> mNaviKeycodes = new List<KeyCode>();
    private int mCurrentInputIndex = -1;

    //private AxisInput mCurrentSelectAxis = AxisInput.L_STICK;

    public override void Start()
    {
        base.Start();
        mBindContainerMG = KeybindContainer.GetComponent<CanvasGroup>();
    }

    public override void Update()
    {
        if(mChangeBindingEnable)
        {
            CheckForKeyboardInput();
            return;
        }
    }

    public override void Enter()
    {
        base.Enter();
        mChangeBindingEnable = true;
        mCurrentInputIndex = 0;
        ActiveKeyboardBox();
    }

    public override void Exit()
    {
        base.Exit();
        mChangeBindingEnable = false;
        mCurrentInputIndex = -1;
        ActiveKeyboardBox();

    }

    public void SetKey(KeybindData aData)
    {
        mKeybindingData = new KeybindData(aData);

        //mNaviKeycodes.Clear();
        //mNaviKeycodes = new List<KeyCode>(mKeybindingData.BindingKeyCodes);

        //mCurrentSelectAxis = mKeybindingData.BindingAxis;

        Display();
    }

    private void Display()
    {
        for(int i = 0; i < KeybordbindBoxes.Count; i++)
            KeybordbindBoxes[i].SetBindingText(mKeybindingData.BindingKeyCodes[i].ToString());
        for(int i = 0; i < GamepadbindBoxes.Count; i++)
            GamepadbindBoxes[i].BindingTrigger(mKeybindingData.BindingAxis);
    }

    private void CheckForKeyboardInput()
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
                //mKeybindingData.ChangeBindingKeyCode(kcode);
                Display();
                return;
            }
        }
    }

    private void ChangeNaviSticks()
    {
        //if(ControlManager.Ins.)
    }

    private void KeyboardbindBoxAppearence()
    {   
        mCurrentInputIndex++;
        if (mCurrentInputIndex == KeybordbindBoxes.Count)
            mCurrentInputIndex = 0;
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
}
