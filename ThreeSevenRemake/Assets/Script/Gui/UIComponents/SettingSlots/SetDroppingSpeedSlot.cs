using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDroppingSpeedSlot : SettingSlotWithSlider
{
    private int mDroppingSpeedValue = Constants.MIN_DROPPING_SPEED_LEVEL;
    public override void Start()
    {
        base.Start();
        Slider.maxValue = Constants.MAX_DROPPING_SPEED_LEVEL;
        Slider.minValue = Constants.MIN_DROPPING_SPEED_LEVEL;

        Slider.value = mDroppingSpeedValue;
        DisplayValue();
    }

    protected override void Navigation()
    {
        if (ControlManager.Ins.MenuNavigationHold(CommandIndex.NAVI_LEFT, 2f))
        {
            MoveSlider(-1);
        }
        if (ControlManager.Ins.MenuNavigationHold(CommandIndex.NAVI_RIGHT, 2f))
        {
            MoveSlider(1);
        }
    }

    public override void ChangeSetting(ref GameplaySettingData aSettingData)
    {
        aSettingData.SelectLevelOfSpeed = mDroppingSpeedValue;
    }

    protected override void MoveSlider(int aDirection)
    {
        base.MoveSlider(aDirection);
        mDroppingSpeedValue = (int)Slider.value;
    }

}
