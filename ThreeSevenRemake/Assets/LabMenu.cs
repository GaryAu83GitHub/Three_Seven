using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabMenu : MonoBehaviour
{
    public Text labText;
    public Image Seconds;
    public Image Minutes;
    public Image SecondLine;
    public Image MinuteLine;

    private float waitTime = 2.0f;
    private float timer = 0.0f;
    private float visualTime = 0.0f;
    private float sectionValue = 1f / 60f;
    private float lineRotateIntervall = 360f / 60f;
    private int second = 0;
    private int minute = 0;
    private float secondDegree = 0f;
    private float minuteDegree = 0f;

    private RectTransform secondLineTransform;
    private RectTransform minuteLineTransform;

    // Start is called before the first frame update
    void Start()
    {
        secondLineTransform = SecondLine.GetComponent<RectTransform>();
        minuteLineTransform = MinuteLine.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        labText.text = ((int)timer).ToString();

        if (timer >= 1)
        {
            timer = 0;
            second++;
            
        }
        if (second > 60)
        {
            second = 0;
            minute++;

        }
        if (minute >= 60)
            minute = 0;

        //Seconds.fillAmount = (int)timer * sectionValue;
        //Minutes.fillAmount = minute * sectionValue;

        Clock();
    }

    private void Clock()
    {
        secondDegree = second * lineRotateIntervall;
        minuteDegree = minute * lineRotateIntervall;
        secondLineTransform.rotation.z = new Vector3(0, 0, -secondDegree);
        minuteLineTransform.Rotate(new Vector3(0, 0, -minuteDegree));
    }
}
