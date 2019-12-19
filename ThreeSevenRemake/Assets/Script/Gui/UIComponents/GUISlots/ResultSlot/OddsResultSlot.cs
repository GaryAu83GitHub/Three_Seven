using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OddsResultSlot : ResultSlotBase
{
    public override void Start()
    {
        mResultIssue = ResultIssues.ODDS;
        base.Start();
    }

    public override void Update() { }

    protected override void SetupResult(ResultData aData)
    {
        float displayProcent = (Mathf.Round(aData.AverageOdds * 1000) / 1000f);


        mDisplayString = (displayProcent * 100).ToString() + "%";
        mMedalRankIndex = RankRecrusive(aData.GainedLevelCount);
    }
}
