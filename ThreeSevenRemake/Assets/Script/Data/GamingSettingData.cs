using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameRoundData
{
    // this
    private string mPlayerName = "";
    public string PlayerName { get { return mPlayerName; } set { mPlayerName = value; } }

    // Game round settings variabler
    private List<bool> mEnableScoringMethods = new List<bool>() { true, false, false, false };
    public List<bool> EnableScoringMethods { get { return mEnableScoringMethods; } set { mEnableScoringMethods = value; } }

    private int mInitialTaskValue = 0;
    public int InitialTaskValue { get { return mInitialTaskValue; } set { mInitialTaskValue = value; } }

    private int mRoofHeightValue = 0;
    public int RoofHeightValue { get { return mRoofHeightValue; } set { mRoofHeightValue = value; } }

    private int mDroppingSpeedMultiplyValue = 0;
    public int DroppingSpeedMultiplyValue { get { return mDroppingSpeedMultiplyValue; } set { mDroppingSpeedMultiplyValue = value; } }

    // game round achievments for storing purpose
    private int mCurrentLevel = 0;
    public int CurrentLevel { get { return mCurrentLevel; } set { mCurrentLevel = value; } }

    private int mCurrentScore = 0;
    public int CurrentScore { get { return mCurrentScore; } set { mCurrentScore = value; } }

    private int mCurrentMaxCombo = 0;
    public int MaxCombo { get { return mCurrentMaxCombo; } set { mCurrentMaxCombo = value; } }

    private float mGameTime = 0f;
    public float GameTime { get { return mGameTime; } set { mGameTime = value; } }

    private int mLandedBlockCount = 0;
    public int LandedBlockCount { get { return mLandedBlockCount; } set { mLandedBlockCount = value; } }
}

