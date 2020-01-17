using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreListObjectSlot : GuiSlotBase
{
    public TextMeshProUGUI RankText;
    public TextMeshProUGUI ChainText;
    public TextMeshProUGUI TaskText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI OddsText;
    public TextMeshProUGUI ScoreText;

    public List<Image> DigitImages;

    private bool mIsSelected = false;
    private string mDisplayName = "";

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public void SetSlotData(int aRankNumber, SavingResultData aData)
    {
        mDisplayName = aData.PlayerName;

        RankText.text = aRankNumber.ToString();
        TitleText.text = mDisplayName;
        ChainText.text = aData.LongestChains.ToString();
        TaskText.text = aData.CompletedTaskCount.ToString();
        LevelText.text = aData.GainedLevel.ToString();
        TimeText.text = TimeTool.TimeString(aData.PlayTime);
        OddsText.text = aData.AverageOdds.ToString();
        ScoreText.text = aData.TotalScores.ToString();
    }

    public void SetAsSelected(bool isSelected)
    {
        mIsSelected = isSelected;
    }
}
