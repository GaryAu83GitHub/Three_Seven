using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingSlotForCommandBinding : SettingSlotBase
{
    public List<CommandIndex> CommandIndexes;

    public GameObject KeybindContainer;

    public delegate void OnSetDisplayControlType(ControlType aDisplayingType);
    public static OnSetDisplayControlType setDisplayControlType;

    private CanvasGroup mBindContainerMG;

    protected ControlType mDisplayControlType = ControlType.XBOX_360;
    protected KeybindData mKeybindingData = new KeybindData();
    protected Dictionary<CommandIndex, KeybindData> mKeybindingDatas = new Dictionary<CommandIndex, KeybindData>();

    private enum AxisName
    {
        L_STICK_HORI,
        L_STICK_VERT,
        TRIGGERS,
        R_STICK_HORI,
        R_STICK_VERT,
        D_PAD_HORI,
        D_PAD_VERT,
        NONE
    }

    private bool mChangeModeOn = false;

    private float mChangeModeSuspendCountdown = 0f;

    private int mXboxStickValue = 0;
    private AxisName mLastMovedAxis = AxisName.NONE;

    private const float mChangeModeSuspendTime = .05f;

    public override void Awake()
    {
        base.Awake();

        foreach (CommandIndex com in CommandIndexes)
        {
            if (!mKeybindingDatas.ContainsKey(com))
                mKeybindingDatas.Add(com, new KeybindData());
        }
    }

    public override void Start()
    {
        base.Start();
        if (KeybindContainer != null)
            mBindContainerMG = KeybindContainer.GetComponent<CanvasGroup>();
        ActiveChangeMode(false);
    }

    public override void Update()
    {
        base.Update();

        if (mChangeModeSuspendCountdown > 0f)
            mChangeModeSuspendCountdown -= Time.deltaTime;

        if(mChangeModeOn && mChangeModeSuspendCountdown <= 0f)
        {
            if(mDisplayControlType == ControlType.KEYBOARD)
                CheckForKeyboardInput();
            else if(mDisplayControlType == ControlType.XBOX_360)
                CheckForGamepadInput();
        }
    }

    public override void Enter()
    {
        base.Enter();
        ActiveChangeMode(false);
    }

    public override void Exit()
    {
        base.Exit();
        ActiveChangeMode(false);
    }

    protected override void MenuButtonPressed()
    {
        if (ControlManager.Ins.MenuSelectButtonPressed() && !mChangeModeOn)
            ActiveChangeMode(true);
    }

    protected virtual void CheckForKeyboardInput()
    { }

    protected virtual void CheckForGamepadInput()
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

       
        foreach (AxisName axis in Enum.GetValues(typeof(AxisName)))
        {
            if (axis == AxisName.NONE)
                continue;

            mXboxStickValue = (int)UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[(int)axis]);

            if (mXboxStickValue != 0 && mLastMovedAxis == AxisName.NONE)
            {
                mLastMovedAxis = axis;
                CheckForAnalogueInput(axis, mXboxStickValue);
                break;
            }
            else if(mXboxStickValue == 0 && mLastMovedAxis == axis)
            {
                mLastMovedAxis = AxisName.NONE;
                break;
            }
        }
    }

    protected virtual void Display()
    { }

    protected virtual void SetXboxButtonCodeToData(XBoxButton aButtonCode)
    { }

    protected virtual void ActiveChangeMode(bool isChangeModeOn)
    {
        if (KeybindContainer == null)
            return;

        mChangeModeOn = isChangeModeOn;
        mLockParentInput = mChangeModeOn;

        mChangeModeSuspendCountdown = (isChangeModeOn) ? mChangeModeSuspendTime : 0f;
        mBindContainerMG.alpha = (mChangeModeOn ? 1f : .5f);
    }

    public virtual void SetKey(ControlType aDisplayControlType, KeybindData aData)
    {
        mDisplayControlType = aDisplayControlType;
        if(CommandIndexes.Count == 1)
            mKeybindingData = new KeybindData(aData);
        else
        {
            if (CommandIndexes.Contains(aData.Command))
                mKeybindingDatas[aData.Command] = new KeybindData(aData);
        }
        
        //Display();
    }

    private void CheckForAnalogueInput(AxisName anAxisName, int anAxisDirection)
    {
        XBoxButton button = XBoxButton.NONE;

        if(anAxisName == AxisName.L_STICK_HORI)
        {
            if(anAxisDirection == 1)
                button = XBoxButton.L_Thumb_Right;
            else if(anAxisDirection == -1)
                button = XBoxButton.L_Thumb_Left;
        }
        if (anAxisName == AxisName.L_STICK_VERT)
        {
            if (anAxisDirection == 1)
                button = XBoxButton.L_Thumb_Up;
            else if (anAxisDirection == -1)
                button = XBoxButton.L_Thumb_Down;
        }
        if (anAxisName == AxisName.TRIGGERS)
        {
            if (anAxisDirection == 1)
                button = XBoxButton.R_Trigger;
            else if (anAxisDirection == -1)
                button = XBoxButton.L_Trigger;
        }
        if (anAxisName == AxisName.R_STICK_HORI)
        {
            if (anAxisDirection == 1)
                button = XBoxButton.R_Thumb_Right;
            else if (anAxisDirection == -1)
                button = XBoxButton.R_Thumb_Left;
        }
        if (anAxisName == AxisName.R_STICK_VERT)
        {
            if (anAxisDirection == 1)
                button = XBoxButton.R_Thumb_Up;
            else if (anAxisDirection == -1)
                button = XBoxButton.R_Thumb_Down;
        }
        if (anAxisName == AxisName.D_PAD_HORI)
        {
            if (anAxisDirection == 1)
                button = XBoxButton.DPad_Right;
            else if (anAxisDirection == -1)
                button = XBoxButton.DPad_Left;
        }
        if (anAxisName == AxisName.D_PAD_VERT)
        {
            if (anAxisDirection == 1)
                button = XBoxButton.DPad_Up;
            else if (anAxisDirection == -1)
                button = XBoxButton.DPad_Down;
        }
        SetXboxButtonCodeToData(button);
    }
}
