using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TaskData
{
    private int mTaskCountNumber = 0;
    public int TaskCountNumber { get { return mTaskCountNumber; } }

    private TaskRank mRank = TaskRank.X1;
    public TaskRank Rank { get { return mRank; } }

    private int mTaskValue = 0;
    public int TaskValue { get { return mTaskValue; } }

    private int mTaskLinkedCube = 0;
    public int LinkedCubes { get { return mTaskLinkedCube; } }

    private bool mTaskComplete = false;
    public bool TaskComplete { get { return mTaskComplete; } }

    public TaskData()
    {
        mTaskCountNumber = 0;
        mTaskValue = 0;
        mTaskLinkedCube = 0;
    }

    public TaskData(TaskData aData)
    {
        mTaskCountNumber = aData.TaskCountNumber;
        mRank = aData.Rank;
        mTaskValue = aData.TaskValue;
        mTaskLinkedCube = aData.LinkedCubes;
        mTaskComplete = false;
    }

    public TaskData(int aTaskValue, int aTaskLinkedCount)
    {
        mTaskLinkedCube = aTaskLinkedCount;
        mTaskValue = aTaskValue;
        mTaskComplete = false;
    }

    public TaskData(TaskRank aRank, int aTaskCubeCount, int aTaskNumber)
    {
        mRank = aRank;
        mTaskLinkedCube = aTaskCubeCount;
        mTaskValue = aTaskNumber;
        mTaskComplete = false;
    }

    public TaskData(int aTaskCount/*, TaskRank aRank*/, int aTaskCubeCount, int aTaskNumber)
    {
        mTaskCountNumber = aTaskCount;
        //mRank = aRank;
        mTaskLinkedCube = aTaskCubeCount;
        mTaskValue = aTaskNumber;
        mTaskComplete = false;
    }

    public bool Equals(TaskData obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            TaskData p = (TaskData)obj;
            if (p.TaskCountNumber != mTaskCountNumber)
                return false;
            if (p.TaskValue != mTaskValue)
                return false;
            if (p.LinkedCubes != mTaskLinkedCube)
                return false;
            if (p.TaskComplete != mTaskComplete)
                return false;
        }
        return true;
    }

    public void SetValue(TaskData aData)
    {
        mTaskCountNumber = aData.TaskCountNumber;
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

