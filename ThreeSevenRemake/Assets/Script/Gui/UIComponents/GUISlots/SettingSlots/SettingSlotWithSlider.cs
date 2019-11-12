using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingSlotWithSlider : SettingSlotBase
{
    public Slider Slider;
    public TextMeshProUGUI ValueText;

    public float mSliderScrollingSpeed = .1f;
    public int mSliderMoveIntervall = 1;

    public override void Start()
    {
        base.Start();
    }

    protected override void Navigation()
    {
        if (ControlManager.Ins.MenuNavigationHold(CommandIndex.NAVI_LEFT, mSliderScrollingSpeed))
        {
            MoveSlider(-mSliderMoveIntervall);
        }
        if (ControlManager.Ins.MenuNavigationHold(CommandIndex.NAVI_RIGHT, mSliderScrollingSpeed))
        {
            MoveSlider(mSliderMoveIntervall);
        }

        //Debug.Log("<color=cyan>" + anDelayIntervall + "</color>");
    }

    protected virtual void MoveSlider(int aDirection)
    {
        Slider.value += aDirection ;
        DisplayValue();
    }

    protected void DisplayValue()
    {
        ValueText.text = Slider.value.ToString();
    }
}
