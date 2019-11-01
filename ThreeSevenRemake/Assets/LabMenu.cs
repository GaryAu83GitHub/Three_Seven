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

    //private float waitTime = 2.0f;
    //private float timer = 0.0f;
    //private float visualTime = 0.0f;
    //private float sectionValue = 1f / 60f;
    //private float lineRotateIntervall = 360f / 60f;
    //private int second = 0;
    //private int minute = 0;
    //private int nextSecond = 0;
    //private int nextMinute = 1;
    //private float secondDegree = 0f;
    //private float minuteDegree = 0f;

    //private RectTransform secondLineTransform;
    //private RectTransform minuteLineTransform;

    // Start is called before the first frame update
    void Start()
    {
        MenuManager.Instance.StartWithPanel(MenuPanelIndex.TITLE_PANEL);
        //secondLineTransform = SecondLine.GetComponent<RectTransform>();
        //minuteLineTransform = MinuteLine.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;

        ////labText.text = timer.ToString();

        //if (timer >= 1)
        //{
        //    second++;
        //    Clock();
        //    timer = 0;
        //}
        //if (second >= 60)
        //{
        //    minute++;
        //    Clock();
        //    second = 0;
        //    nextSecond = 1;
        //}
        //if (minute >= 60)
        //{
        //    minute = 0;
        //    nextMinute = 1;
        //}

        //labText.text = ((int)minute).ToString() + " : " + ((int)second).ToString();
        ////Seconds.fillAmount = (int)timer * sectionValue;
        ////Minutes.fillAmount = minute * sectionValue;

        
    }

    private void Clock()
    {
        //if (second >= nextSecond)
        //{
        //    secondLineTransform.Rotate(new Vector3(0, 0, -lineRotateIntervall));
        //    nextSecond++;
        //}
        //if (minute >= nextMinute)
        //{
        //    minuteLineTransform.Rotate(new Vector3(0, 0, -lineRotateIntervall));
        //    nextMinute++;
        //}
    }
}
