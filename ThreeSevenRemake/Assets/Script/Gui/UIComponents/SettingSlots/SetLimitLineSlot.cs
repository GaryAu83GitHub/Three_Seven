using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLimitLineSlot : SettingSlotWithSlider
{
    private int mLineHeightValue = Constants.DEFAULT_ROOF_HEIGHT;

    public override void Start()
    {
        base.Start();
        Slider.maxValue = Constants.MAX_CEILING_HIGH;
        Slider.minValue = Constants.MIN_CEILING_HIGH;

        Slider.value = mLineHeightValue;
        DisplayValue();

        //mSliderScrollingSpeed = 1f;
        //mSliderMoveIntervall = 1;
    }

    public override void SetSlotValue(GameplaySettingData aData)
    {
        mLineHeightValue = aData.SelectLimitLineHeight;
        Slider.value = mLineHeightValue;
        DisplayValue();
    }

    protected override void ChangeGameplaySetting()
    {
        mGameplaySettingData.SelectLimitLineHeight = mLineHeightValue;
        base.ChangeGameplaySetting();
    }

    protected override void MoveSlider(int aDirection)
    {
        base.MoveSlider(aDirection);
        mLineHeightValue = (int)Slider.value;
        ChangeGameplaySetting();
    }
}
