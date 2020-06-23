﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindingSettings : SettingsContainerBase
{
    public delegate void OnReturnToSettingButtonContainer();
    public static OnReturnToSettingButtonContainer returnToSettingButtonContainer;

    private Dictionary<CommandIndex, KeybindData> mOriginalCommandsBindings = new Dictionary<CommandIndex, KeybindData>();
    private Dictionary<CommandIndex, KeybindData> mNewCommandsBindings = new Dictionary<CommandIndex, KeybindData>();

    private Dictionary<NavigatorType, KeybindData> mOriginalNavigateBindings = new Dictionary<NavigatorType, KeybindData>();
    private Dictionary<NavigatorType, KeybindData> mNewNavigateBindings = new Dictionary<NavigatorType, KeybindData>();

    private SetActiveControllSlot GetActiveControlSlot { get { return (SettingSlots[0] as SetActiveControllSlot);} }
    private SetKeybindConfirmSlot GetKeybindConfirmSlot { get { return (SettingSlots[mConfirmSlotIndex] as SetKeybindConfirmSlot); } }

    private ControlType mActiveControlType = ControlType.XBOX_360;
    private ControlType mCurrentDisplayType = ControlType.XBOX_360;

    private int mConfirmSlotIndex = -1;

    private void Awake()
    {
        SetActiveControllSlot.switchControlType += SwitchActiveControl;
        SetActiveControllSlot.displaySelectControlBinds += DisplaySelectedControlbinds;
        SetKeybindSlot.keybindingSettingHaveChange += ChangeKeybindingSetting;
        SetNavigatebindingSlot.navigatorSettingHaveChange += ChangeNavigatorBindSetting;
        SetKeybindConfirmSlot.applyButtonPressed += ApplySettings;
        SetKeybindConfirmSlot.resetButtonPressed += ResetSettings;
        SetKeybindConfirmSlot.backButtonPressed += BackButtonPressed;
    }

    protected override void Start()
    {
        base.Start();
        foreach (SettingSlotBase slot in SettingSlots)
        {
            if (slot.GetType() == typeof(SetKeybindSlot))
            {
                CommandIndex command = (slot as SetKeybindSlot).KeybindCommand;
                if (!mOriginalCommandsBindings.ContainsKey(command))
                {
                    mOriginalCommandsBindings.Add(command, new KeybindData(command));
                    mNewCommandsBindings.Add(command, new KeybindData(mOriginalCommandsBindings[command])/*mOriginalCommandsBindings[command]*/);
                }
            }
            if(slot.GetType() == typeof(SetNavigatebindingSlot))
            {
                NavigatorType naviType = (slot as SetNavigatebindingSlot).NavigatorType;
                if(!mOriginalNavigateBindings.ContainsKey(naviType))
                {
                    mOriginalNavigateBindings.Add(naviType, new KeybindData(naviType));
                    mNewNavigateBindings.Add(naviType, new KeybindData(mOriginalNavigateBindings[naviType])/*mOriginalNavigateBindings[naviType]*/);
                }
            }
            mConfirmSlotIndex++;
        }
        GetKeybindConfirmSlot.BackAndApplyButtonsSwap(SettingHasBeenChanged());

        //mActiveControlType = ControlManager.Ins.ActiveControlType;
        //GetActiveControlSlot.SetSelectedControlType(mActiveControlType);
        GetActiveControlSlot.SetupSelectiveToggles(ref mActiveControlType);
        mCurrentDisplayType = mActiveControlType;
        //DisplaySlots();
    }

    private void OnDestroy()
    {
        SetActiveControllSlot.switchControlType -= SwitchActiveControl;
        SetActiveControllSlot.displaySelectControlBinds -= DisplaySelectedControlbinds;
        SetKeybindSlot.keybindingSettingHaveChange -= ChangeKeybindingSetting;
        SetNavigatebindingSlot.navigatorSettingHaveChange -= ChangeNavigatorBindSetting;
        SetKeybindConfirmSlot.applyButtonPressed -= ApplySettings;
        SetKeybindConfirmSlot.resetButtonPressed -= ResetSettings;
        SetKeybindConfirmSlot.backButtonPressed -= BackButtonPressed;
    }

    protected override void Input()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_DOWN))
            SelectingSlot(1);
        else if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_UP))
            SelectingSlot(-1);

        //if (ControlManager.Ins.MenuBackButtonPressed() || ControlManager.Ins.MenuCancelButtonPressed())
        //    returnToSettingButtonContainer?.Invoke();

        DisplaySlots();
        base.Input();
    }

    public override void Enter()
    {
        mCurrentSelectingSlotIndex = 0;
        GetActiveControlSlot.SetSelectedControlType(mActiveControlType);
        DisplaySlots();
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SwitchActiveControl(ControlType aSelectedType)
    {
        mActiveControlType = aSelectedType;
    }

    private void DisplaySelectedControlbinds(ControlType aDisplayType)
    {
        mCurrentDisplayType = aDisplayType;
        DisplaySlots();
    }

    private void SelectingSlot(int anIncreame)
    {
        if ((mCurrentSelectingSlotIndex + anIncreame) >= mCurrentDisplaySlotsCount)
            mCurrentSelectingSlotIndex = 0;
        else if ((mCurrentSelectingSlotIndex + anIncreame) < 0)
            mCurrentSelectingSlotIndex = mCurrentDisplaySlotsCount - 1;
        else
            mCurrentSelectingSlotIndex += anIncreame;

        SwitchSelectingSlot(mCurrentSelectingSlotIndex);
    }

    private void ChangeKeybindingSetting(CommandIndex aCommand, KeybindData aKeybindData)
    {
        // put a function here to check if any of the commandindex already has the key
        // in binding
        // if it does. then call the function for swaping the commands with the keys by
        // swaping the old command recieve the bind from the changing command index,
        // and the changing command index get the new binding.
        if(!DuplicateKeyOccured(aCommand, aKeybindData))
            mNewCommandsBindings[aCommand] = new KeybindData(aKeybindData);
        DisplaySlots();
        ConfirmButtonDisplay();
    }

    private void ChangeNavigatorBindSetting(NavigatorType aNavigator, KeybindData aKeybindData)
    {
        if(!DuplicateKeyOccured(aNavigator, aKeybindData))
            mNewNavigateBindings[aNavigator] = new KeybindData(aKeybindData);
        DisplaySlots();
        ConfirmButtonDisplay();
    }

    protected override void ApplySettings()
    {
        base.ApplySettings();

        mOriginalCommandsBindings = new Dictionary<CommandIndex, KeybindData>(mNewCommandsBindings);
        mOriginalNavigateBindings = new Dictionary<NavigatorType, KeybindData>(mNewNavigateBindings);
        ConfirmButtonDisplay();
        ControlManager.Ins.NewBinding(mOriginalCommandsBindings, mOriginalNavigateBindings);
    }

    protected override void ResetSettings()
    {
        base.ResetSettings();

        mNewCommandsBindings = new Dictionary<CommandIndex, KeybindData>(mOriginalCommandsBindings);
        DisplaySlots();
        ConfirmButtonDisplay();
    }

    private void BackButtonPressed()
    {
        returnToSettingButtonContainer?.Invoke();
    }

    private void DisplaySlots()
    {
        foreach (SettingSlotBase slot in SettingSlots)
        {
            if (slot.GetType() == typeof(SetKeybindSlot))
                (slot as SetKeybindSlot).SetKey(mCurrentDisplayType, mNewCommandsBindings[(slot as SetKeybindSlot).KeybindCommand]);
            if (slot.GetType() == typeof(SetNavigatebindingSlot))
                (slot as SetNavigatebindingSlot).SetKey(mCurrentDisplayType, mNewNavigateBindings[(slot as SetNavigatebindingSlot).NavigatorType]);
        }
    }

    private void ConfirmButtonDisplay()
    {
        //bool equalSettings = SettingHasBeenChanged;
        GetKeybindConfirmSlot.BackAndApplyButtonsSwap(SettingHasBeenChanged());
    }

    private void SwitchSelectingSlot(int aNewSelectingSlotIndex)
    {
        mCurrentSelectedSlot.Exit();
        mCurrentSelectedSlot = SettingSlots[aNewSelectingSlotIndex];
        mCurrentSelectedSlot.Enter();
    }

    /// <summary>
    /// Checking both commando and navigate keybinddata had been changed between
    /// the original and new binding dictionaries.
    /// If anyone of the dictionaries is not equaled, mean that the player have
    /// made new unconfirmed settings
    /// </summary>
    /// <returns>Return if the both dictionaries has same data in every stored
    /// commando items</returns>
    private bool SettingHasBeenChanged()
    {
        foreach(CommandIndex com in mOriginalCommandsBindings.Keys)
        {
            if (!mOriginalCommandsBindings[com].Equals(mNewCommandsBindings[com]))
                return false;
        }
        foreach(NavigatorType navi in mOriginalNavigateBindings.Keys)
        {
            if (!mOriginalNavigateBindings[navi].Equals(mNewNavigateBindings[navi]))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Check if there's dublication keycode among the commands in the dictionary
    /// </summary>
    /// <param name="aCommand">The command index key of the searching changed data</param>
    /// <param name="aData">The changed data</param>
    /// <returns>Return true if any change had been made in of the requesting 
    /// command index</returns>
    private bool DuplicateKeyOccured(CommandIndex aCommand, KeybindData aData)
    {
        // look for each commandIndex keys in the commandBinding dictionary
        foreach(CommandIndex com in mNewCommandsBindings.Keys)
        {
            // if the current key is the same as the look
            if (com == aCommand)
                continue;

            if (mNewCommandsBindings[com].BindingKeyCode == aData.BindingKeyCode)
            {
                KeyCode tempKeycode = mNewCommandsBindings[aCommand].BindingKeyCode;
                mNewCommandsBindings[com].ChangeBindingKeyCode(tempKeycode);
                mNewCommandsBindings[aCommand].ChangeBindingKeyCode(aData.BindingKeyCode);

                return true;
            }
            else if (mNewCommandsBindings[com].BindingXBoxBotton == aData.BindingXBoxBotton)
            {
                XBoxButton tempKeycode = mNewCommandsBindings[aCommand].BindingXBoxBotton;
                mNewCommandsBindings[com].ChangeXBoxBotton(tempKeycode);
                mNewCommandsBindings[aCommand].ChangeXBoxBotton(aData.BindingXBoxBotton);

                return true;
            }
        }

        // look for each navi type in the navigatebindings dictionary
        foreach(NavigatorType navi in mNewNavigateBindings.Keys)
        {
            // check if the data's keycode list in the current key in the navigate binding dictionary
            // contain the binding keycode from the checking data
            if(mNewNavigateBindings[navi].BindingKeyCodes.Contains(aData.BindingKeyCode))
            {
                // get the index of the item that have the dublicate keycode
                int dublicateKeycodeIndex = mNewNavigateBindings[navi].BindingKeyCodes.IndexOf(aData.BindingKeyCode);
                // temporary store the keycode from the checking command data
                KeyCode tempKeycode = mNewCommandsBindings[aCommand].BindingKeyCode;
                // change the requesting command data keycode with the one store
                // in the dublicateKeycodeIndex item of the current checking data
                mNewNavigateBindings[navi].ChangeBindingCodesAt(dublicateKeycodeIndex, tempKeycode);
                mNewCommandsBindings[aCommand].ChangeBindingKeyCode(aData.BindingKeyCode);
                return true;
            }
        }
        return false;
    }

    private bool DuplicateKeyOccured(NavigatorType aNavigate, KeybindData aData)
    {
        // this foreach is to check for dublication axistype selection
        // and the keycode list of the navigate commando
        foreach (NavigatorType navi in mNewNavigateBindings.Keys)
        {
            if (navi == aNavigate)
                continue;
            if (mNewNavigateBindings[navi].BindingAxis == aData.BindingAxis)
            {
                AxisInput tempAxis = mNewNavigateBindings[aNavigate].BindingAxis;
                mNewNavigateBindings[navi].ChangeAxis(tempAxis);
                mNewNavigateBindings[aNavigate].ChangeAxis(aData.BindingAxis);
                return true;
            }

            int itemIndexFromDictionary = -1;
            int itemIndexFromKeydata = -1;
            if (CheckForDublicateKeycodeIn(navi, aData, ref itemIndexFromDictionary, ref itemIndexFromKeydata))
            {
                KeyCode dictionaryKeycode = mNewNavigateBindings[aNavigate].BindingKeyCodes[itemIndexFromDictionary];
                KeyCode keydataKeycode = aData.BindingKeyCodes[itemIndexFromKeydata];
                mNewNavigateBindings[navi].ChangeBindingCodesAt(itemIndexFromDictionary, dictionaryKeycode);
                mNewNavigateBindings[aNavigate].ChangeBindingCodesAt(itemIndexFromKeydata, keydataKeycode);
                return true;
            }
        }

        // this foreach is to check if any of the single keycode command have dublicate
        // with navi commando keycodes
        foreach (CommandIndex com in mNewCommandsBindings.Keys)
        {
            KeyCode keyCodeInThisComando = mNewCommandsBindings[com].BindingKeyCode;
            if(aData.BindingKeyCodes.Contains(keyCodeInThisComando))
            {
                int dublicateKeycodeIndex = aData.BindingKeyCodes.IndexOf(keyCodeInThisComando);
                KeyCode tempKeyCode = mNewNavigateBindings[aNavigate].BindingKeyCodes[dublicateKeycodeIndex];
                mNewCommandsBindings[com].ChangeBindingKeyCode(tempKeyCode);
                mNewNavigateBindings[aNavigate].ChangeBindingCodesAt(dublicateKeycodeIndex, keyCodeInThisComando);

                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check of any keycodes in the data has been dublicated with the checking
    /// navigatorType's keycodes
    /// </summary>
    /// <param name="aType">The checking navigateType key</param>
    /// <param name="aData">The checking daga</param>
    /// <param name="aDublicateIndex">the index of item with the dublicate keycode
    ///                                 in the dictionary of the seeking type</param>
    /// <param name="aDataIndex">the index of item with the dublicated keycode in 
    ///                         the data</param>
    /// <returns></returns>
    private bool CheckForDublicateKeycodeIn(NavigatorType aType, KeybindData aData, ref int aDublicateIndex, ref int aDataIndex)
    {
        for(int i = 0; i < aData.BindingKeyCodes.Count; i++)
        {
            if(mNewNavigateBindings[aType].BindingKeyCodes.Contains(aData.BindingKeyCodes[i]))
            {
                aDublicateIndex = mNewNavigateBindings[aType].BindingKeyCodes.IndexOf(aData.BindingKeyCodes[i]);
                aDataIndex = i;
                return true;
            }
        }
        return false;
    }
}

public class KeybindData
{
    private KeyCode mBindingKeyCode = KeyCode.None;
    public KeyCode BindingKeyCode { get { return mBindingKeyCode; } }
    public void ChangeBindingKeyCode(KeyCode aKeyCode) { mBindingKeyCode = aKeyCode; }

    private List<KeyCode> mBindingKeyCodes = new List<KeyCode>();
    public List<KeyCode> BindingKeyCodes { get { return mBindingKeyCodes; } }
    public void ChangeBindingKeyCodes(List<KeyCode> someKeyCodes)
    {
        mBindingKeyCodes.Clear();
        mBindingKeyCodes = new List<KeyCode>(someKeyCodes);
    }
    public void ChangeBindingCodesAt(int anIndex, KeyCode aKeyCode)
    {
        mBindingKeyCodes[anIndex] = aKeyCode;
    }

    private XBoxButton mBindingXBoxBotton = XBoxButton.NONE;
    public XBoxButton BindingXBoxBotton { get { return mBindingXBoxBotton; } }
    public void ChangeXBoxBotton(XBoxButton aBotton) { mBindingXBoxBotton = aBotton; }

    private AxisInput mBindingAxis = AxisInput.NONE;
    private Vector2Int mAxisCommandDirection = Vector2Int.zero;
    public AxisInput BindingAxis { get { return mBindingAxis; } }
    public Vector2Int AxisCommandDirection { get { return mAxisCommandDirection; } }
    public void ChangeAxis(AxisInput anAxis) { mBindingAxis = anAxis; }
    public void ChangeAxisCommand(AxisInput anAxis, Vector2Int aCommandDireciton) { mBindingAxis = anAxis; mAxisCommandDirection = aCommandDireciton; }

    /// <summary>
    /// Default constructor for an empty data object
    /// </summary>
    public KeybindData() { }

    public KeybindData(CommandIndex aCommand)
    {
        List<ControlObject> availableControls = ControlManager.Ins.Controls;
        foreach(ControlObject control in availableControls)
        {
            if(control.Type == ControlType.KEYBOARD)
                (control as KeyboardControl).GetKeyCodeFor(aCommand, ref mBindingKeyCode);
            if(control.Type == ControlType.XBOX_360)
                (control as XBox360Constrol).GetButtonFor(aCommand, ref mBindingXBoxBotton);
        }
    }

    public KeybindData(NavigatorType aNavigateType)
    {
        List<ControlObject> availableControls = ControlManager.Ins.Controls;
        foreach(ControlObject control in availableControls)
        {
            if (control.Type == ControlType.KEYBOARD)
                (control as KeyboardControl).GetNavigatorCodesFor(aNavigateType, ref mBindingKeyCodes);
            if (control.Type == ControlType.XBOX_360)
                (control as XBox360Constrol).GetNavigationSticksFor(aNavigateType, ref mBindingAxis);
        }
    }

    public KeybindData(KeyCode aBindingKeyCode, XBoxButton aBindingXBoxButton)
    {
        mBindingKeyCode = aBindingKeyCode;
        mBindingXBoxBotton = aBindingXBoxButton;
    }

    public KeybindData(List<KeyCode> someBindingCodes, AxisInput aBindingAxis)
    {
        mBindingKeyCodes = someBindingCodes;
        mBindingAxis = aBindingAxis;
    }

    public KeybindData(KeybindData data)
    {
        mBindingKeyCode = data.BindingKeyCode;
        mBindingXBoxBotton = data.BindingXBoxBotton;
        mBindingAxis = data.BindingAxis;
        for (int i = 0; i < data.BindingKeyCodes.Count; i++)
        {
            if (mBindingKeyCodes.Count < data.BindingKeyCodes.Count)
                mBindingKeyCodes.Add(data.BindingKeyCodes[i]);
            else
                mBindingKeyCodes[i] = data.BindingKeyCodes[i];
        }
    }

    public bool Equals(KeybindData obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            KeybindData p = (KeybindData)obj;
            if(mBindingKeyCode != p.BindingKeyCode)
                return false;
            if (mBindingXBoxBotton != p.mBindingXBoxBotton)
                return false;
            if (mBindingAxis != p.mBindingAxis)
                return false;
            if (mAxisCommandDirection != p.mAxisCommandDirection)
                return false;
            if (mBindingKeyCodes.Any() && p.BindingKeyCodes.Any())
            {
                for (int i = 0; i < mBindingKeyCodes.Count; i++)
                {
                    if (mBindingKeyCodes[i] != p.BindingKeyCodes[i])
                        return false;
                }
            }
        }
        return true;
    }
}
