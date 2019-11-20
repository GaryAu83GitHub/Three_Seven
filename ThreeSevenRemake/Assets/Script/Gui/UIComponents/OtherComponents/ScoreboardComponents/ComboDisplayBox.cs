using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboDisplayBox : ScoreboardComponentBase
{
    public delegate void OnScoringOccure(int aScoringCount);
    public static OnScoringOccure scoringOccure;

    public delegate void OnChangeLevel(float aNewOdds);
    public static OnChangeLevel changeLevel;

    public CanvasGroup DigitCanvas;

    private Animation mComboAnimation;

    private int mConsecutiveScoringCount = 0;
    private int mScoringCount = 0;
    private int mNumberOfBlock = 0;

    public override void Start()
    {
        mComboAnimation = GetComponent<Animation>();
        //MainGamePanel.onAddCombo += UpdateCombo;
        BlockManager.consecutiveScoring += NewBlockLanded;
    }

    private void OnDestroy()
    {
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
            ConsecutiveScoring(1);
            mScoringCount++;
            scoringOccure?.Invoke(mScoringCount);

            if (mScoringCount >= 10)
            {
                changeLevel?.Invoke((float)mScoringCount / (float)mNumberOfBlock);
                mScoringCount = 0;
                mNumberOfBlock = 0;
            }
        }
        else
            ConsecutiveScoring(0);
    }

    private void ConsecutiveScoring(int aValue)
    {
        if (aValue != 0)
        {
            mConsecutiveScoringCount += aValue;
            mComboAnimation.Play();
        }
        else
            mConsecutiveScoringCount = aValue;

        DigitCanvas.alpha = mConsecutiveScoringCount;
        ValueText.text = mConsecutiveScoringCount.ToString();
    }
}
