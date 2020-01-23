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

    public Sprite HighlightSprite;
    public Sprite DefaultSprite;

    private Image mBG;

    private bool mIsSelected = false;
    private string mDisplayName = "";

    public override void Awake()
    {
        base.Awake();
        mBG = GetComponent<Image>();
        mBG.sprite = DefaultSprite;
    }

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
        float oddsProcent = (Mathf.Round(aData.AverageOdds * 1000) / 1000f);
        OddsText.text = (oddsProcent * 100).ToString(); //aData.AverageOdds.ToString();
        ScoreText.text = aData.TotalScores.ToString();

        DigitImages[0].gameObject.SetActive(aData.EnableDigit2);
        DigitImages[1].gameObject.SetActive(aData.EnableDigit3);
        DigitImages[2].gameObject.SetActive(aData.EnableDigit4);
        DigitImages[3].gameObject.SetActive(aData.EnableDigit5);
    }

    public void SetAsSelected(bool isSelected)
    {
        mIsSelected = isSelected;
        mBG.sprite = DefaultSprite;

        if (mIsSelected)
            mBG.sprite = HighlightSprite;        
    }

    public void SlotVisible(bool isVisible)
    {
        mCG.alpha = 1f;

        if (!isVisible)
            mCG.alpha = 0f;
    }
}
