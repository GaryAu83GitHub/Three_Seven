using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum TaskRank
{
    X1,
    X5,
    X10
}

public class TaskManagerNew
{
    public static TaskManagerNew Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new TaskManagerNew();
            return mInstance;
        }
    }
    private static TaskManagerNew mInstance;

    public delegate void OnDisplayTaskAt(int aTaskIndex, TaskData aData);
    public static OnDisplayTaskAt displayTaskAt;

    public delegate void OnDisplayTaskList(List<TaskData> someDatas);
    public static OnDisplayTaskList displayTaskList;

    public delegate void OnTaskAccomplish(List<TaskData> someNewData);
    public static OnTaskAccomplish taskAccomplish;

    public delegate void OnCreateNewBlock();
    public static OnCreateNewBlock createNewBlock;

    private TaskSubject mSubject = new TaskSubject();

    private List<TaskData> mActiveTasks = new List<TaskData>();
    public List<TaskData> ActiveTask { get { return mActiveTasks; } }

    private int mScoringActiveTaskIndex = -1;
    private readonly bool mUnderDebug = false;

    public TaskManagerNew()
    {
        for (int i = 0; i < 3; i++)
            mActiveTasks.Add(new TaskData());
    }

    public void StartFirstSetOfTask()
    {
        displayTaskList?.Invoke(mActiveTasks);

        CubeNumberManager.Instance.GenerateNewUseableCubeNumberFor(mActiveTasks);
    }

    /// <summary>
    /// Check if the sending value and linked cube count match with any of the active task.
    /// If any match, i'll have the match data's rank send back as reference and stored the
    /// index of the mathcing data in the list
    /// </summary>
    /// <param name="retriveRank">Reference rank that send back</param>
    /// <param name="aValue">checking value</param>
    /// <param name="aLinkCubes">checking link count</param>
    /// <returns></returns>
    public bool MatchTaskData(ref TaskRank retriveRank, ref int slotIndex,  int aValue, int aLinkCubes)
    {
        for(int i = 0; i < mActiveTasks.Count; i++)
        {
            if (mActiveTasks[i].TaskValue == aValue && mActiveTasks[i].LinkedCubes == aLinkCubes)
            {
                mScoringActiveTaskIndex = slotIndex = i;
                retriveRank = mActiveTasks[i].Rank;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// This purpose is for setting the task has been complete after it has pass both checking
    /// method of MatchTaskData in this class and ThisGroupHasNotbeenRegistrate from GridManager
    /// with the index value that was stored from MatchTaskData and call the TaskCompleted method
    /// of the TaskData of the item of the stored index in the list
    /// </summary>
    public void ConfirmAchiveTask()
    {
        if (mScoringActiveTaskIndex < 0 || mScoringActiveTaskIndex >= mActiveTasks.Count)
            return;

        mActiveTasks[mScoringActiveTaskIndex].TaskCompleted();
    }

    public void ChangeTask()
    {
        if (!mActiveTasks.Any(x => x.TaskComplete == true))
        {
            createNewBlock?.Invoke();
            return;
        }

        for(int i = 0; i < mActiveTasks.Count; i++)
        {
            if (mActiveTasks[i].TaskComplete)
            {
                SetNewActiveTaskFor(i);
            }
        }
        CubeNumberManager.Instance.GenerateNewUseableCubeNumberFor(mActiveTasks);
        taskAccomplish?.Invoke(mActiveTasks);
    }

    public void PrepareNewTaskSubjects()
    {
        mSubject = new TaskSubject();

        if (GameSettings.Instance.IsScoringMethodActiveTo(LinkIndexes.LINK_2_DIGIT))
        {
            TaskSubjectObject data = new TaskSubjectObject(2);
            mSubject.FillValueListFor(data);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(LinkIndexes.LINK_3_DIGIT))
        {
            TaskSubjectObject data = new TaskSubjectObject(3);
            mSubject.FillValueListFor(data);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(LinkIndexes.LINK_4_DIGIT))
        {
            TaskSubjectObject data = new TaskSubjectObject(4);
            mSubject.FillValueListFor(data);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(LinkIndexes.LINK_5_DIGIT))
        {
            TaskSubjectObject data = new TaskSubjectObject(5);
            mSubject.FillValueListFor(data);
        }

        if (!mUnderDebug)
        {
            for (int i = 0; i < mActiveTasks.Count; i++)
            {
                SetNewActiveTaskFor(i);
            }
        }
        else
        {
            SetNewActiveTaskFor(0, 2, 1);
            SetNewActiveTaskFor(1, 2, 8);
            SetNewActiveTaskFor(2, 2, 0);
        }
        CubeNumberManager.Instance.GenerateNewUseableCubeNumberFor(mActiveTasks);
    }

    private void SetNewActiveTaskFor(int anIndex)
    {
        mActiveTasks[anIndex].SetValue(mSubject.CreateTask());
        //displayTaskAt?.Invoke(anIndex, mActiveTasks[anIndex]);   
    }

    private void SetNewActiveTaskFor(int anIndex, int aLink, int aValue)
    {
        mActiveTasks[anIndex].SetValue(mSubject.CreateTask(aLink, aValue));
        displayTaskAt?.Invoke(anIndex, mActiveTasks[anIndex]);
    }
}

public class TaskSubject
{
    public TaskRank Rank { get { return mRank; } }
    private readonly TaskRank mRank = TaskRank.X1;

    private Dictionary<int, bool> mUsedLinks = new Dictionary<int, bool>();
    private Dictionary<int, List<int>> mTaskValueLists = new Dictionary<int, List<int>>();
    private Dictionary<int, List<bool>> mUsedTaskNumbers = new Dictionary<int, List<bool>>();

    private List<int> mAvailableLinks = new List<int>();
    private Dictionary<int, TaskSubjectObject> mSubjectData = new Dictionary<int, TaskSubjectObject>();

    private int mTaskValueLimit = 0;
    private int mMaxValue = 0;
    private int mCreatedTaskCount = 0;

    private readonly bool mUnderDebuging = false;
    private readonly int mDebugingValue = 0;
    private readonly int mDebugingLink = 0;


    /// <summary>
    /// Default constructor
    /// </summary>
    public TaskSubject()
    {

    }

    /// <summary>
    /// Previous Default constructor
    /// </summary>
    /// <param name="aRank">Task subjects rank</param>
    /// <param name="aInitialTaskValueLimit">the start task value</param>
    public TaskSubject(TaskRank aRank, int aInitialTaskValueLimit)
    {
        mRank = aRank;
        mTaskValueLimit = aInitialTaskValueLimit;
    }

    /// <summary>
    /// This constructor is called when an Tasksubject is used for debug purpose, with this set the value and link will not change during the game progress
    /// </summary>
    /// <param name="aRank">The task rank that want to be use for debug</param>
    /// <param name="useDebuging">The link use for debug</param>
    /// <param name="debugingValue">The value use for debug</param>
    public TaskSubject(TaskRank aRank, int debugingValue, int debugingLink)
    {
        mRank = aRank;
        mTaskValueLimit = Constants.MINIMAL_TASK_VALUE;

        mDebugingLink = debugingLink;
        mDebugingValue = debugingValue;
        mUnderDebuging = true;
    }

    public void FillValueListFor(TaskSubjectObject aData)
    {
        mAvailableLinks.Add(aData.LinkCubes);
        mSubjectData.Add(aData.LinkCubes, aData);

        //mUsedLinks.Add(aData.LinkCubes, false);

        //if(!mUsedTaskNumbers.ContainsKey(aData.LinkCubes) && !mTaskValueLists.ContainsKey(aData.LinkCubes))
        //{
        //    mUsedTaskNumbers.Add(aData.LinkCubes, new List<bool>());
        //    mTaskValueLists.Add(aData.LinkCubes, new List<int>());
        //}

        //List<int> valueList = aData.GetNumberListForRank(mRank);

        //for(int i = 0; i < valueList.Count; i++)
        //{
        //    mTaskValueLists[aData.LinkCubes].Add(valueList[i]);
        //    if (valueList[i] <= mTaskValueLimit)
        //        mUsedTaskNumbers[aData.LinkCubes].Add(false);
        //}

        //mMaxValue = valueList.Last();
        return;
    }

    public TaskData CreateTask()
    {
        int link = mAvailableLinks[Random.Range(0, mAvailableLinks.Count)];
        int task = GetNewTaskValue(link);
        TaskRank rank = mSubjectData[link].GetRankFor(task);
        mCreatedTaskCount++;

        //return new TaskData(rank, link, task);
        return new TaskData(mCreatedTaskCount, rank, link, task);
    }

    public TaskData CreateTask(int aLink, int aTaskValue)
    {
        TaskRank rank = mSubjectData[aLink].GetRankFor(aTaskValue);
        mCreatedTaskCount++;

        //return new TaskData(rank, aLink, aTaskValue);
        return new TaskData(mCreatedTaskCount, rank, aLink, aTaskValue);
    }

    public TaskData CreateNewTask()
    {
        int linkCount = GetCubesLink();
        int taskValue = GetTaskValue(linkCount);

        if (mUnderDebuging)
        {
            linkCount = mDebugingLink;
            taskValue = mDebugingValue;
        }

        //if (mCombinationCountList[aTaskValue] > 500)
        //    return TaskRank.X10;
        //else if (mCombinationCountList[aTaskValue] > 50 && mCombinationCountList[aTaskValue] < 500)
        //    return TaskRank.X5;

        //return TaskRank.X1;

        return new TaskData(mRank, linkCount, taskValue);
    }

    public void IncreaseTaskValueLimit()
    {
        if ((mTaskValueLimit + 1) > mMaxValue)
            return;

        mTaskValueLimit++;
        foreach (int key in mUsedTaskNumbers.Keys)
            mUsedTaskNumbers[key].Add(false);
    }

    public bool Contains(int aLinkCount, int aValue)
    {
        return mTaskValueLists[aLinkCount].Contains(aValue);
    }

    private int GetCubesLink()
    {
        if (mUsedLinks.Count < 2)
            return mUsedLinks.First().Key;

        int selectedLinkKey = 0;

        List<int> keyList = new List<int>();
        foreach (int sl in mUsedLinks.Keys)
        {
            if (!mUsedLinks[sl])
                keyList.Add(sl);
        }

        if (keyList.Count == 1)
        {
            selectedLinkKey = keyList[0];
            ResetUsedScoreLinks();
        }
        else
        {
            int availableCount = keyList.Count;
            selectedLinkKey = keyList[Random.Range(0, availableCount)];
        }

        mUsedLinks[selectedLinkKey] = true;

        return selectedLinkKey;
    }

    private void ResetUsedScoreLinks()
    {
        List<int> keys = new List<int>(mUsedLinks.Keys);

        foreach (int key in keys)
            mUsedLinks[key] = false;
    }

    private int GetNewTaskValue(int aLink)
    {
        List<int> availableValues = mSubjectData[aLink].GetUnusedTaskValue();

        if (availableValues.Count == 1)
        {
            mSubjectData[aLink].ResetUsedTaskValue(availableValues[0]);
            return availableValues[0];
        }

        int selectedValue = availableValues[Random.Range(0, availableValues.Count)];

        return selectedValue;
    }

    // this will be replaced by GetNewTaskValue
    private int GetTaskValue(int aLinkKey)
    {
        int selectedIndex = 0;

        List<int> indexList = new List<int>();
        for (int i = 0; i < mUsedTaskNumbers[aLinkKey].Count; i++)
        {
            if (!mUsedTaskNumbers[aLinkKey][i])
                indexList.Add(i);
        }

        if (indexList.Count == 1)
        {
            selectedIndex = indexList[0];
            ResetTaskValues(aLinkKey);
        }
        else
        {
            int availableCount = indexList.Count;
            selectedIndex = indexList[Random.Range(0, availableCount)];
        }

        mUsedTaskNumbers[aLinkKey][selectedIndex] = true;

        if (selectedIndex < 0 || selectedIndex >= mTaskValueLists[aLinkKey].Count)
        {
            return mTaskValueLists[aLinkKey][0];
        }

        return mTaskValueLists[aLinkKey][selectedIndex];
    }

    private void ResetTaskValues(int aLinkKey)
    {
        for (int i = 0; i < mUsedTaskNumbers[aLinkKey].Count; i++)
            mUsedTaskNumbers[aLinkKey][i] = false;
    }
}
