using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is attached to the component of update and display the active tasks in
/// the task list.
/// It is responsible to count the number of completed task to the Result Data.
/// The sub issue for the result data this class has is
/// -> Highest count of complete task during the last round (max 3 since each round
/// only have 3 active task)
/// -> Highest count a single task had scored during the last round
/// </summary>
public class TaskSlot : GuiSlotBase
{
    //public List<TaskFrame> TaskFrames;
    public List<TaskBox> TaskBoxes;

    public delegate void OnPopupAppear(Vector3 aTargetPosition, List<Cube> someScoringCube, int aDisplayScore);
    public static OnPopupAppear popupAppear;

    /// <summary>
    /// Counting to total completed task
    /// This will be set to Result Data
    /// </summary>
    private int mCompletedTaskCount = 0;

    /// <summary>
    /// Counting on number of completed task during the last round.
    /// Always reset before the iterator of the tasklist
    /// </summary>
    private int mRoundCompletedTaskCount = 0;

    /// <summary>
    /// Storing the highest count of number of task (max 3) the last round has made
    /// This will be set to Result Data
    /// </summary>
    private int mHighestRoundCompleteTaskCount = 0;

    /// <summary>
    /// Storing the highest count a single task has scored during the last round
    /// This will be set to Result Data
    /// </summary>
    private int mHighestSingleTaskScoringCount = 0;

    public override void Awake()
    {
        BlockManager.achieveScoring += DisplayScoring;
        BlockManager.achieveScoringFor += DisplayScoringFor;
        TaskManagerNew.displayTaskAt += SetTaskNumbersAt;
        TaskManagerNew.displayTaskList += SetTaskNumbersByList;

        TaskManagerNew.taskAccomplish += TaskAccomplish;

        MainGamePanel.gatherResultData += GatherResultData;
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        //BlockManager.achieveScoring += DisplayScoring;
        //BlockManager.achieveScoringFor += DisplayScoringFor;
        //TaskManagerNew.displayTaskAt += SetTaskNumbersAt;
        //TaskManagerNew.displayTaskList += SetTaskNumbersByList;

        //TaskManagerNew.taskAccomplish += TaskAccomplish;

        //MainGamePanel.gatherResultData += GatherResultData;
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
        mRoundCompletedTaskCount = 0;

        for (int i = 0; i < someDatas.Count; i++)
        {
            if (TaskBoxes[i].TaskAccomplished)
            {
                if (TaskBoxes[i].ScoringTimes > mHighestSingleTaskScoringCount)
                    mHighestSingleTaskScoringCount = TaskBoxes[i].ScoringTimes;

                mCompletedTaskCount++;
                mRoundCompletedTaskCount++;

                TaskBoxes[i].AssignNewTaskData(someDatas[i]);
            }
        }

        if (mRoundCompletedTaskCount > mHighestRoundCompleteTaskCount)
            mHighestRoundCompleteTaskCount = mRoundCompletedTaskCount;
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
        aData.SetCompletedTasks(mCompletedTaskCount, mHighestRoundCompleteTaskCount, mHighestSingleTaskScoringCount);
    }
}
