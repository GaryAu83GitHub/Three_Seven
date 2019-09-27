using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    private TaskSubject mSubject = new TaskSubject();
    private List<TaskData> mActiveTasks = new List<TaskData>();

    private int mScoringActiveTaskIndex = -1;

    public TaskManagerNew()
    {
        for (int i = 0; i < 3; i++)
            mActiveTasks.Add(new TaskData());
    }

    public void StartFirstSetOfTask()
    {
        displayTaskList?.Invoke(mActiveTasks);

        //CubeNumberManager.Instance.GenerateNewCubeNumberOdds(mActiveTasks);
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
            return;

        for(int i = 0; i < mActiveTasks.Count; i++)
        {
            if (mActiveTasks[i].TaskComplete)
            {
                SetNewActiveTaskFor(i);
            }
        }
    }

    public void PrepareNewTaskSubjects()
    {
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

        for (int i = 0; i < mActiveTasks.Count; i++)
        {
            SetNewActiveTaskFor(i);
        }

        //CubeNumberManager.Instance.GenerateNewCubeNumberOdds(mActiveTasks);
        CubeNumberManager.Instance.GenerateNewUseableCubeNumberFor(mActiveTasks);
    }

    private void SetNewActiveTaskFor(int anIndex)
    {
        mActiveTasks[anIndex].SetValue(mSubject.CreateTask());
        displayTaskAt?.Invoke(anIndex, mActiveTasks[anIndex]);   
    }
}
