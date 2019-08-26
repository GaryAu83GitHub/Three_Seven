using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskRank
{
    X1,
    X5,
    X10,
}

public class TaskManager
{
    public static TaskManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new TaskManager();
            return mInstance;
        }
    }
    private static TaskManager mInstance;

    //public delegate void OnAchieveObjective(TaskRank anObjective, int anObjectiveNumber);
    public delegate void OnAchieveObjective(TaskRank anObjective, TaskData aTaskData);
    public static OnAchieveObjective achieveObjective;

    private List<bool> mUsedTaskNumbers = new List<bool>();
    private List<int> mActiveLinkedCubes = new List<int>();
    private Dictionary<TaskRank, List<int>> mTaskNumbersList = new Dictionary<TaskRank, List<int>>();
    private Dictionary<TaskRank, TaskData> mActiveTasks = new Dictionary<TaskRank, TaskData>();
    private Dictionary<TaskRank, bool> mTaskAchieveList = new Dictionary<TaskRank, bool>();
    private Dictionary<int, List<int>> mLinkCubeTaskValueCountList = new Dictionary<int, List<int>>();

    private int mMaxLimitObjectiveValue = 18;
    private int mCurrentObjectiveValueLimit = 0;
    public int CurrentLimetObjectiveValue { get { return mCurrentObjectiveValueLimit; } }

    private readonly bool mDebugMode = true;
    
    public TaskManager()
    {
        mTaskNumbersList.Add(TaskRank.X1, new List<int>());
        mTaskNumbersList.Add(TaskRank.X5, new List<int>());
        mTaskNumbersList.Add(TaskRank.X10, new List<int>());
        
        foreach (TaskRank obj in mTaskNumbersList.Keys)
        {
            mTaskAchieveList.Add(obj, false);
            //mActiveTasks.Add(obj, 0);
            mActiveTasks.Add(obj, new TaskData());
        }

        mCurrentObjectiveValueLimit = 0;
    }

    public void StartFirstSetOfObjective()
    {
        if (mDebugMode)
        {
            //mActiveTasks[TaskRank.X1].SetValue(7, GetTaskedLinkedCubeCount());
            //mActiveTasks[TaskRank.X5].SetValue(14, GetTaskedLinkedCubeCount());
            //mActiveTasks[TaskRank.X10].SetValue(21, GetTaskedLinkedCubeCount());

            mActiveTasks[TaskRank.X1].SetValue(CreateNewTask(7));
            mActiveTasks[TaskRank.X5].SetValue(CreateNewTask(14));
            mActiveTasks[TaskRank.X10].SetValue(CreateNewTask(21));

            for (TaskRank r = TaskRank.X1; r != TaskRank.X10 + 1; r++)
                achieveObjective?.Invoke(r, mActiveTasks[r]);
        }
        else
        {
            foreach (TaskRank obj in mTaskNumbersList.Keys)
            {
                achieveObjective?.Invoke(obj, mActiveTasks[obj]);
            }
        }
    }

    public bool AchiveObjective(ref TaskRank getObjectiveRank, int aCheckingNumber, int aScoringCubeCount)
    {
        //if (!mActiveTasks.ContainsValue(aCheckingNumber))
        if(!TaskDataContainValue(aCheckingNumber))
            return false;

        getObjectiveRank = mActiveTasks.FirstOrDefault(x => x.Value.Number == aCheckingNumber).Key;

        if (mActiveTasks[getObjectiveRank].CubeCount != aScoringCubeCount)
            return false;

        //mUsedObjectiveNumbers[aCheckingNumber] = true;
        //mObjectiveAchieveList[getObjectiveRank] = true;
        return true;
    }

    public void ConfirmAchiveTaskOn(TaskRank aRank, int aTaskValue)
    {
        mTaskAchieveList[aRank] = true;
        //mUsedObjectiveNumbers[aTaskValue] = true;
    }

    public void ChangeObjective()
    {
        //if (mDebugMode)
        //    return;

        if (!mTaskAchieveList.ContainsValue(true))
            return;

        foreach (TaskRank obj in mTaskAchieveList.Keys.ToList())
        {
            if (mTaskAchieveList[obj])
            {
                mTaskAchieveList[obj] = false;
                //mActiveTasks[obj].SetValue(GetTaskValueFor(obj), GetTaskedLinkedCubeCount());
                mActiveTasks[obj].SetValue(CreateNewTask(GetTaskValueFor(obj)));
                achieveObjective?.Invoke(obj, mActiveTasks[obj]);

            }
        }
    }
    
    public void SetMaxLimitObjectiveValue(int aMaxValue)
    {
        mMaxLimitObjectiveValue = aMaxValue;
    }

    public void SetInitialObjectiveValue(int aStartValue)
    {
        mCurrentObjectiveValueLimit = aStartValue;
        GameSettings.Instance.SetInitialValue(mCurrentObjectiveValueLimit);
    }

    public void IncreaseObjectiveValue()
    {
        if ((mCurrentObjectiveValueLimit + 1) > mMaxLimitObjectiveValue)
            return;

        mCurrentObjectiveValueLimit++;
        mUsedTaskNumbers.Add(false);
    }

    public void PrepareObjectives()
    {
        
        Dictionary<int, int> combinationCount = IterateCombination();

        foreach(TaskRank k in mTaskNumbersList.Keys)
        {
            mTaskNumbersList[k].Clear();
        }
        mUsedTaskNumbers.Clear();
        
        for (int i = 0; i <= mCurrentObjectiveValueLimit; i++)
            mUsedTaskNumbers.Add(false);

        int mostCombination = combinationCount.Values.Max();
        foreach (int key in combinationCount.Keys)
        {
            if (mostCombination.ToString().Length == 4)
            {
                if (combinationCount[key].ToString().Length == 1 || combinationCount[key].ToString().Length == 2)
                    mTaskNumbersList[TaskRank.X10].Add(key);
                else if (combinationCount[key].ToString().Length == 3)
                    mTaskNumbersList[TaskRank.X5].Add(key);
                else if (combinationCount[key].ToString().Length == 4)
                    mTaskNumbersList[TaskRank.X1].Add(key);
            }
            else if (mostCombination.ToString().Length == 3)
            {
                if (combinationCount[key].ToString().Length == 1)
                    mTaskNumbersList[TaskRank.X10].Add(key);
                else if (combinationCount[key].ToString().Length == 2)
                    mTaskNumbersList[TaskRank.X5].Add(key);
                else if (combinationCount[key].ToString().Length == 3)
                    mTaskNumbersList[TaskRank.X1].Add(key);
            }
            else
            {
               if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_3_DIGIT))
                {
                    if (key <= 3 || key >= 24) // 1 to 3 combination
                        mTaskNumbersList[TaskRank.X10].Add(key);
                    else if ((key >= 4 && key <= 9) || (key >= 18 && key <= 23)) // 4 to 7 combination
                        mTaskNumbersList[TaskRank.X5].Add(key);
                    else if (key >= 10 && key <= 17)   // 
                        mTaskNumbersList[TaskRank.X1].Add(key);
                }
               else
                {
                    if (key <= 2 || key >= 16) // 1 to 3 combination
                        mTaskNumbersList[TaskRank.X10].Add(key);
                    else if ((key >= 3 && key <= 6) || (key >= 12 && key <= 15)) // 4 to 7 combination
                        mTaskNumbersList[TaskRank.X5].Add(key);
                    else if (key >= 7 && key <= 11)   // 
                        mTaskNumbersList[TaskRank.X1].Add(key);
                }
            }
        }

        foreach (TaskRank obj in mTaskNumbersList.Keys)
        {
            mTaskAchieveList[obj] = false;
            //mActiveTasks[obj].SetValue(GetTaskValueFor(obj), GetTaskedLinkedCubeCount());
            mActiveTasks[obj].SetValue(CreateNewTask(GetTaskValueFor(obj)));
            achieveObjective?.Invoke(obj, mActiveTasks[obj]);
        }
        return;
    }

    private List<int> ResetObjectiveNumberFor(TaskRank anObjective)
    {
        List<int> availableObjective = new List<int>();
        foreach (int i in mTaskNumbersList[anObjective])
        {
            if (i > mCurrentObjectiveValueLimit)
                break;
            mUsedTaskNumbers[i] = false;
            availableObjective.Add(i);
        }

        return availableObjective;
    }

    private int GetTaskValueFor(TaskRank anObjective)
    {
        if (mDebugMode)
            return mActiveTasks[anObjective].Number;

        List<int> avaiableObjective = new List<int>();
        
        for(int i = 0; i < mUsedTaskNumbers.Count; i++)
        {
            if (mTaskNumbersList[anObjective].Contains(i) && !mUsedTaskNumbers[i])
                avaiableObjective.Add(i);
        }

        if (!avaiableObjective.Any())
            avaiableObjective = ResetObjectiveNumberFor(anObjective);

        int availableCount = avaiableObjective.Count;
        int selectedIndex = Random.Range(0, availableCount);
        int selectedValue = avaiableObjective[selectedIndex];
        
        return selectedValue;
    }

    private int GetTaskedLinkedCubeCount()
    {
        return mActiveLinkedCubes[Random.Range(0, mActiveLinkedCubes.Count)];
    }

    private TaskData CreateNewTask(int aTaskValue)
    {
        List<int> availableLinkCube = new List<int>();

        foreach(int cc in mLinkCubeTaskValueCountList.Keys)
        {
            if (mLinkCubeTaskValueCountList[cc].Contains(aTaskValue))
                availableLinkCube.Add(cc);
        }

        int linkCount = availableLinkCube.Count;

        TaskData data = new TaskData(aTaskValue, availableLinkCube[Random.Range(0, linkCount)]);
        return data;
    }

    private bool TaskDataContainValue(int aValue)
    {
        foreach(TaskData d in mActiveTasks.Values)
        {
            if (d.Number == aValue)
                return true;
        }
        return false;
    }

    private Dictionary<int, int> IterateCombination()
    {
        Dictionary<int, int> combinationList = new Dictionary<int, int>();

        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_2_DIGIT))
        {   
            IterateTwoCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(2);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_3_DIGIT))
        {
            IterateThreeCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(3);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_4_DIGIT))
        {
            IterateFourCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(4);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_5_DIGIT))
        {
            IterateFiveCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(5);
        }

        return combinationList;
    }

    private void IterateTwoCubesCombination(ref Dictionary<int, int> someObjectives)
    {
        int sum = 0;
        mLinkCubeTaskValueCountList.Add(2, new List<int>());

        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                sum = a + b;

                if (!mLinkCubeTaskValueCountList[2].Contains(sum))
                    mLinkCubeTaskValueCountList[2].Add(sum);

                if (!someObjectives.ContainsKey(sum))
                    someObjectives.Add(sum, 0);

                someObjectives[sum]++;
            }
        }
    }

    private void IterateThreeCubesCombination(ref Dictionary<int, int> someObjectives)
    {
        int sum = 0;
        mLinkCubeTaskValueCountList.Add(3, new List<int>());

        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                for (int c = 0; c < 10; c++)
                {
                    sum = a + b + c;

                    if (!mLinkCubeTaskValueCountList[3].Contains(sum))
                        mLinkCubeTaskValueCountList[3].Add(sum);

                    if (!someObjectives.ContainsKey(sum))
                        someObjectives.Add(sum, 0);

                    someObjectives[sum]++;
                }
            }
        }
    }

    private void IterateFourCubesCombination(ref Dictionary<int, int> someObjectives)
    {
        int sum = 0;
        mLinkCubeTaskValueCountList.Add(4, new List<int>());

        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                for (int c = 0; c < 10; c++)
                {
                    for (int d = 0; d < 10; d++)
                    {
                        sum = a + b + c + d;

                        if (!mLinkCubeTaskValueCountList[4].Contains(sum))
                            mLinkCubeTaskValueCountList[4].Add(sum);

                        if (!someObjectives.ContainsKey(sum))
                            someObjectives.Add(sum, 0);

                        someObjectives[sum]++;
                    }
                }
            }
        }
    }

    private void IterateFiveCubesCombination(ref Dictionary<int, int> someObjectives)
    {
        int sum = 0;
        mLinkCubeTaskValueCountList.Add(5, new List<int>());

        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                for (int c = 0; c < 10; c++)
                {
                    for (int d = 0; d < 10; d++)
                    {
                        for (int e = 0; e < 10; e++)
                        {
                            sum = a + b + c + d + e;

                            if (!mLinkCubeTaskValueCountList[5].Contains(sum))
                                mLinkCubeTaskValueCountList[5].Add(sum);

                            if (!someObjectives.ContainsKey(sum))
                                someObjectives.Add(sum, 0);

                            someObjectives[sum]++;
                        }
                    }
                }
            }
        }
    }
}

public class TaskData
{
    public int Number { get { return mTaskNumber; } }
    private int mTaskNumber = 0;

    public int CubeCount { get { return mTaskCubeCount; } }
    private int mTaskCubeCount = 0;

    public TaskData()
    {
        mTaskNumber = 0;
        mTaskCubeCount = 0;
    }

    public TaskData(TaskData aData)
    {
        mTaskNumber = aData.Number;
        mTaskCubeCount = aData.CubeCount;
    }

    public TaskData(int aTaskNumber, int aTaskCubeCount)
    {
        mTaskNumber = aTaskNumber;
        mTaskCubeCount = aTaskCubeCount;
    }

    public void SetValue(TaskData aData)
    {
        mTaskNumber = aData.Number;
        mTaskCubeCount = aData.CubeCount;
    }

    public void SetValue(int aTaskNumber, int aTaskCubeCount)
    {
        mTaskNumber = aTaskNumber;
        mTaskCubeCount = aTaskCubeCount;
    }
}

