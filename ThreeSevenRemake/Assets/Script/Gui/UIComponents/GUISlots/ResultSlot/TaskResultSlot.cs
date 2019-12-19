using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskResultSlot : ResultSlotBase
{
    public override void Start()
    {
        mResultIssue = ResultIssues.TASKS;
        base.Start();
    }

    public override void Update() { }

    protected override void SetupResult(ResultData aData)
    {
        mDisplayString = aData.TotalCompletedTaskCount.ToString();
        mMedalRankIndex = RankRecrusive(aData.TotalCompletedTaskCount);
    }
}
