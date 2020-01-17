using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResultSlot : ResultSlotBase
{
    public override void Awake() { base.Awake(); }

    public override void Start()
    {
        mResultIssue = ResultIssues.LEVEL;
        base.Start();
    }

    public override void Update() { }

    protected override void SetupResult(ResultData aData)
    {
        mDisplayString = aData.GainedLevelCount.ToString();
        mMedalRankIndex = RankRecrusive(aData.GainedLevelCount);
    }
}
