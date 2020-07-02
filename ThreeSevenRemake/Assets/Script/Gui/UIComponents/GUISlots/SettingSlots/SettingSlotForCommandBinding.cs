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
    { }

    protected virtual void Display()
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

}
