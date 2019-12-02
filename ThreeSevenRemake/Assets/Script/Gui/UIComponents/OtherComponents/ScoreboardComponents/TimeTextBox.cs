using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTextBox : ScoreboardComponentBase
{
    public delegate void OnTimeOver(bool aTimeIsOver);
    public static OnTimeOver timeOver;

    private float mGameTime = 300f;
    private string mTimeInText = "";

    private bool mTimerIsActive = false;

    private const float mGameTimeLimit = 300f;

    public override void Start()
    {
        base.Start();
        mGameTime = mGameTimeLimit;

        GameSceneMain.activeTimer += ActiveTheClock;
        Clock();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        GameSceneMain.activeTimer -= ActiveTheClock;
    }

    protected override void ComponentsDisplay()
    {
        if(mTimerIsActive)
            Clock();
    }

    private void Clock()
    {

        int seconds = (int)(mGameTime % 60);
        int minutes = (int)((mGameTime / 60) % 60);
        int hours = (int)((mGameTime / 3600) % 60);

        if(hours > 0)
            mTimeInText = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        else
            mTimeInText = string.Format("{0:00}:{1:00}", minutes, seconds);

        ValueText.text = mTimeInText;

        mGameTime -= Time.deltaTime;
        if (mGameTime < 0f)
            timeOver?.Invoke(true);
    }

    private void ActiveTheClock(bool activeTimer)
    {
        mTimerIsActive = activeTimer;
    }

    protected override void GatherResultData(ref ResultData aData)
    {   
        aData.SetPlayTime(mGameTime, mGameTimeLimit);
    }
}
