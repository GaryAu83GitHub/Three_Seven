using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainResultSlot : ResultSlotBase
{
    public override void Start()
    {
        mResultIssue = ResultIssues.CHAIN;
        base.Start();
    }

    public override void Update() { }

    protected override void SetupResult(ResultData aData)
    {
        mDisplayString = aData.LongestChains.ToString();
        mMedalRankIndex = RankRecrusive(aData.LongestChains);
    }
}
