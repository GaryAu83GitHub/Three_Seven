using UnityEngine;

public enum Difficulties
{
    EASY,
    NORMAL,
    HARD
}
/// <summary>
/// This class is for storing the gameplay setting that can be change when the player chose to start a new game.
/// </summary>
public class GameSettings
{
    

    public static GameSettings Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new GameSettings();
            return mInstance;
        }
    }
    private static GameSettings mInstance;

    private Difficulties mDifficulties = Difficulties.EASY;
    public Difficulties Difficulty { get { return mDifficulties; } }

    private int mLimitRow = 17;
    public int LimitHigh { get { return mLimitRow; } }

    private int mDropSpeedMultiply = 0;
    public int StartSpeedMultiply { get { return mDropSpeedMultiply; } }

    private const int mMaxLimitRow = 18;
    private const int mMinLimitRow = 9;

    public void SetDifficulty(Difficulties aDifficulty)
    {
        mDifficulties = aDifficulty;
    }

    public void SetStartDropSpeed(int aSpeed)
    {
        mDropSpeedMultiply = aSpeed;
        Debug.Log(mDropSpeedMultiply);
    }

    /// <summary>
    /// Use to set the high of the limit line in the game.
    /// It's purpose is for future of adding in the setting to let player chose the challenge level
    /// </summary>
    /// <param name="aLimitLineRow">Requesting limit row</param>
    /// <returns>Return the row number that had been clamp between the max and min row number</returns>
    public void SetLimitLineLevel(int aLimitLineRow)
    {
        mLimitRow = Mathf.Clamp(aLimitLineRow, mMinLimitRow, mMaxLimitRow);
    }
}
