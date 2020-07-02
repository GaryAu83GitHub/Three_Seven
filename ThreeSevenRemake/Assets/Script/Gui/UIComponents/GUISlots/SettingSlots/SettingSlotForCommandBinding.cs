using System.Collections;
using System.Collections.Generic;
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

    private bool mChangeModeOn = false;

    private float mChangeModeSuspendCountdown = 0f;

    private int mXboxStickValue = 0;
    private int mXboxAxisIndexCount = XBox360Constrol.XboxAxisNames.Count;
    private int mLastMovedXboxStickIndex = 0;
    private bool mAStickHasMoved = false;

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

        mLastMovedXboxStickIndex = mXboxAxisIndexCount;
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

        if (CheckXBoxAnalogeIsNeutural(0) && mXboxStickValue == -1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Left);
        if (CheckXBoxAnalogeIsNeutural(0) && mXboxStickValue == 1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Right);
        if (CheckXBoxAnalogeIsNeutural(1) && mXboxStickValue == 1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Down);
        if (CheckXBoxAnalogeIsNeutural(1) && mXboxStickValue == -1)
            SetXboxButtonCodeToData(XBoxButton.L_Thumb_Up);
        if (CheckXBoxAnalogeIsNeutural(2) && mXboxStickValue == -1)
            SetXboxButtonCodeToData(XBoxButton.L_Trigger);
        if (CheckXBoxAnalogeIsNeutural(2) && mXboxStickValue == 1)
            SetXboxButtonCodeToData(XBoxButton.R_Trigger);
        if (CheckXBoxAnalogeIsNeutural(3) && mXboxStickValue == -1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Left);
        if (CheckXBoxAnalogeIsNeutural(3) && mXboxStickValue == 1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Right);
        if (CheckXBoxAnalogeIsNeutural(4) && mXboxStickValue == -1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Down);
        if (CheckXBoxAnalogeIsNeutural(4) && mXboxStickValue == -1)
            SetXboxButtonCodeToData(XBoxButton.R_Thumb_Up);
        if (CheckXBoxAnalogeIsNeutural(5) && mXboxStickValue == 1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Left);
        if (CheckXBoxAnalogeIsNeutural(5) && mXboxStickValue == 1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Right);
        if (CheckXBoxAnalogeIsNeutural(6) && mXboxStickValue == -1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Down);
        if (CheckXBoxAnalogeIsNeutural(6) && mXboxStickValue == 1)
            SetXboxButtonCodeToData(XBoxButton.DPad_Up);
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
    
    private bool CheckXBoxAnalogeIsNeutural(int anAxisIndex)
    {
        if (mAStickHasMoved )
            return false;

        mXboxStickValue = (int)UnityEngine.Input.GetAxis(XBox360Constrol.XboxAxisNames[anAxisIndex]);

        if (mXboxStickValue != 0)
        {
            mAStickHasMoved = true;
            Debug.Log(XBox360Constrol.XboxAxisNames[anAxisIndex] + " " + mXboxStickValue);
            return false;
        }

        mAStickHasMoved = false;
        
        return true;
    }
}
