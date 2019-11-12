using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTextBox : ScoreboardComponentBase
{
    private float mGameTime = 0f;
    private string mTimeInText = "";

    public override void Start()
    {

    }

    private void OnDestroy()
    {
    }

    protected override void ComponentsDisplay()
    {
        Clock();
    }

    private void Clock()
    {
        mGameTime += Time.deltaTime;

        int seconds = (int)(mGameTime % 60);
        int minutes = (int)((mGameTime / 60) % 60);
        int hours = (int)((mGameTime / 3600) % 60);

        if(hours > 0)
            mTimeInText = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        else
            mTimeInText = string.Format("{0:00}:{1:00}", minutes, seconds);

        ValueText.text = mTimeInText;
    }
}
