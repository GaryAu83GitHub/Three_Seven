using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is attached to the component that update and display the play time during the game round.
/// It'll be responsible for storing the time and convert it to displaying format of 00:00:00, and if
/// the game mode has a time base rule , this class is the major factor to determined the game is over. 
/// Depending on the game mode the direction of the time flow will change.
/// The storing result subject this class is responsible for
/// -> Used time
/// </summary>
public class TimeTextBox : ScoreboardComponentBase
{
    public delegate void OnTimeOver(bool aTimeIsOver);
    public static OnTimeOver timeOver;

    /// <summary>
    /// The update value for the game time
    /// </summary>
    private float mGameTime = 0f;

    /// <summary>
    /// The direction of the time flow
    /// </summary>
    private int mTimerDirection = 1;

    /// <summary>
    /// The acitviator for if the time shall update
    /// </summary>
    private bool mTimerIsActive = false;

    /// <summary>
    /// The value of time the limited time when the time flow is backwarded
    /// </summary>
    private float mGameTimeLimit = 300f;

    /// <summary>
    /// When enter the game state, the object will subscribe to the main game codes methods
    /// to set up the game mode to determined if the game's time direciton, and the time
    /// active switch command.
    /// </summary>
    public override void Start()
    {
        base.Start();
        mGameTime = mGameTimeLimit;

        GameSceneMain.setGameMode += SetGameMode;
        GameSceneMain.activeTimer += ActiveTheClock;

        MainGamePanel.gamePause += GamePause;

        Clock();
    }

    /// <summary>
    /// Unsubscribe all delegates when this object is destroyed
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();

        GameSceneMain.setGameMode -= SetGameMode;
        GameSceneMain.activeTimer -= ActiveTheClock;

        MainGamePanel.gamePause -= GamePause;
    }

    /// <summary>
    /// The override method of displaying and update the time when the game is playing
    /// </summary>
    protected override void ComponentsDisplay()
    {
        if(mTimerIsActive)
            Clock();
    }

    /// <summary>
    /// The result subject that been stored from this class is the used time value.
    /// </summary>
    /// <param name="aData"></param>
    protected override void GatherResultData(ref ResultData aData)
    {   
        aData.SetPlayTime(Mathf.RoundToInt(mGameTime), Mathf.RoundToInt(mGameTimeLimit));
        base.GatherResultData(ref aData);
    }

    /// <summary>
    /// The override method to reset the start value to this component
    /// </summary>
    public override void ResetStartValue()
    {
        mGameTime = 0;
        mTimerIsActive = false;
    }

    /// <summary>
    /// The update and display method for the timer and also checking if the time is up if the game mode
    /// is time based.
    /// It also is responsible to convert the current time to the string format of 00:00:00
    /// </summary>
    private void Clock()
    {
        if (mTimerDirection == -1 && mGameTime < 0f)
        {
            mGameTime = 0f;
            timeOver?.Invoke(true);
            return;
        }

        mGameTime += mTimerDirection * Time.deltaTime;

        //int seconds = (int)(mGameTime % 60);
        //int minutes = (int)((mGameTime / 60) % 60);
        //int hours = (int)((mGameTime / 3600) % 60);

        //if(hours > 0)
        //    mTimeInText = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        //else
        //    mTimeInText = string.Format("{0:00}:{1:00}", minutes, seconds);

        ValueText.text = TimeTool.TimeString(mGameTime);
    }

    /// <summary>
    /// Allow the timer to update depending on if the game is paused or under scoring and 
    /// animation progress.
    /// /// This method will be subscrbing with the activeTimer delegate method in GameSceneMain
    /// </summary>
    /// <param name="activeTimer"></param>
    private void ActiveTheClock(bool activeTimer)
    {
        mTimerIsActive = activeTimer;
    }

    /// <summary>
    /// Set up the time flow direction of the desired game mode.
    /// Time based game mode will have the time flow to tick down while the other
    /// will tick up.
    /// This method will be subscrbing with the setGameMode delegate method in GameSceneMain
    /// </summary>
    /// <param name="aMode">The desired game mode</param>
    private void SetGameMode(GameMode aMode)
    {
        if (aMode != GameMode.CLASSIC)
            mTimerDirection = -1;
        else
            mTimerDirection = 1;
    }

    /// <summary>
    /// Set up the time limit value in game mode that are time based.
    /// What is take in is the number of seconds that is playing during the game mode
    /// </summary>
    /// <param name="aTimeLimit"></param>
    private void SetTimeLimit(float aTimeLimit)
    {
        mGameTimeLimit = aTimeLimit;
    }

    private void GamePause(bool gameIsPausing)
    {
        mTimerIsActive = !gameIsPausing;
    }
}
