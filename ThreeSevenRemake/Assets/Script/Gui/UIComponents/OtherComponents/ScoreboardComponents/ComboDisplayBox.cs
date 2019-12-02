using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboDisplayBox : ScoreboardComponentBase
{
    public delegate void OnScoringOccure(int aScoringCount, float anOdds);
    public static OnScoringOccure scoringOccure;

    public delegate void OnChangeLevel(float aNewOdds);
    public static OnChangeLevel changeLevel;

    public CanvasGroup DigitCanvas;

    private Animation mComboAnimation;

    private float ScoringOdds
    {
        get
        {
            if (mNumberOfBlock == 0)
                return 1f;

            return (float)mScoringCount / (float)mNumberOfBlock;
        }
    }

    private int mChainCount = 0;
    private int mLongestChain = 0;
    private int mScoringCount = 0;
    private int mNumberOfBlock = 0;

    public override void Start()
    {
        base.Start();

        mComboAnimation = GetComponent<Animation>();
        //MainGamePanel.onAddCombo += UpdateCombo;
        BlockManager.consecutiveScoring += NewBlockLanded;
        scoringOccure?.Invoke(mScoringCount, ScoringOdds);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        //MainGamePanel.onAddCombo -= UpdateCombo;
        BlockManager.consecutiveScoring -= NewBlockLanded;
    }

    protected override void ComponentsDisplay()
    {

    }

    private void UpdateCombo(int aComboCount)
    {
        if (aComboCount < 1)
            return;

        ValueText.text = aComboCount.ToString();
        mComboAnimation.Play();
    }

    private void NewBlockLanded(bool isScoring)
    {
        mNumberOfBlock++;

        if (isScoring)
        {
            ConsecutiveNewBlockScoring(1);
            AddScoring();
        }
        else
            ConsecutiveNewBlockScoring(-1);
    }

    private void AddScoring()
    {
        mScoringCount++;
        scoringOccure?.Invoke(mScoringCount, ScoringOdds);

        if (mScoringCount >= 10)
        {
            changeLevel?.Invoke(ScoringOdds/*(float)mScoringCount / (float)mNumberOfBlock*/);
            mScoringCount = 0;
            mNumberOfBlock = 0;
        }
    }

    private void ConsecutiveNewBlockScoring(int aValue)
    {
        mChainCount += aValue;
        if (mChainCount < 0)
            mChainCount = 0;

        if (aValue == 1)
        {
            //mConsecutiveScoringCount += aValue;
            mComboAnimation.Play();
        }
        //else
        //    mConsecutiveScoringCount = aValue;

        DigitCanvas.alpha = mChainCount;
        ValueText.text = mChainCount.ToString();

        if (mChainCount > mLongestChain)
            mLongestChain = mChainCount;
    }

    protected override void GatherResultData(ref ResultData aData)
    {
        aData.SetLongestChain(mLongestChain);
        aData.SetAverageOdds(mScoringCount, mNumberOfBlock);
    }
}
