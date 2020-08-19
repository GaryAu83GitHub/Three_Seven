using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadNaviBindBox : MonoBehaviour
{
    public AnalogueSticks AxisType;

    public Image IconImage;
    public Toggle Toggle;    

    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;

    private Image BackgroundImage;

    void Start()
    {
        BackgroundImage = GetComponent<Image>();
    }

    public void BindingTrigger(AnalogueSticks aCurrentSelectAxis)
    {
        Toggle.isOn = (aCurrentSelectAxis == AxisType);
    }

    public void BoxSelected(ref AnalogueSticks aSelectAxis, bool isSelected)
    {
        if (isSelected)
        {
            BackgroundImage.sprite = SelectedSprite;
            aSelectAxis = AxisType;
        }
        else
            BackgroundImage.sprite = UnSelectedSprite;
    }
}
