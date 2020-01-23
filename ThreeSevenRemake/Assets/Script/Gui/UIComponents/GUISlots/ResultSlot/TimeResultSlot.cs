using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeResultSlot : ResultSlotBase
{
    public List<int> ClassicModeThresholdValues;

    public override void Awake()
    {
        GameSceneMain.setGameMode += SetGameMode;
        base.Awake();
    }

    public override void Start()
    {
        mResultIssue = ResultIssues.TIME;

        //GameSceneMain.setGameMode += SetGameMode;

        base.Start();
    }

    private void OnDestroy()
    {
        GameSceneMain.setGameMode -= SetGameMode;
    }

    public override void Update() { }

    protected override void SetupResult(ResultData aData)
    {
        mDisplayString = aData.TimeString;
        mMedalRankIndex = RankRecrusive(aData.PlayTime);
    }

    private void SetGameMode(GameMode aMode)
    {
        mRankScoreThresholds.Clear();
        if (aMode == GameMode.CLASSIC)
        {
            for (int i = 0; i < ClassicModeThresholdValues.Count; i++)
                mRankScoreThresholds.Add((MedalRank)i, ClassicModeThresholdValues[i]);
        }
        else
        {
            int timeLimit = GameSettings.Instance.TimeLimit;

            for (int i = 0; i < ThresholdValues.Count; i++)
                mRankScoreThresholds.Add((MedalRank)i, GetThresholdValueAt(ThresholdValues[i], timeLimit));
        }
    }

    private int GetThresholdValueAt(int aProcentValue, int aTimeLimitValue)
    {
        int result = aTimeLimitValue * (aProcentValue / 100);
        return result;
    }
}
