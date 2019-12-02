using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSlot : GuiSlotBase
{
    //public List<TaskFrame> TaskFrames;
    public List<TaskBox> TaskBoxes;

    public delegate void OnPopupAppear(Vector3 aTargetPosition, List<Cube> someScoringCube, int aDisplayScore);
    public static OnPopupAppear popupAppear;

    private int mCompletedTaskCount = 0;

    public override void Start()
    {
        base.Start();

        BlockManager.achieveScoring += DisplayScoring;
        BlockManager.achieveScoringFor += DisplayScoringFor;
        TaskManagerNew.displayTaskAt += SetTaskNumbersAt;
        TaskManagerNew.displayTaskList += SetTaskNumbersByList;

        TaskManagerNew.taskAccomplish += TaskAccomplish;

        MainGamePanel.gatherResultData += GatherResultData;
    }

    private void OnDestroy()
    {
        BlockManager.achieveScoring -= DisplayScoring;
        BlockManager.achieveScoringFor -= DisplayScoringFor;
        TaskManagerNew.displayTaskAt -= SetTaskNumbersAt;
        TaskManagerNew.displayTaskList -= SetTaskNumbersByList;

        TaskManagerNew.taskAccomplish -= TaskAccomplish;

        MainGamePanel.gatherResultData -= GatherResultData;
    }

    public override void Update()
    {
        base.Update();
        //if (Input.GetKeyDown(KeyCode.DownArrow) && taskboxIndex < TaskBoxes.Count)
        //    taskboxIndex++;
        //if (Input.GetKeyDown(KeyCode.UpArrow) && taskboxIndex >= 0)
        //    taskboxIndex--;

        //if (Input.GetKeyDown(KeyCode.Space))
        //    TaskBoxes[taskboxIndex].PlayScoringAnimation();
        //if (Input.GetKeyDown(KeyCode.Return))
        //    TaskBoxes[taskboxIndex].PlayAccomplishAnimaiton();
    }

    public void SetTaskNumbersByList(List<TaskData> someDatas)
    {
        for (int i = 0; i < someDatas.Count; i++)
        {
            TaskBoxes[i].SetUpTask(someDatas[i]);// TaskFrames[i].SetUpTask(someDatas[i]);
            //TaskBoxes[i].gameObject.SetActive(true);
        }
    }

    public void TaskAccomplish(List<TaskData> someDatas)
    {
        for(int i = 0; i < someDatas.Count; i++)
        {
            if (TaskBoxes[i].TaskAccomplished)
            {
                TaskBoxes[i].AssignNewTaskData(someDatas[i]);
                mCompletedTaskCount++;
            }
        }
    }

    public void SetTaskNumbersAt(int aTaskIndex, TaskData aData)
    {
        //TaskFrames[aTaskIndex].SetUpTask(aData);
        TaskBoxes[aTaskIndex].SetUpTask(aData);
    }

    private void DisplayScoring(TaskRank anObjective, List<Cube> someScoringCube)
    {
        int totalScore = ScoreCalculatorcs.LinkingScoreCalculation(anObjective, someScoringCube.Count);

        Vector3 midPos = GetMidPointBetweenScoringCubes(someScoringCube[0].transform.position, someScoringCube[someScoringCube.Count - 1].transform.position);
        popupAppear?.Invoke(midPos, someScoringCube, totalScore);

        //TaskFrames[(int)anObjective].DisplayScoring(totalScore, someScoringCube);
        TaskBoxes[(int)anObjective].DisplayScoring(totalScore, someScoringCube);
    }

    private void DisplayScoringFor(ScoringGroupAchieveInfo anInfo, List<Cube> someScoringCube)
    {
        int totalScore = ScoreCalculatorcs.LinkingScoreCalculation(anInfo.TaskRank, someScoringCube.Count);

        Vector3 midPos = GetMidPointBetweenScoringCubes(someScoringCube[0].transform.position, someScoringCube[someScoringCube.Count - 1].transform.position);
        popupAppear?.Invoke(midPos, someScoringCube, totalScore);

        //TaskFrames[anInfo.TaskSlotIndex].DisplayScoring(totalScore, someScoringCube);
        TaskBoxes[anInfo.TaskSlotIndex].DisplayScoring(totalScore, someScoringCube);
    }

    private Vector3 GetMidPointBetweenScoringCubes(Vector3 firstCubeWorldPos, Vector3 lastCubeWorldPos)
    {
        return Vector3.Lerp(firstCubeWorldPos, lastCubeWorldPos, .5f);
    }

    private void GatherResultData(ref ResultData aData)
    {
        aData.SetCompletedTasks(mCompletedTaskCount);
    }
}
