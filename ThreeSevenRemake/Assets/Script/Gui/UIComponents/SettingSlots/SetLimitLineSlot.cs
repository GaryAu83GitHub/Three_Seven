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
    }
    
    public override void ChangeSetting(ref GameplaySettingData aSettingData)
    {
        aSettingData.SelectLimitLineHeight = mLineHeightValue;
    }

    protected override void MoveSlider(int aDirection)
    {
        base.MoveSlider(aDirection);
        mLineHeightValue = (int)Slider.value;
    }
}
