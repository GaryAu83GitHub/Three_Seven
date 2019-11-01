using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingSlotWithSlider : SettingSlotBase
{
    public Slider Slider;
    public TextMeshProUGUI ValueText;

    public override void Start()
    {
        base.Start();
    }

    protected override void Navigation()
    {
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_LEFT))
        {
            MoveSlider(-1);
        }
        if (ControlManager.Ins.MenuNavigationPress(CommandIndex.NAVI_RIGHT))
        {
            MoveSlider(1);
        }
    }

    protected virtual void MoveSlider(int aDirection)
    {
        Slider.value += aDirection;
        DisplayValue();
    }

    protected void DisplayValue()
    {
        ValueText.text = Slider.value.ToString();
    }
}
