using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartLevelSlot : SettingSlotWithSlider
{
    private int mStartLevel = LevelManager.Instance.CurrentLevel;//Constants.MIN_LEVEL;

    
    public override void Start()
    {
        base.Start();
        Slider.maxValue = Constants.MAX_LEVEL;
        Slider.minValue = Constants.MIN_LEVEL;

        Slider.value = mStartLevel;
        DisplayValue();
    }

    public override void SetSlotValue(GameplaySettingData aData)
    {
        mStartLevel = aData.SelectStartLevel;
        Slider.value = mStartLevel;
        DisplayValue();
    }

    protected override void ChangeGameplaySetting()
    {
        mGameplaySettingData.SelectStartLevel = mStartLevel;
        base.ChangeGameplaySetting();
    }

    protected override void MoveSlider(int aDirection)
    {
        base.MoveSlider(aDirection);
        mStartLevel = (int)Slider.value;
        ChangeGameplaySetting();
    }

}
