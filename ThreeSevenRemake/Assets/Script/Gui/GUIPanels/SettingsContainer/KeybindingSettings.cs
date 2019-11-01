using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindingSettings : SettingsContainerBase
{
    public delegate void OnReturnToSettingButtonContainer();
    public static OnReturnToSettingButtonContainer returnToSettingButtonContainer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (ControlManager.Ins.MenuBackButtonPressed() || ControlManager.Ins.MenuCancelButtonPressed())
            returnToSettingButtonContainer?.Invoke();
    }

    public override void Enter()
    {
        mCurrentSelectingSlotIndex = 0;
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }
}
