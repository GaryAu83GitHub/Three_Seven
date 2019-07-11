using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDroppingRatePanel : SettingPanelBase
{
    public Slider DroppingSpeedValueSlider;
    public TextMeshProUGUI CurrentValueText;
    public TextMeshProUGUI MaxSliderValueText;
    public TextMeshProUGUI MinSliderValueText;

    private float mCurrentDroppingSpeed = 0;

    private const float FASTEST = 18;
    private const float SLOWEST = 9;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
