using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetGuideblockSlot : SettingSlotWithToggles
{
    private enum GuideBlockSettingIndex
    {
        VISIBLE,
    }

    public override void Start()
    {
        Toggles[(int)GuideBlockSettingIndex.VISIBLE].isOn = GameSettings.Instance.ActiveGuideBlock;
        base.Start();
    }

    public override void SetSlotValue(GameplaySettingData aData)
    {
        Toggles[(int)GuideBlockSettingIndex.VISIBLE].isOn = aData.SelectActiveGuide;
    }

    protected override void ChangeGameplaySetting()
    {
        mGameplaySettingData.SelectActiveGuide = Toggles[(int)GuideBlockSettingIndex.VISIBLE].isOn;
        base.ChangeGameplaySetting();
    }

    protected override void SwitchToggle()
    {
        base.SwitchToggle();
        ChangeGameplaySetting();
    }
}
