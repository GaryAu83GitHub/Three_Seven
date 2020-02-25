using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSlotForCommandBinding : SettingSlotBase
{
    public CommandIndex KeybindCommand;
    public GameObject KeybindContainer;

    private CanvasGroup mBindContainerMG;

    protected KeybindData mKeybindingData = new KeybindData();

    private bool mChangeModeOn = false;

    private float mChangeModeSuspendCountdown = 0f;

    private const float mChangeModeSuspendTime = .05f;

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
            CheckForKeyboardInput();
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

    public void SetKey(KeybindData aData)
    {
        mKeybindingData = new KeybindData(aData);
        Display();
    }

    protected void ActiveChangeMode(bool isChangeModeOn)
    {
        if (KeybindContainer == null)
            return;

        mChangeModeOn = isChangeModeOn;
        mLockParentInput = mChangeModeOn;

        mChangeModeSuspendCountdown = (isChangeModeOn) ? mChangeModeSuspendTime : 0f;
        mBindContainerMG.alpha = (mChangeModeOn ? 1f : .5f);
    }
}
