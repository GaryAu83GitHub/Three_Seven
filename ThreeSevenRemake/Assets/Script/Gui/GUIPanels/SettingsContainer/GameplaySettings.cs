using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySettings : SettingsContainerBase
{
    private enum SettingIndex
    {
        DIFFICULTY,
        LIMIT_LINE,
        DROPPING_SPEED,
        ACTIVE_GUIDE,
    }

    public delegate void OnReturnToSettingButtonContainer();
    public static OnReturnToSettingButtonContainer returnToSettingButtonContainer;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
    }
    
    protected override void Input()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_DOWN))
            SelectingSlot(1);
        else if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_UP))
            SelectingSlot(-1);

        if (ControlManager.Ins.MenuBackButtonPressed() || ControlManager.Ins.MenuCancelButtonPressed())
            returnToSettingButtonContainer?.Invoke();

        base.Input();
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

    private void SelectingSlot(int anIncreame)
    {
        if ((mCurrentSelectingSlotIndex + anIncreame) >= SettingSlots.Count)
            mCurrentSelectingSlotIndex = 0;
        else if ((mCurrentSelectingSlotIndex + anIncreame) < 0)
            mCurrentSelectingSlotIndex = SettingSlots.Count - 1;
        else
            mCurrentSelectingSlotIndex += anIncreame;

        mCurrentSelectedSlot.Exit();//ActivatingSlot(false);
        mCurrentSelectedSlot = SettingSlots[mCurrentSelectingSlotIndex];
        mCurrentSelectedSlot.Enter();//ActivatingSlot(true);
        return;
    }
}

public class GameplaySettingData
{
    public Difficulties SelectDifficulty = Difficulties.EASY;
    public List<bool> EnableDigits = new List<bool>() { true, true, false, false };
    public int SelectLimitLineHeight = 0;
    public int SelectLevelOfSpeed = 0;
    public bool SelectActiveGuide = true;
}  
