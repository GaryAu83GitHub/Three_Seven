using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSlot : GuiSlotBase
{
    public List<TaskFrame> TaskFrames;

    public delegate void OnPopupAppear(Vector3 aTargetPosition, List<Cube> someScoringCube, int aDisplayScore);
    public static OnPopupAppear popupAppear;

    public override void Start()
    {
        base.Start();

        BlockManager.achieveScoring += DisplayScoring;
        BlockManager.achieveScoringFor += DisplayScoringFor;
        TaskManagerNew.displayTaskAt += SetTaskNumbersAt;
        TaskManagerNew.displayTaskList += SetTaskNumbersByList;
    }

    private void OnDestroy()
    {
        BlockManager.achieveScoring -= DisplayScoring;
        BlockManager.achieveScoringFor -= DisplayScoringFor;
        TaskManagerNew.displayTaskAt -= SetTaskNumbersAt;
        TaskManagerNew.displayTaskList -= SetTaskNumbersByList;
    }

    public override void Update()
    {
        base.Update();
    }

    public void SetTaskNumbersByList(List<TaskData> someDatas)
    {
        for (int i = 0; i < someDatas.Count; i++)
            TaskFrames[i].SetUpTask(someDatas[i]);
    }

    public void SetTaskNumbersAt(int aTaskIndex, TaskData aData)
    {
        TaskFrames[aTaskIndex].SetUpTask(aData);
    }

    private void DisplayScoring(TaskRank anObjective, List<Cube> someScoringCube)
    {
        int totalScore = ScoreCalculatorcs.LinkingScoreCalculation(anObjective, someScoringCube.Count);

        Vector3 midPos = GetMidPointBetweenScoringCubes(someScoringCube[0].transform.position, someScoringCube[someScoringCube.Count - 1].transform.position);
        popupAppear?.Invoke(midPos, someScoringCube, totalScore);

        TaskFrames[(int)anObjective].DisplayScoring(totalScore, someScoringCube);
    }

    private void DisplayScoringFor(ScoringGroupAchieveInfo anInfo, List<Cube> someScoringCube)
    {
        int totalScore = ScoreCalculatorcs.LinkingScoreCalculation(anInfo.TaskRank, someScoringCube.Count);

        Vector3 midPos = GetMidPointBetweenScoringCubes(someScoringCube[0].transform.position, someScoringCube[someScoringCube.Count - 1].transform.position);
        popupAppear?.Invoke(midPos, someScoringCube, totalScore);

        TaskFrames[anInfo.TaskSlotIndex].DisplayScoring(totalScore, someScoringCube);
    }

    private Vector3 GetMidPointBetweenScoringCubes(Vector3 firstCubeWorldPos, Vector3 lastCubeWorldPos)
    {
        return Vector3.Lerp(firstCubeWorldPos, lastCubeWorldPos, .5f);
    }
}
