using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class is attached to the component that update and display the current scoring
/// in the game round.
/// This class will also be responsible to store the score value which is a main subject to the highscore
/// along with its sub issues:
/// -> Highest gained score in one round
/// -> Highest bonus term accessed
/// -> Highest combo that had been made in the scoring round
/// </summary>
public class ScoreDisplayBox : ScoreboardComponentBase
{
    public GameObject ScoreAddOn;
    public GameObject BonusAddOn;

    public delegate void OnPowerUpCharging();
    public static OnPowerUpCharging powerUpCharging;

    /// <summary>
    /// The score that is displaying, updating when the total score is changed, it'll be recieving
    /// the total score value after the update if it become greater than the total score
    /// </summary>
    private int mCurrentDisplayScore = 0;

    /// <summary>
    /// The total score the player earn during the play throught
    /// This will be set to Result Data
    /// </summary>
    private int mCurrentTotalScore = 0;

    /// <summary>
    /// The score that gained during the last landed block's scoring progress
    /// </summary>
    private int mThisRoundGainedScore = 0;

    /// <summary>
    /// The highest score that ever had gained in all the landed block's scoring progress
    /// This will be set to Result Data 
    /// </summary>
    private int mHighestGainedRoundScore = 0;

    /// <summary>
    /// The current bonus multiply term that the bonus tree had reached
    /// </summary>
    private int mCurrentBonusTerm = 1;

    /// <summary>
    /// The best bonus term that ever had reached during the play throught
    /// This will be set to Result Data 
    /// </summary>
    private int mBestBonusTerm = 1;

    /// <summary>
    /// The counting on number of combo had been made during the last landed block's scoring progress
    /// </summary>
    private int mComboCount = 0;

    /// <summary>
    /// The best combo that ever had made among the number of scoring progress of the landed blocks
    /// This will be set to Result Data 
    /// </summary>
    private int mBestComboCount = 0;

    /// <summary>
    /// The switcher to update the displaying score value when the total score value is greater and switch
    /// back when both value are equal
    /// </summary>
    private bool mUpdateDisplayScore = false;

    public override void Start()
    {
        base.Start();

        GamingManager.comboScoring += ComboScoring;
        GamingManager.linkingCubeScores += LinkingCubeScores;
        GamingManager.newBlockLandedScores += NewLandedBlockScores;

        BonusDisplayBox.changeBonusTerm += ChangeBonusTerm;
        UpdateDisplayScore();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        GamingManager.comboScoring -= ComboScoring;
        GamingManager.linkingCubeScores -= LinkingCubeScores;
        GamingManager.newBlockLandedScores -= NewLandedBlockScores;

        BonusDisplayBox.changeBonusTerm -= ChangeBonusTerm;
    }
    
    protected override void ComponentsDisplay()
    {
        if (mUpdateDisplayScore)
            UpdateDisplayScore();
    }

    protected override void GatherResultData(ref ResultData aData)
    {
        aData.SetGainScores(mCurrentTotalScore, mHighestGainedRoundScore, mBestBonusTerm, mBestComboCount);
        base.GatherResultData(ref aData);
    }

    public override void ResetStartValue()
    {
        mCurrentDisplayScore = 0;
        mCurrentTotalScore = 0;

        mHighestGainedRoundScore = 0;
        mThisRoundGainedScore = 0;

        mUpdateDisplayScore = false;

        mCurrentBonusTerm = 1;
        mBestBonusTerm = 1;

        mComboCount = 0;
        mBestComboCount = 0;
    }

    /// <summary>
    /// Here is where the current total score and round score us updateing and assign the displaying
    /// add score to the Text UI components and switch on the update score boolian
    /// </summary>
    /// <param name="anAddOnTotalScore">A calculat total score that adds in to the current total score</param>
    /// <param name="anAddOnDisplayScore">A score that will be displaying on the addOn component</param>
    private void UpdateScore(int anAddOnTotalScore, int anAddOnDisplayScore)
    {
        mCurrentTotalScore += anAddOnTotalScore;
        mThisRoundGainedScore += anAddOnTotalScore;

        SurpassCountCheck();

        ScoreAddOn.GetComponent<Text>().text = "+" + anAddOnDisplayScore.ToString();

        if (mCurrentBonusTerm > 1)
        {
            ScoreAddOn.GetComponent<Text>().text += " x " + mCurrentBonusTerm.ToString();
        }
        ScoreAddOn.GetComponent<Animation>().Play();
        mUpdateDisplayScore = true;
    }

    /// <summary>
    /// This method is subscribing to the change bonus term delegate from BonusDisplayBox to
    /// change the current bonus multiply term and stored down the best bonus term when it recieved
    /// bonus term is greater than the current best bonus term
    /// </summary>
    /// <param name="aNewBonusTerms"></param>
    private void ChangeBonusTerm(int aNewBonusTerms)
    {
        mCurrentBonusTerm = aNewBonusTerms;
        if (mCurrentBonusTerm > mBestBonusTerm)
            mBestBonusTerm = mCurrentBonusTerm;
    }

    /// <summary>
    /// The method to update the displaying score when the total score is greater and convert
    /// to string in order for the Text UI component to display the value.
    /// The update will turn off when the displaying score is greater then the total score and
    /// change its value to the total score.
    /// Base on how much the display score increase its value is depending on how much different
    /// its value is from the total score
    /// </summary>
    private void UpdateDisplayScore()
    {
        if(mCurrentDisplayScore < mCurrentTotalScore)
        {
            if ((mCurrentTotalScore - mCurrentDisplayScore) > 100000)
                mCurrentDisplayScore += 10001;
            else if ((mCurrentTotalScore - mCurrentDisplayScore) > 10000)
                mCurrentDisplayScore += 1001;
            else if ((mCurrentTotalScore - mCurrentDisplayScore) > 1000)
                mCurrentDisplayScore += 101;
            else if ((mCurrentTotalScore - mCurrentDisplayScore) > 100)
                mCurrentDisplayScore += 11;
            else
                mCurrentDisplayScore++;

            if(mCurrentDisplayScore >= mCurrentTotalScore)
            {
                mUpdateDisplayScore = false;
                mCurrentDisplayScore = mCurrentTotalScore;
            }
        }
        ValueText.text = mCurrentDisplayScore.ToString();
    }

    /// <summary>
    /// When this method is called, it'll begin the score storing progress of the new landed block.
    /// This method is responsible to reset the store score variable and the combo count of this scoring round,
    /// and update the first score for the landing block
    /// This method will be subscrbing with the newBlockLandedScores delegate method in GamingManager
    /// </summary>
    private void NewLandedBlockScores()
    {
        mThisRoundGainedScore = 0;
        mComboCount = 0;
        int addsOnValue = ScoreCalculatorcs.OriginalBlockLandingScoreCalculation() * mCurrentBonusTerm;
        UpdateScore(addsOnValue, ScoreCalculatorcs.OriginalBlockLandingScoreCalculation());
    }

    /// <summary>
    /// This method is responsible to calculate and add linking score which is the number of cube that are
    /// been used to score multibly with the current bonus term to the total score, and add up the combo count
    /// This method will be subscrbing with the linkingCubeScores delegate method in GamingManager
    /// </summary>
    /// <param name="aCubeCount">The number of cubes that was use to make the score appear</param>
    private void LinkingCubeScores(int aCubeCount)
    {
        mComboCount++;
        powerUpCharging?.Invoke();

        int addsOnValue = aCubeCount * mCurrentBonusTerm;
        UpdateScore(addsOnValue, aCubeCount);
    }

    /// <summary>
    /// This method is responsible to calculate and add the combo score to the total score.
    /// The combo is occured for each scoring during the scoring progress of new landed block
    /// This method will be subscrbing with the comboScoring delegate method in GamingManager
    /// </summary>
    /// <param name="aComboCount"></param>
    private void ComboScoring(int aComboCount)
    {
        mComboCount++;
        if (aComboCount < 1)
            return;

        int comboScore = ScoreCalculatorcs.ComboScoreCalculation(aComboCount);
        int addsOnValue = comboScore * mCurrentBonusTerm;
        UpdateScore(addsOnValue, comboScore);
    }

    /// <summary>
    /// Change the sub issue value of the score gaine and combo made during the scoring progress
    /// of the new landed block.
    /// If either of them surpass the highest value of their respective high value holder, the highest
    /// value holder will store their surpassing value in them.
    /// </summary>
    private void SurpassCountCheck()
    {
        if (mThisRoundGainedScore > mHighestGainedRoundScore)
            mHighestGainedRoundScore = mThisRoundGainedScore;

        if (mComboCount > mBestComboCount)
            mBestComboCount = mComboCount;
    }
}
