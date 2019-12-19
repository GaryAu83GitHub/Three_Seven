using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LevelUpMode
{
    LINEAR,
    DYNAMIC
}

/// <summary>
/// This class is attached to the component that update and display the game level during the game round.
/// It'll be responsible for storing the level and displaying the level status. The level value
/// is the factor to determined most of the earning score value.
/// The storing result subject this class is the number of level up that had been made during
/// the play throught 
/// The sub issue it's responsible for
/// -> Highest reached level
/// -> Highest level up threshold
/// -> Lowest level down threshold
/// </summary>
public class LevelTermBox : ScoreboardComponentBase
{
    public Image LevelBar;
    public Image UpArrowImage;
    public Image DownArrowImage;

    public List<Color> StatusColors;
    public List<float> StatusGradientAlphas;
    public List<float> StatusGradientTimes;

    //public Animation LevelUpAnimation;
    private Animator mAnimator;

    /// <summary>
    /// Holding the current level value
    /// </summary>
    private int mCurrentLevel = 1;
    /// <summary>
    /// Storing the highest level value that had ever reached during the playthroght
    /// This will be set to Result Data
    /// </summary>
    private int mHighestLevel = 1;

    /// <summary>
    /// Counting on number of time the level have surpassed it's highest value
    /// </summary>
    private int mGainedLevelCount = 0;
    /// <summary>
    /// Storing value of the highest level value had been reached, recieve the current
    /// level count when it's lesser
    /// This will be set to Result Data
    /// </summary>
    private int mHighestGainedLevelCount = 0;

    private float mFillingAmountValue = 0f;
    private float mFillupBarValue = 0f;
    private bool mFillingBar = false;

    /// <summary>
    /// The current threshold odds value for increase the level value
    /// </summary>
    private float mCurrentUpgradeThresholdOdds = .5f;
    /// <summary>
    /// The best threshold odds had ever reached, changed when the odds is larger than the
    /// current upgrade threhold odds value
    /// This will be set to Result Data
    /// </summary>
    private float mBestThresholdOdds = .5f;

    /// <summary>
    /// The current threshold odds value for decreasing the level value
    /// </summary>
    private float mCurrentDowngradeThresholdOdds = .5f;
    /// <summary>
    /// the worst threshold odds had ever fall, changed when the odds is smaller than the
    /// current downgrade threshold odds value
    /// This will be set to Result Data
    /// </summary>
    private float mWorstThresholdOdds = .5f;

    /// <summary>
    /// The default center value for the up and downgrade threshold odds value
    /// </summary>
    private const float mMiddleOddsValue = .5f;

    /// <summary>
    /// The gradient for the level bar
    /// </summary>
    private Gradient mGradient = new Gradient();

    public override void Start()
    {
        base.Start();
        //MainGamePanel.onAddLevelFilling += FillingBarWith;
        ChainDisplayBox.scoringOccure += ScoringOccure;
        ChainDisplayBox.changeLevel += ChangeLevel;

        //mAnimator = GetComponent<Animator>();

        GradientContent.Instance.AddGradient(
            "LevelBarGradient",
            StatusColors.ToArray(),
            StatusGradientAlphas.ToArray(),
            StatusGradientTimes.ToArray());

        mGradient = GradientContent.Instance.GetGradientBy("LevelBarGradient");
        return;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        ChainDisplayBox.scoringOccure -= ScoringOccure;
        ChainDisplayBox.changeLevel -= ChangeLevel;
    }

    protected override void ComponentsDisplay()
    {
        ValueText.text = mCurrentLevel.ToString();
        BarFilling();
    }

    public override void ResetStartValue()
    {
        mGainedLevelCount = 0;
        mCurrentUpgradeThresholdOdds = .5f;
        mCurrentDowngradeThresholdOdds = .5f;
    }

    protected override void GatherResultData(ref ResultData aData)
    {
        aData.SetGainedLevels(mGainedLevelCount, mHighestLevel, mBestThresholdOdds, mWorstThresholdOdds);
    }

    private void ScoringOccure(int aScoringCount, float anOdds)
    {
        FillingBarWith((float)aScoringCount / 10f);
        LevelBar.color = mGradient.Evaluate(anOdds);
    }

    private void ChangeLevel(float aNewOdds)
    {
        DynamicLeveling(aNewOdds);

        LevelManager.Instance.SetCurrentLevel(mCurrentLevel);

        if (mCurrentLevel > mHighestLevel || mCurrentLevel == 100)
        {
            mHighestLevel = mCurrentLevel;
            mGainedLevelCount++;
            if (mGainedLevelCount > mHighestGainedLevelCount)
                mHighestGainedLevelCount = mGainedLevelCount;
        }
    }

    private void DynamicLeveling(float aNewOdds)
    {
        // when the odds pass the upgrade threshold odds value
        if (aNewOdds >= mCurrentUpgradeThresholdOdds)
        {
            if (mCurrentLevel < 100)
                mCurrentLevel++;
            //mAnimator.SetTrigger("LevelUp");
            mCurrentUpgradeThresholdOdds = aNewOdds;
            if (mCurrentUpgradeThresholdOdds > mBestThresholdOdds)
                mBestThresholdOdds = mCurrentUpgradeThresholdOdds;
        }
        // when the odds pass the downgrade threshold odds value
        else if (aNewOdds <= mCurrentDowngradeThresholdOdds)
        {
            if (mCurrentLevel > 0)
                mCurrentLevel--;
            //mAnimator.SetTrigger("LevelDown");
            mCurrentDowngradeThresholdOdds = aNewOdds;
            if (mCurrentDowngradeThresholdOdds < mWorstThresholdOdds)
                mWorstThresholdOdds = mCurrentDowngradeThresholdOdds;
        }
        // when the odds is between the threshold of upgrade and downgrade
        else
        {
            if (aNewOdds < mCurrentUpgradeThresholdOdds && aNewOdds > mMiddleOddsValue)
                mCurrentUpgradeThresholdOdds = aNewOdds;
            else if (aNewOdds > mCurrentDowngradeThresholdOdds && aNewOdds < mMiddleOddsValue)
                mCurrentDowngradeThresholdOdds = aNewOdds;
        }
    }

    private void LinearLeveling()
    { }

    private void FillingBarWith(float aFillingValue)
    {
        mFillingAmountValue = aFillingValue;
        mFillingBar = true;
    }

    private void BarFilling()
    {
        if (!mFillingBar)
            return;

        mFillupBarValue += Time.deltaTime;
        if(mFillupBarValue >= 1f)
        {
            mFillupBarValue = 0f;
            mFillingAmountValue = 0f;
            mFillingBar = false;
        }
        else if(mFillupBarValue >= mFillingAmountValue)
        {
            mFillupBarValue = mFillingAmountValue;
            mFillingBar = false;
        }

        LevelBar.fillAmount = mFillupBarValue;
    }
    
    
}
