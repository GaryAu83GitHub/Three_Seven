using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultSlotBase : MonoBehaviour
{
    public TextMeshProUGUI IssueHeaderText;
    public TextMeshProUGUI ValueText;
    public TextMeshProUGUI RankText;
    public Image MedalImage;

    public List<Sprite> MedalSprites;
    public List<int> ThresholdValues;

    protected enum MedalRank
    {
        DIAMOND,
        PLATINUM,
        GOLD,
        SILVER,
        BRONZE,
        COPPER,
        NONE,
    };

    protected ResultIssues mResultIssue = ResultIssues.NONE;
    protected string mDisplayString = "";

    protected MedalRank mMedalRankIndex = MedalRank.NONE;

    protected Dictionary<MedalRank, int> mRankScoreThresholds = new Dictionary<MedalRank, int>();

    public virtual void Start()
    {
        for (int i = 0; i < ThresholdValues.Count; i++)
            mRankScoreThresholds.Add((MedalRank)i, ThresholdValues[i]);
    }

    public virtual void Update() { }

    protected virtual void SetupResult(ResultData aData) { }

    protected virtual MedalRank RankRecrusive(int aResultValue, MedalRank aRank = MedalRank.COPPER)
    {
        MedalRank currentRank = aRank;
        if (currentRank == MedalRank.DIAMOND)
            return currentRank;

        if (aResultValue < mRankScoreThresholds[currentRank])
            return currentRank;

        return RankRecrusive(aResultValue, currentRank - 1);
    }
    

    public void SetResultData(ResultData aData)
    {
        SetupResult(aData);

        DisplayValueText();
        DisplayMedalSprite();
    }

    public void SetValue(string aDisplayResult)
    {
        if(ValueText != null)
            ValueText.text = aDisplayResult;
    }

    protected void DisplayValueText()
    {
        if (ValueText != null)
            ValueText.text = mDisplayString;
    }

    protected void DisplayMedalSprite()
    {
        if (MedalImage != null && MedalSprites.Any() && mMedalRankIndex != MedalRank.NONE)
            MedalImage.sprite = MedalSprites[(int)mMedalRankIndex];
    }
}
