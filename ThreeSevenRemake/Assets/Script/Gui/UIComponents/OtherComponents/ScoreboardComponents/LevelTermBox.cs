using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTermBox : ScoreboardComponentBase
{
    public Image LevelBar;
    public Image UpArrowImage;
    public Image DownArrowImage;

    //public Animation LevelUpAnimation;
    private Animator mAnimator;

    private int mLevel = 1;
    private float mFillingAmountValue = 0f;
    private float mFillupBarValue = 0f;
    private bool mFillingBar = false;

    private float mPreviousLevelUpOdds = 0f;

    public override void Start()
    {
        //MainGamePanel.onAddLevelFilling += FillingBarWith;
        ComboDisplayBox.scoringOccure += ScoringOccure;
        ComboDisplayBox.changeLevel += ChangeLevel;
    }

    private void OnDestroy()
    {
        //MainGamePanel.onAddLevelFilling -= FillingBarWith;
        ComboDisplayBox.scoringOccure -= ScoringOccure;
        ComboDisplayBox.changeLevel -= ChangeLevel;
    }

    protected override void ComponentsDisplay()
    {
        ValueText.text = mLevel.ToString();
        BarFilling();
    }

    private void ScoringOccure(int aScoringCount)
    {
        FillingBarWith((float)aScoringCount / 10f);
    }

    private void ChangeLevel(float aNewOdds)
    {
        if(aNewOdds >= mPreviousLevelUpOdds)
        {
            // Animation for level go up plays
            if (mLevel < 100)
                mLevel++;
            //mAnimator.SetTrigger("LevelUp");
        }
        else
        {
            // Animation for level go down plays
            if (mLevel > 0)
                mLevel--;
            //mAnimator.SetTrigger("LevelDown");
        }
        mPreviousLevelUpOdds = aNewOdds;
    }

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
