using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TaskData
{
    public TaskRank Rank { get { return mRank; } }
    private TaskRank mRank = TaskRank.X1;

    public int TaskValue { get { return mTaskValue; } }
    private int mTaskValue = 0;

    public int LinkedCubes { get { return mTaskLinkedCube; } }
    private int mTaskLinkedCube = 0;

    public bool TaskComplete { get { return mTaskComplete; } }
    private bool mTaskComplete = false;

    public TaskData()
    {
        mTaskValue = 0;
        mTaskLinkedCube = 0;
    }

    public TaskData(TaskData aData)
    {
        mRank = aData.Rank;
        mTaskValue = aData.TaskValue;
        mTaskLinkedCube = aData.LinkedCubes;
        mTaskComplete = false;
    }

    public TaskData(TaskRank aRank, int aTaskCubeCount, int aTaskNumber)
    {
        mRank = aRank;
        mTaskLinkedCube = aTaskCubeCount;
        mTaskValue = aTaskNumber;
        mTaskComplete = false;
    }

    public void SetValue(TaskData aData)
    {
        mRank = aData.Rank;
        mTaskValue = aData.TaskValue;
        mTaskLinkedCube = aData.LinkedCubes;
        mTaskComplete = false;
    }

    public bool IsMatchingData(int aTaskNumber, int aLinkCube)
    {
        if (mTaskValue == aTaskNumber && mTaskLinkedCube == aLinkCube)
            return true;

        return false;
    }

    public void TaskCompleted()
    {
        mTaskComplete = true;
    }
}

