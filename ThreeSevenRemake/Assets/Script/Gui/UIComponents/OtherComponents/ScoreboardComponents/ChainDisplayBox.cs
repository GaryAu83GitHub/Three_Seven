using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is attached to the component that update and display the chain value 
/// during the game round.
/// It'll be responsible for increase and decrease the chain value when the new 
/// created block scores or not.
/// The chain value is also the changing factor for level and bonus value to increase
/// or decrease.
/// The sub issue this class have responsible for is
/// -> Longest consecutive increase chain
/// -> Longest consecutive decrease chain
/// Except of the chain result data, this class also has the responsible of keep count
/// on scoring times and number of landed blocks for the result data
/// </summary>
public class ChainDisplayBox : ScoreboardComponentBase
{
    public delegate void OnScoringOccure(int aScoringCount, float anOdds);
    public static OnScoringOccure scoringOccure;

    public delegate void OnChangeLevel(float aNewOdds);
    public static OnChangeLevel changeLevel;

    public delegate void OnChangeBonusTerm(int aNewBonusTerms);
    public static OnChangeBonusTerm changeBonusTerm;

    public delegate void OnUpdateChainCount(int aCurrentChainCount);
    public static OnUpdateChainCount updateChainCount;

    public CanvasGroup DigitCanvas;

    private Animation mChainAnimation;

    /// <summary>
    /// Return an odds value of the scoring count divided with number of block that has been landed between each level up
    /// </summary>
    private float ScoringOdds
    {
        get
        {
            if (mTotalLandedBlockCount == 0)
                return 1f;

            return (float)mTotalScoringAtLandingCount / (float)mTotalLandedBlockCount;
        }
    }

    /// <summary>
    /// Storing the change of the chain value
    /// </summary>
    private int mChainCount = 0;

    /// <summary>
    /// Storing the longest chain value that had made during the game round
    /// This will be set to Result Data
    /// </summary>
    private int mLongestChain = 0;

    /// <summary>
    /// Storing the current count of increasing consecutive chain
    /// </summary>
    private int mCurrentConsecutiveIncreaseCount = 0;

    /// <summary>
    /// Storing the highest consecutive increasing chain that ever made during the play
    /// throught.
    /// This will be set to Result Data
    /// </summary>
    private int mHighestConsecutiveIncreaseCount = 0;

    /// <summary>
    /// Storing the current count of decreasing consecutive chain
    /// </summary>
    private int mCurrentConsecutiveDecreaseCount = 0;

    /// <summary>
    /// Storing the lowest consecutive decreasing chain that ever made during the play
    /// throught.
    /// This will be set to Result Data
    /// </summary>
    private int mLowestConsecutiveDecreaseChain = 0;

    /// <summary>
    /// Counting value to keep the number of scoring each time when a new block make a scoring.
    /// Will be reset when the value reach 10 which is a default value for level up.
    /// This will be set to Result Data
    /// </summary>
    private int mScoringCount = 0;

    /// <summary>
    /// Counting value to keep the number of new landed block before mScoringCount reach 10.
    /// Will be reset when the mScoringCount reach 10 which is a default value for level up.
    /// This will be set to Result Data
    /// </summary>
    private int mNumberOfBlock = 0;

    private int mTotalLandedBlockCount = 0;
    private int mTotalScoringAtLandingCount = 0;

    public override void Start()
    {
        base.Start();

        mChainAnimation = GetComponent<Animation>();
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

    protected override void GatherResultData(ref ResultData aData)
    {
        aData.SetLongestChain(mLongestChain, mHighestConsecutiveIncreaseCount, mLowestConsecutiveDecreaseChain);
        aData.SetAverageOdds(mTotalScoringAtLandingCount, mTotalLandedBlockCount);
    }

    private void UpdateCombo(int aComboCount)
    {
        if (aComboCount < 1)
            return;

        ValueText.text = aComboCount.ToString();
        mChainAnimation.Play();
    }

    private void NewBlockLanded(bool isScoring)
    {
        if (BlockManager.Instance.Blocks.Count < 1)
            return;

        mNumberOfBlock++;
        mTotalLandedBlockCount++;

        if (isScoring)
        {
            mTotalScoringAtLandingCount++;
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
        if (aValue == 1)
        {
            mCurrentConsecutiveIncreaseCount++;
            mChainAnimation.Play();
        }
        else if (aValue == -1)
        {
            mCurrentConsecutiveDecreaseCount--;
        }

        SurpassCurrentConsecutiveCheck();

            mChainCount += aValue;
        if (mChainCount < 0)
            mChainCount = 0;

        updateChainCount?.Invoke(mChainCount);

        DigitCanvas.alpha = mChainCount;
        ValueText.text = mChainCount.ToString();

        if (mChainCount > mLongestChain)
            mLongestChain = mChainCount;
    }

    private void SurpassCurrentConsecutiveCheck()
    {
        if (mCurrentConsecutiveIncreaseCount > mHighestConsecutiveIncreaseCount)
            mHighestConsecutiveIncreaseCount = mCurrentConsecutiveIncreaseCount;

        if (mCurrentConsecutiveDecreaseCount < mLowestConsecutiveDecreaseChain)
            mLowestConsecutiveDecreaseChain = mCurrentConsecutiveDecreaseCount;
    }
}
