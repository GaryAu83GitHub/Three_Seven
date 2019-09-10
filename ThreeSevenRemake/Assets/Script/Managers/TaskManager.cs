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
    private Dictionary<TaskRank, bool> mTaskAchieveList = new Dictionary<TaskRank, bool>();
    private Dictionary<int, List<int>> mLinkCubeTaskValueCountList = new Dictionary<int, List<int>>();

    // these two dictionary will replace the two lists and three dictionary in the future
    private Dictionary<TaskRank, TaskSubject> mTaskSubjects = new Dictionary<TaskRank, TaskSubject>();
    private Dictionary<TaskRank, TaskData> mActiveTasks = new Dictionary<TaskRank, TaskData>();

    private int mMaxLimitObjectiveValue = 18;
    private int mCurrentObjectiveValueLimit = 0;
    public int CurrentLimetObjectiveValue { get { return mCurrentObjectiveValueLimit; } }

    private readonly bool mDebugMode = false;
    
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
            mActiveTasks[TaskRank.X1].SetValue(CreateNewTask(TaskRank.X1, 9));
            mActiveTasks[TaskRank.X5].SetValue(CreateNewTask(TaskRank.X5, 13));
            mActiveTasks[TaskRank.X10].SetValue(CreateNewTask(TaskRank.X10, 16));

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

    public bool MatchTaskData(ref TaskRank retriveRank, int aValue, int aLinkCubes)
    {
        foreach (TaskData d in mActiveTasks.Values)
        {
            if (d.Number == aValue && d.LinkedCubes == aLinkCubes)
            {
                retriveRank = d.Rank;
                return true;
            }
        }
        return false;
    }

    // this will be replaced by MatchTaskData
    public bool AchiveObjective(ref TaskRank getObjectiveRank, int aCheckingNumber, int aScoringCubeCount)
    {
        //if(!TaskDataContainValue(ref getObjectiveRank, aCheckingNumber))
        //    return false;

        //getObjectiveRank = mActiveTasks.FirstOrDefault(x => x.Value.Number == aCheckingNumber).Key;

        if (mActiveTasks[getObjectiveRank].LinkedCubes != aScoringCubeCount)
            return false;

        return true;
    }

    public void ConfirmAchiveTaskOn(TaskRank aRank)
    {
        // this line will be removed and the line below will take over the task of this method
        mTaskAchieveList[aRank] = true;

        mActiveTasks[aRank].TaskCompleted();
    }

    // this will be replaced by ChangeTask()
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
                mActiveTasks[obj].SetValue(CreateNewTask(obj, GetTaskValueFor(obj)));
                achieveObjective?.Invoke(obj, mActiveTasks[obj]);

            }
        }
    }

    public void ChangeTask()
    {
        if (!mActiveTasks.Any(x => x.Value.TaskComplete == true))
            return;

        foreach (TaskRank obj in mActiveTasks.Keys)
        {
            if (mActiveTasks[obj].TaskComplete)
            {
                mActiveTasks[obj].SetValue(mTaskSubjects[obj].CreateNewTask());
                achieveObjective?.Invoke(obj, mActiveTasks[obj]);
            }
        }
    }

    // this will be removed and will be handle in the TaskSubject class, it won't be affecting the GUI of setting the initialStartValue
    public void SetMaxLimitObjectiveValue(int aMaxValue)
    {
        mMaxLimitObjectiveValue = aMaxValue;
    }

    // this will be removed and it responsible will be handle by PrepareTask instead
    public void SetInitialObjectiveValue(int aStartValue)
    {
        mCurrentObjectiveValueLimit = aStartValue;
        GameSettings.Instance.SetInitialValue(mCurrentObjectiveValueLimit);
    }

    // this shall be replaced by ExpandTaskValue
    public void IncreaseObjectiveValue()
    {
        if ((mCurrentObjectiveValueLimit + 1) > mMaxLimitObjectiveValue)
            return;

        mCurrentObjectiveValueLimit++;
        mUsedTaskNumbers.Add(false);
    }

    public void ExpandTaskValue()
    {
        foreach (TaskRank rank in mTaskSubjects.Keys)
            mTaskSubjects[rank].IncreaseTaskValueLimit();
    }

    // this shall be replaced by PrepareTaskValues
    public void PrepareObjectives()
    {
        
        Dictionary<int, int> combinationCount = IterateCombination();

        // all this below can be removed when the factoricing is finish and confirm working

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
            mActiveTasks[obj].SetValue(CreateNewTask(obj, GetTaskValueFor(obj)));
            achieveObjective?.Invoke(obj, mActiveTasks[obj]);
        }
        return;
    }

    public void PrepareTaskValues(int aStartValue)
    {
        GameSettings.Instance.SetInitialValue(aStartValue);
        mTaskSubjects.Clear();

        mTaskSubjects.Add(TaskRank.X1, new TaskSubject(TaskRank.X1, aStartValue));
        mTaskSubjects.Add(TaskRank.X5, new TaskSubject(TaskRank.X5, aStartValue));
        mTaskSubjects.Add(TaskRank.X10, new TaskSubject(TaskRank.X10, aStartValue));

        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_2_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(2, 0.035f, 0.065f);
            FillTaskSubjects(data);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_3_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(3, 0.013f, 0.04f);
            FillTaskSubjects(data);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_4_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(4, 0.007f, 0.037f);
            FillTaskSubjects(data);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_5_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(5, 0.0028f, 0.025f);
            FillTaskSubjects(data);
        }

        foreach (TaskRank obj in mTaskNumbersList.Keys)
        {
            mActiveTasks[obj].SetValue(mTaskSubjects[obj].CreateNewTask());
            achieveObjective?.Invoke(obj, mActiveTasks[obj]);
        }
    }

    

    private void FillTaskSubjects(TaskRankValueData aData)
    {
        foreach (TaskRank key in mTaskSubjects.Keys)
            mTaskSubjects[key].FillValueListFor(aData);
    }

    // when the TaskSubject class is confirm working on its own, the rest of the code below will be removed from this class
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

    private TaskData CreateNewTask(TaskRank aRank, int aTaskValue)
    {
        List<int> availableLinkCube = new List<int>();
        int selectIndex = 0;

        foreach(int cc in mLinkCubeTaskValueCountList.Keys)
        {
            if (mLinkCubeTaskValueCountList[cc].Contains(aTaskValue))
                availableLinkCube.Add(cc);
        }

        int linkCount = availableLinkCube.Count;
        if (linkCount > 1)
            selectIndex = Random.Range(0, linkCount);

        TaskData data = new TaskData(aRank, availableLinkCube[selectIndex], aTaskValue);
        return data;
    }

    private Dictionary<int, int> IterateCombination()
    {
        Dictionary<int, int> combinationList = new Dictionary<int, int>();

        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_2_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(2, 0.035f, 0.065f);
            //mTaskSubjects.Add(2, data);

            IterateTwoCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(2);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_3_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(3, 0.013f, 0.04f);
            //mTaskSubjects.Add(3, data);

            IterateThreeCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(3);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_4_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(4, 0.007f, 0.037f);
            //mTaskSubjects.Add(4, data);

            IterateFourCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(4);
        }
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreingLinks.LINK_5_DIGIT))
        {
            TaskRankValueData data = new TaskRankValueData(5, 0.0028f, 0.025f);
            //mTaskSubjects.Add(5, data);

            IterateFiveCubesCombination(ref combinationList);
            mActiveLinkedCubes.Add(5);
        }

        return combinationList;
    }
    // these four function will be removed with the succession of the permutation logic implanted in TaskRankValueData class
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
                {
                    mLinkCubeTaskValueCountList[2].Add(sum);
                }

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

public class TaskSubject
{
    public TaskRank Rank { get { return mRank; } }
    private readonly TaskRank mRank = TaskRank.X1;

    private Dictionary<int, bool> mUsedLinks = new Dictionary<int, bool>();
    private Dictionary<int, List<int>> mTaskValueLists = new Dictionary<int, List<int>>();
    private Dictionary<int, List<bool>> mUsedTaskNumbers = new Dictionary<int, List<bool>>();

    private int mTaskValueLimit = 0;
    private int mMaxValue = 0;

    private readonly bool mUnderDebuging = false;
    private readonly int mDebugingValue = 0;
    private readonly int mDebugingLink = 0;

    /// <summary>
    /// Default constructor
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

    public void FillValueListFor(TaskRankValueData aData)
    {
        mUsedLinks.Add(aData.LinkCubes, false);

        if(!mUsedTaskNumbers.ContainsKey(aData.LinkCubes) && !mTaskValueLists.ContainsKey(aData.LinkCubes))
        {
            mUsedTaskNumbers.Add(aData.LinkCubes, new List<bool>());
            mTaskValueLists.Add(aData.LinkCubes, new List<int>());
        }

        List<int> valueList = aData.GetNumberListForRank(mRank);

        for(int i = 0; i < valueList.Count; i++)
        {
            mTaskValueLists[aData.LinkCubes].Add(valueList[i]);
            if (valueList[i] <= mTaskValueLimit)
                mUsedTaskNumbers[aData.LinkCubes].Add(false);
        }

        mMaxValue = valueList.Last();
        return;
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

        return new TaskData(mRank, linkCount, taskValue);
    }

    public void IncreaseTaskValueLimit()
    {
        if ((mTaskValueLimit + 1) > mMaxValue)
            return;

        mTaskValueLimit++;
        foreach(int key in mUsedTaskNumbers.Keys)
            mUsedTaskNumbers[key].Add(false);
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

    private int GetTaskValue(int aLinkKey)
    {
        int selectedIndex = 0;

        List<int> indexList = new List<int>();
        for(int i = 0; i  < mUsedTaskNumbers[aLinkKey].Count; i++)
        {
            if (!mUsedTaskNumbers[aLinkKey][i])
                indexList.Add(i);
        }

        if(indexList.Count == 1)
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

        return mTaskValueLists[aLinkKey][selectedIndex];
    }

    private void ResetTaskValues(int aLinkKey)
    {
        for(int i = 0; i < mUsedTaskNumbers[aLinkKey].Count; i++)
            mUsedTaskNumbers[aLinkKey][i] = false;
    }
}

public class TaskData
{
    public TaskRank Rank { get { return mRank; } }
    private TaskRank mRank = TaskRank.X1;

    public int Number { get { return mTaskNumber; } }
    private int mTaskNumber = 0;

    public int LinkedCubes { get { return mTaskLinkedCube; } }
    private int mTaskLinkedCube = 0;

    public bool TaskComplete { get { return mTaskComplete; } }
    private bool mTaskComplete = false;
    
    public TaskData()
    {
        mTaskNumber = 0;
        mTaskLinkedCube = 0;
    }

    public TaskData(TaskData aData)
    {
        mRank = aData.Rank;
        mTaskNumber = aData.Number;
        mTaskLinkedCube = aData.LinkedCubes;
        mTaskComplete = false;
    }

    public TaskData(TaskRank aRank, int aTaskCubeCount, int aTaskNumber)
    {
        mRank = aRank;
        mTaskLinkedCube = aTaskCubeCount;
        mTaskNumber = aTaskNumber;
        mTaskComplete = false;
    }

    public void SetValue(TaskData aData)
    {
        mRank = aData.Rank;
        mTaskNumber = aData.Number;
        mTaskLinkedCube = aData.LinkedCubes;
        mTaskComplete = false;
    }

    public bool IsMatchingData(int aTaskNumber, int aLinkCube)
    {
        if (mTaskNumber == aTaskNumber && mTaskLinkedCube == aLinkCube)
            return true;

        return false;
    }

    public void TaskCompleted()
    {
        mTaskComplete = true;
    }
}

public class TaskRankValueData
{
    public int LinkCubes { get { return mLinkedCubeCount; } }
    private readonly int mLinkedCubeCount = 0;

    private readonly int mMaxValue = 1;

    private List<bool> mNumberUsed = new List<bool>();
    private Dictionary<TaskRank, List<int>> mRankNumberList = new Dictionary<TaskRank, List<int>>();

    private readonly float mRedSumDistribution = 0f;
    private readonly float mBlueSumDistribution = 0f;
    
    public TaskRankValueData()
    {

    }

    public TaskRankValueData(int aCubeCount, float redDistribution, float blueDistribution)
    {
        mLinkedCubeCount = aCubeCount;
        mRedSumDistribution = redDistribution;
        mBlueSumDistribution = blueDistribution;

        mMaxValue += 9 * aCubeCount;
        for(int i = 0; i < Constants.MINIMAL_TASK_VALUE; i++)
            mNumberUsed.Add(false);

        for (TaskRank r = TaskRank.X1; r != TaskRank.X10 + 1; r++)
            mRankNumberList.Add(r, new List<int>());

        IEnumerable<IEnumerable<int>> result = GetPermutationsWithRept(Enumerable.Range(0, 10), aCubeCount);

        List<List<int>> lists = result.Select(i => i.ToList()).ToList();

        RankDistribution(lists);
    }
    
    public List<int> GetNumberListForRank(TaskRank aRank)
    {
        return mRankNumberList[aRank];
    }
    
    private void RankDistribution(List<List<int>> aPremutatedList)
    {
        Dictionary<int, float> oddsList = CategorizeSumOdds(CategorizeSumValue(aPremutatedList));

        foreach(int key in oddsList.Keys)
        {
            if (oddsList[key] < mRedSumDistribution)
                mRankNumberList[TaskRank.X10].Add(key);
            else if(oddsList[key] < mBlueSumDistribution && oddsList[key] > mRedSumDistribution)
                mRankNumberList[TaskRank.X5].Add(key);
            else
                mRankNumberList[TaskRank.X1].Add(key);
        }
    }

    private Dictionary<int, int> CategorizeSumValue(List<List<int>> aPremutatedList)
    {
        Dictionary<int, int> categorizedList = new Dictionary<int, int>();

        for(int i = 0; i < aPremutatedList.Count; i++)
        {
            int sum = 0;
            for (int j = 0; j < aPremutatedList[i].Count; j++)
                sum += aPremutatedList[i][j];

            if (!categorizedList.ContainsKey(sum))
                categorizedList.Add(sum, 0);
            categorizedList[sum]++;
        }

        return categorizedList;
    }

    private Dictionary<int, float> CategorizeSumOdds(Dictionary<int, int> aCategorizedList)
    {
        float total = 0f;
        foreach (int key in aCategorizedList.Keys)
            total += aCategorizedList[key];

        Dictionary<int, float> sumOddsList = new Dictionary<int, float>();

        foreach (int key in aCategorizedList.Keys)
            sumOddsList.Add(key, (float)aCategorizedList[key] / total);

        return sumOddsList;
    }

    static IEnumerable<IEnumerable<T>>
    GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetPermutationsWithRept(list, length - 1)
            .SelectMany(t => list,
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }
}

