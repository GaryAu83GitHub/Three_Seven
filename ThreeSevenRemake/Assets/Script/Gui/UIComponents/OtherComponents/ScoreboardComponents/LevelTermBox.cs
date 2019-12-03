using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTermBox : ScoreboardComponentBase
{
    public Image LevelBar;
    public Image UpArrowImage;
    public Image DownArrowImage;

    public List<Color> StatusColors;

    //public Animation LevelUpAnimation;
    private Animator mAnimator;

    private int mLevel = 1;
    private int mHighestLevel = 1;
    private float mFillingAmountValue = 0f;
    private float mFillupBarValue = 0f;
    private bool mFillingBar = false;

    private float mPreviousLevelUpOdds = 0f;
    private float mUpgradeThresholdOdds = .5f;
    private float mDowngradeThresholdOdds = .5f;

    private const float mMiddleOddsValue = .5f;

    private Gradient mGradient = new Gradient();
    private GradientAlphaKey[] mGradientAplhaKeys;
    private GradientColorKey[] mGradientColorKeys;


    public override void Start()
    {
        base.Start();
        //MainGamePanel.onAddLevelFilling += FillingBarWith;
        ComboDisplayBox.scoringOccure += ScoringOccure;
        ComboDisplayBox.changeLevel += ChangeLevel;

        //mAnimator = GetComponent<Animator>();

        mGradientColorKeys = new GradientColorKey[3];
        mGradientColorKeys[0].color = StatusColors[0];
        mGradientColorKeys[0].time = 0f;
        mGradientColorKeys[1].color = StatusColors[1];
        mGradientColorKeys[1].time = .5f;
        mGradientColorKeys[2].color = StatusColors[2];
        mGradientColorKeys[2].time = 1f;

        mGradientAplhaKeys = new GradientAlphaKey[3];
        mGradientAplhaKeys[0].alpha = 1f;
        mGradientAplhaKeys[0].time = 0f;
        mGradientAplhaKeys[1].alpha = 1f;
        mGradientAplhaKeys[1].time = .5f;
        mGradientAplhaKeys[2].alpha = 1f;
        mGradientAplhaKeys[2].time = 1f;

        mGradient.SetKeys(mGradientColorKeys, mGradientAplhaKeys);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        //MainGamePanel.onAddLevelFilling -= FillingBarWith;
        ComboDisplayBox.scoringOccure -= ScoringOccure;
        ComboDisplayBox.changeLevel -= ChangeLevel;
    }

    protected override void ComponentsDisplay()
    {
        ValueText.text = mLevel.ToString();
        BarFilling();
    }

    public override void ResetStartValue()
    {
        mPreviousLevelUpOdds = 0;
        mUpgradeThresholdOdds = .5f;
        mDowngradeThresholdOdds = .5f;
    }

    private void ScoringOccure(int aScoringCount, float anOdds)
    {
        FillingBarWith((float)aScoringCount / 10f);
        LevelBar.color = mGradient.Evaluate(anOdds);
    }

    private void ChangeLevel(float aNewOdds)
    {
        DynamicLeveling(aNewOdds);

        if (mLevel > mHighestLevel)
            mHighestLevel = mLevel;
    }

    private void DynamicLeveling(float aNewOdds)
    {
        // when the odds pass the upgrade threshold odds value
        if (aNewOdds >= mUpgradeThresholdOdds)
        {
            if (mLevel < 100)
                mLevel++;
            //mAnimator.SetTrigger("LevelUp");
            mUpgradeThresholdOdds = aNewOdds;
        }
        // when the odds pass the downgrade threshold odds value
        else if (aNewOdds <= mDowngradeThresholdOdds)
        {
            if (mLevel > 0)
                mLevel--;
            //mAnimator.SetTrigger("LevelDown");
            mDowngradeThresholdOdds = aNewOdds;
        }
        // when the odds is between the threshold of upgrade and downgrade
        else
        {
            if (aNewOdds < mUpgradeThresholdOdds && aNewOdds > mMiddleOddsValue)
                mUpgradeThresholdOdds = aNewOdds;
            else if (aNewOdds > mDowngradeThresholdOdds && aNewOdds < mMiddleOddsValue)
                mDowngradeThresholdOdds = aNewOdds;
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
    
    protected override void GatherResultData(ref ResultData aData)
    {
        aData.SetReachedLevels(mHighestLevel);
    }
}
