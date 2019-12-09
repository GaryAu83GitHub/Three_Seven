using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplayBox : ScoreboardComponentBase
{
    public GameObject ScoreAddOn;
    public TextMeshProUGUI BonusText;

    private int mCurrentDisplayScore = 0;
    private int mCurrentTotalScore = 0;

    private bool mUpdateDisplayScore = false;

    private int mScoreMultiplyTerm = 1;

    public override void Start()
    {
        base.Start();

        GamingManager.comboScoring += ComboScoring;
        GamingManager.linkingCubeScores += LinkingCubeScores;
        GamingManager.newBlockLandedScores += NewLandedBlockScores;

        ComboDisplayBox.changeBonusTerm += ChangeBonusTerm;
        //MainGamePanel.onAddScore += UpdateScore;
        UpdateDisplayScore();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        GamingManager.comboScoring -= ComboScoring;
        GamingManager.linkingCubeScores -= LinkingCubeScores;
        GamingManager.newBlockLandedScores -= NewLandedBlockScores;

        ComboDisplayBox.changeBonusTerm -= ChangeBonusTerm;
        //MainGamePanel.onAddScore -= UpdateScore;
    }
    
    protected override void ComponentsDisplay()
    {
        if (mUpdateDisplayScore)
            UpdateDisplayScore();
    }

    protected override void GatherResultData(ref ResultData aData)
    {
        aData.SetGainScores(mCurrentTotalScore);
    }

    private void UpdateScore(/*int aNewTotalScore, */int anAddOnScore)
    {
        //mCurrentTotalScore = aNewTotalScore;

        ScoreAddOn.GetComponent<Text>().text = "+" + anAddOnScore.ToString();
        ScoreAddOn.GetComponent<Animation>().Play();

        mUpdateDisplayScore = true;
    }

    private void ChangeBonusTerm(int aNewBonusTerms)
    {
        mScoreMultiplyTerm = aNewBonusTerms;
    }

    private void UpdateDisplayScore()
    {
        if(mCurrentDisplayScore < mCurrentTotalScore)
        {
            if ((mCurrentTotalScore - mCurrentDisplayScore) > 1000)
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

    private void NewLandedBlockScores()
    {
        int addsOnValue = ScoreCalculatorcs.OriginalBlockLandingScoreCalculation() * mScoreMultiplyTerm;
        mCurrentTotalScore = addsOnValue;
        UpdateScore(addsOnValue);
    }

    private void LinkingCubeScores(int aCubeCount)
    {
        //ScoreCalculatorcs.LinkingScoreCalculation(anObjective, anValue);
        int addsOnValue = aCubeCount * mScoreMultiplyTerm;
        mCurrentTotalScore = addsOnValue;
        UpdateScore(addsOnValue);
    }

    private void ComboScoring(int aComboCount)
    {
        int addsOnValue = ScoreCalculatorcs.ComboScoreCalculation(aComboCount) * mScoreMultiplyTerm;
        mCurrentTotalScore = addsOnValue;
        UpdateScore(addsOnValue);
    }
}
