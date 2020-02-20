using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindingSettings : SettingsContainerBase
{
    public delegate void OnReturnToSettingButtonContainer();
    public static OnReturnToSettingButtonContainer returnToSettingButtonContainer;

    private Dictionary<CommandIndex, KeybindData> mOriginalBindings = new Dictionary<CommandIndex, KeybindData>();
    private Dictionary<CommandIndex, KeybindData> mNewBindings = new Dictionary<CommandIndex, KeybindData>();

    private SetKeybindConfirmSlot GetKeybindConfirmSlot { get { return (SettingSlots[mConfirmSlotIndex] as SetKeybindConfirmSlot); } }

    private bool SettingHasBeenChanged { get { return KeyBindingHasChanged(); } }

    private int mConfirmSlotIndex = -1;

    private void Awake()
    {
        SetKeybindSlot.keybindingSettingHaveChange += ChangeKeybindingSetting;
        SetKeybindConfirmSlot.applyButtonPressed += ApplySettings;
        SetKeybindConfirmSlot.resetButtonPressed += ResetSettings;
        SetKeybindConfirmSlot.backButtonPressed += BackButtonPressed;
    }

    protected override void Start()
    {
        base.Start();
        foreach(SettingSlotBase slot in SettingSlots)
        {
            if (slot.GetType() == typeof(SetKeybindSlot))
            {
                CommandIndex command = (slot as SetKeybindSlot).KeybindCommand;
                if (!mOriginalBindings.ContainsKey(command))
                {
                    mOriginalBindings.Add(command, new KeybindData(command));
                    mNewBindings.Add(command, mOriginalBindings[command]);
                }
            }
            mConfirmSlotIndex++;
        }
        //SettingSlots[mConfirmSlotIndex].gameObject.SetActive(!SettingHasBeenChanged);
        GetKeybindConfirmSlot.BackAndApplyButtonsSwap(SettingHasBeenChanged);
        //CheckNumberOfActiveSlots();
        return;
    }

    private void OnDestroy()
    {
        SetKeybindSlot.keybindingSettingHaveChange -= ChangeKeybindingSetting;
        SetKeybindConfirmSlot.applyButtonPressed -= ApplySettings;
        SetKeybindConfirmSlot.resetButtonPressed -= ResetSettings;
        SetKeybindConfirmSlot.backButtonPressed -= BackButtonPressed;
    }

    protected override void Update()
    {
        base.Update();
        
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
        DisplaySlots();
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
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
            mNewBindings[aCommand] = new KeybindData(aKeybindData);

        ConfirmButtonDisplay();
    }

    protected override void ApplySettings()
    {
        base.ApplySettings();

        mOriginalBindings = new Dictionary<CommandIndex, KeybindData>(mNewBindings);
        ConfirmButtonDisplay();
    }

    protected override void ResetSettings()
    {
        base.ResetSettings();

        mNewBindings = new Dictionary<CommandIndex, KeybindData>(mOriginalBindings);
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
                (slot as SetKeybindSlot).SetKey(mNewBindings[(slot as SetKeybindSlot).KeybindCommand]);
        }
    }

    private void ConfirmButtonDisplay()
    {
        bool equalSettings = SettingHasBeenChanged;
        GetKeybindConfirmSlot.BackAndApplyButtonsSwap(SettingHasBeenChanged);
    }

    private void SwitchSelectingSlot(int aNewSelectingSlotIndex)
    {
        mCurrentSelectedSlot.Exit();
        mCurrentSelectedSlot = SettingSlots[aNewSelectingSlotIndex];
        mCurrentSelectedSlot.Enter();
    }

    private bool KeyBindingHasChanged()
    {
        foreach(CommandIndex com in mOriginalBindings.Keys)
        {
            if (!mOriginalBindings[com].Equals(mNewBindings[com]))
                return false;
        }
        return true;
    }

    private bool DuplicateKeyOccured(CommandIndex aCommand, KeybindData aData)
    {
        foreach(CommandIndex com in mNewBindings.Keys)
        {
            if (com == aCommand)
                continue;

            if (mNewBindings[com].BindingKeyCode == aData.BindingKeyCode)
            {
                KeyCode tempKeycode = mNewBindings[aCommand].BindingKeyCode;
                mNewBindings[com].ChangeBindingKeyCode(tempKeycode);
                mNewBindings[aCommand].ChangeBindingKeyCode(aData.BindingKeyCode);

                return true;
            }
            else if (mNewBindings[com].BindingXBoxBotton == aData.BindingXBoxBotton)
            {
                XBoxButton tempKeycode = mNewBindings[aCommand].BindingXBoxBotton;
                mNewBindings[com].ChangeXBoxBotton(tempKeycode);
                mNewBindings[aCommand].ChangeXBoxBotton(aData.BindingXBoxBotton);

                return true;
            }
            else if (aData.BindingAxis != AxisInput.NONE && mNewBindings[com].BindingAxis == aData.BindingAxis)
            {
                return true;
            }
            //else if(mNewBindings[com].BindingKeyCodes.Any() && aData.BindingKeyCodes.Any())
            //{
            //    var temp = mNewBindings[com].BindingKeyCodes.FirstOrDefault(o => o == aData)
            //}
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
    public void ChangeBindingKeyCodes(List<KeyCode> someKeyCodes){ mBindingKeyCodes = new List<KeyCode>(someKeyCodes); }

    private XBoxButton mBindingXBoxBotton = XBoxButton.NONE;
    public XBoxButton BindingXBoxBotton { get { return mBindingXBoxBotton; } }
    public void ChangeXBoxBotton(XBoxButton aBotton) { mBindingXBoxBotton = aBotton; }

    private AxisInput mBindingAxis = AxisInput.NONE;
    public AxisInput BindingAxis { get { return mBindingAxis; } }
    public void ChangeAxis(AxisInput anAxis) { mBindingAxis = anAxis; }

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
        for(int i = 0; i < data.BindingKeyCodes.Count; i++)
            mBindingKeyCodes[i] = data.BindingKeyCodes[i];
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
