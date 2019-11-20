using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplayBox : ScoreboardComponentBase
{
    public GameObject ScoreAddOn;

    private int mCurrentDisplayScore = 0;
    private int mCurrentTotalScore = 0;

    private bool mUpdateDisplayScore = false;

    public override void Start()
    {
        GameManager.scoreChanging += UpdateScore;
        //MainGamePanel.onAddScore += UpdateScore;
        UpdateDisplayScore();
    }

    private void OnDestroy()
    {
        GameManager.scoreChanging -= UpdateScore;
        //MainGamePanel.onAddScore -= UpdateScore;
    }
    
    protected override void ComponentsDisplay()
    {
        if (mUpdateDisplayScore)
            UpdateDisplayScore();
    }

    private void UpdateScore(int aNewTotalScore, int anAddOnScore)
    {
        mCurrentTotalScore = aNewTotalScore;

        ScoreAddOn.GetComponent<Text>().text = "+" + anAddOnScore.ToString();
        ScoreAddOn.GetComponent<Animation>().Play();

        mUpdateDisplayScore = true;
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
}
