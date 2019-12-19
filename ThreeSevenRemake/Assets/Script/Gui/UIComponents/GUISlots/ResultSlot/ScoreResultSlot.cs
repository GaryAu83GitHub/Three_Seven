using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreResultSlot : ResultSlotBase
{
    public override void Start()
    {
        mResultIssue = ResultIssues.SCORE;
        base.Start();
    }

    public override void Update() { }

    protected override void SetupResult(ResultData aData)
    {
        mDisplayString = aData.TotalScores.ToString();
        mMedalRankIndex = RankRecrusive(aData.TotalScores);
    }
}
