using System.Collections;
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
        SetNavigatebindingSlot.keybindingSettingHaveChange += ChangeKeybindingSetting;
        SetKeybindConfirmSlot.applyButtonPressed += ApplySettings;
        SetKeybindConfirmSlot.resetButtonPressed += ResetSettings;
        SetKeybindConfirmSlot.backButtonPressed += BackButtonPressed;
    }

    protected override void Start()
    {
        base.Start();

        foreach (SettingSlotBase slot in SettingSlots)
        {
            SettingSlotForCommandBinding tempSlot = (slot as SettingSlotForCommandBinding);
            if (tempSlot == null)
            {
                mConfirmSlotIndex++;
                continue;
            }

            for (int i = 0; i < tempSlot.CommandIndexes.Count; i++)
            {
                CommandIndex command = tempSlot.CommandIndexes[i];
                if (!mOriginalCommandsBindings.ContainsKey(command))
                {
                    mOriginalCommandsBindings.Add(command, new KeybindData(command));
                    mNewCommandsBindings.Add(command, new KeybindData(mOriginalCommandsBindings[command])/*mOriginalCommandsBindings[command]*/);
                }
            }
            mConfirmSlotIndex++;
        }
        GetKeybindConfirmSlot.BackAndApplyButtonsSwap(SettingHasBeenChanged());

        GetActiveControlSlot.SetupSelectiveToggles(ref mActiveControlType);
        mCurrentDisplayType = mActiveControlType;
    }

    private void OnDestroy()
    {
        SetActiveControllSlot.switchControlType -= SwitchActiveControl;
        SetActiveControllSlot.displaySelectControlBinds -= DisplaySelectedControlbinds;
        SetKeybindSlot.keybindingSettingHaveChange -= ChangeKeybindingSetting;
        SetNavigatebindingSlot.keybindingSettingHaveChange -= ChangeKeybindingSetting;
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
        //foreach (SettingSlotBase slot in SettingSlots)
        //{
        //    if (slot.GetType() == typeof(SetKeybindSlot))
        //        (slot as SetKeybindSlot).SetKey(mCurrentDisplayType, mNewCommandsBindings[(slot as SetKeybindSlot).CommandIndexes[0]]);
        //    if (slot.GetType() == typeof(SetNavigatebindingSlot))
        //        (slot as SetNavigatebindingSlot).SetKey(mCurrentDisplayType, mNewNavigateBindings[(slot as SetNavigatebindingSlot).NavigatorType]);
        //}

        foreach(CommandIndex com in mNewCommandsBindings.Keys)
        {
            for (int i = 0; i < SettingSlots.Count; i++)
            {
                if (SettingSlots[i].GetType() == typeof(SetKeybindSlot) || SettingSlots[i].GetType() == typeof(SetNavigatebindingSlot))
                {
                    SettingSlotForCommandBinding tempSlot = (SettingSlots[i] as SettingSlotForCommandBinding);
                    if (tempSlot.CommandIndexes.Contains(com))
                    {
                        tempSlot.SetKey(mCurrentDisplayType, mNewCommandsBindings[com]);
                        break;
                    }
                }
            }
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

            if (mActiveControlType == ControlType.KEYBOARD && mNewCommandsBindings[com].BindingKeyCode == aData.BindingKeyCode)
            {
                KeyCode tempKeycode = mNewCommandsBindings[aCommand].BindingKeyCode;
                mNewCommandsBindings[com].ChangeBindingKeyCode(tempKeycode);
                mNewCommandsBindings[aCommand].ChangeBindingKeyCode(aData.BindingKeyCode);

                return true;
            }
            else if (mActiveControlType == ControlType.XBOX_360 && mNewCommandsBindings[com].BindingXBoxButton == aData.BindingXBoxButton)
            {
                XBoxButton tempKeycode = mNewCommandsBindings[aCommand].BindingXBoxButton;
                mNewCommandsBindings[com].ChangeXBoxButton(tempKeycode);
                mNewCommandsBindings[aCommand].ChangeXBoxButton(aData.BindingXBoxButton);

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
            if (mActiveControlType == ControlType.XBOX_360 && mNewNavigateBindings[navi].BindingAxis == aData.BindingAxis)
            {
                AnalogueSticks tempAxis = mNewNavigateBindings[aNavigate].BindingAxis;
                mNewNavigateBindings[navi].ChangeAxis(tempAxis);
                mNewNavigateBindings[aNavigate].ChangeAxis(aData.BindingAxis);
                return true;
            }

            int itemIndexFromDictionary = -1;
            int itemIndexFromKeydata = -1;
            if (mActiveControlType == ControlType.KEYBOARD && CheckForDublicateKeycodeIn(navi, aData, ref itemIndexFromDictionary, ref itemIndexFromKeydata))
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
    private CommandIndex mCommand = CommandIndex.MAX_INPUT;
    public CommandIndex Command { get { return mCommand; } }

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

    private GP_InputType mGamePadInputType = GP_InputType.NONE;
    public GP_InputType GamePadInputType { get { return mGamePadInputType; } }

    private XBoxButton mBindingXBoxButton = XBoxButton.NONE;
    public XBoxButton BindingXBoxButton { get { return mBindingXBoxButton; } }
    public void ChangeXBoxButton(XBoxButton aBotton)
    {
        mGamePadInputType = GP_InputType.BUTTON;
        mBindingXBoxButton = aBotton;
    }
    public void ChangeXBoxAnaloge(XBoxButton aButton)
    {
        mGamePadInputType = GP_InputType.AXIS;
        mBindingXBoxButton = aButton;
    }

    private AnalogueSticks mBindingAxis = AnalogueSticks.NONE;
    private Vector2Int mAxisCommandDirection = Vector2Int.zero;
    public AnalogueSticks BindingAxis { get { return mBindingAxis; } }
    public Vector2Int AxisCommandDirection { get { return mAxisCommandDirection; } }
    public void ChangeAxis(AnalogueSticks anAxis) { mBindingAxis = anAxis; }
    public void ChangeAxisCommand(AnalogueSticks anAxis, Vector2Int aCommandDireciton) { mBindingAxis = anAxis; mAxisCommandDirection = aCommandDireciton; }

    /// <summary>
    /// Default constructor for an empty data object
    /// </summary>
    public KeybindData() { }

    public KeybindData(CommandIndex aCommand)
    {
        mCommand = aCommand;
        List<ControlObject> availableControls = ControlManager.Ins.Controls;
        foreach(ControlObject control in availableControls)
        {
            KeybindData data = this;
            control.GetInputFor(mCommand, ref data);
            //if(control.Type == ControlType.KEYBOARD)
            //    (control as KeyboardControl).GetKeyCodeFor(mCommand, ref mBindingKeyCode);
            //if(control.Type == ControlType.XBOX_360)
            //    (control as XBoxControl/*XBox360Constrol*/).GetButtonFor(mCommand, ref mBindingXBoxButton);
            CopyData(data);
        }
    }

    private void CopyData(KeybindData aData)
    {
        mCommand = aData.Command;
        mBindingKeyCode = aData.BindingKeyCode;
        mBindingKeyCodes = aData.BindingKeyCodes;
        mBindingXBoxButton = aData.BindingXBoxButton;
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
        mBindingXBoxButton = aBindingXBoxButton;
    }

    public KeybindData(List<KeyCode> someBindingCodes, AnalogueSticks aBindingAxis)
    {
        mBindingKeyCodes = someBindingCodes;
        mBindingAxis = aBindingAxis;
    }

    public KeybindData(KeybindData data)
    {
        mCommand = data.Command;

        mBindingKeyCode = data.BindingKeyCode;
        mBindingXBoxButton = data.BindingXBoxButton;
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
            if (mCommand != p.Command)
                return false;
            if (mBindingKeyCode != p.BindingKeyCode)
                return false;
            if (mBindingXBoxButton != p.mBindingXBoxButton)
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
