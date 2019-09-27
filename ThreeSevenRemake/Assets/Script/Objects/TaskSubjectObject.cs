using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaskSubjectObject
{
    public int LinkCubes { get { return mLinkedCubeCount; } }
    private readonly int mLinkedCubeCount = 0;

    private readonly int mMaxValue = 1;

    private Dictionary<TaskRank, List<int>> mRankNumberList = new Dictionary<TaskRank, List<int>>();

    private Dictionary<int, int> mCombinationCountList = new Dictionary<int, int>();
    private Dictionary<int, bool> mUsedTaskValue = new Dictionary<int, bool>();

    private readonly float mRedSumDistribution = 0f;
    private readonly float mBlueSumDistribution = 0f;

    public TaskSubjectObject()
    {

    }

    public TaskSubjectObject(int aCubeCount)
    {
        mLinkedCubeCount = aCubeCount;
        mMaxValue += 9 * aCubeCount;

        //IEnumerable<IEnumerable<int>> result = GetPermutationsWithRept(Enumerable.Range(0, 10), aCubeCount);

        //List<List<int>> lists = result.Select(i => i.ToList()).ToList();

        Initilize(GetPermutationList(0, 10, aCubeCount));
    }

    public TaskSubjectObject(int aCubeCount, float redDistribution, float blueDistribution)
    {
        mLinkedCubeCount = aCubeCount;
        mRedSumDistribution = redDistribution;
        mBlueSumDistribution = blueDistribution;

        mMaxValue += 9 * aCubeCount;

        for (TaskRank r = TaskRank.X1; r != TaskRank.X10 + 1; r++)
            mRankNumberList.Add(r, new List<int>());

        //IEnumerable<IEnumerable<int>> result = GetPermutationsWithRept(Enumerable.Range(0, 10), aCubeCount);

        //List<List<int>> lists = result.Select(i => i.ToList()).ToList();

        Initilize(GetPermutationList(0, 10, aCubeCount));
    }

    public List<int> GetNumberListForRank(TaskRank aRank)
    {
        return mRankNumberList[aRank];
    }

    public List<int> GetUnusedTaskValue()
    {
        List<int> unUsedTaskIndexes = new List<int>();

        foreach (int key in mUsedTaskValue.Keys)
        {
            if (mUsedTaskValue[key] == false)
                unUsedTaskIndexes.Add(key);
        }
        return unUsedTaskIndexes;
    }

    public void ResetUsedTaskValue(int anExceptedIndex = -1)
    {
        for (int i = 0; i < mUsedTaskValue.Count; i++)
            mUsedTaskValue[i] = false;

        if (anExceptedIndex > -1)
            mUsedTaskValue[anExceptedIndex] = true;
    }

    public TaskRank GetRankFor(int aTaskValue)
    {
        mUsedTaskValue[aTaskValue] = true;

        if (mCombinationCountList[aTaskValue] > 500)
            return TaskRank.X10;
        else if (mCombinationCountList[aTaskValue] > 50 && mCombinationCountList[aTaskValue] < 500)
            return TaskRank.X5;

        return TaskRank.X1;
    }

    private void Initilize(List<List<int>> aPremutatedList)
    {
        CubeNumberManager.Instance.GenerateCubeNumberUsedCountFor((LinkIndexes)(mLinkedCubeCount - 2), aPremutatedList);
        mCombinationCountList = new Dictionary<int, int>(CategorizeSumValue(aPremutatedList));

        for (int i = 0; i < mMaxValue; i++)
            mUsedTaskValue.Add(i, false);

        //Dictionary<int, float> oddsList = CategorizeSumOdds(mCombinationCountList);
        //CubeNumberManager.Instance.GenerateCubeNumberOddsFor((LinkIndexes)(mLinkedCubeCount - 2), oddsList.Keys.ToList());

        //foreach(int key in mCombinationCountList.Keys)
        //{
        //    if(mCombinationCountList[key] > 500)
        //        mRankNumberList[TaskRank.X10].Add(key);
        //    else if (mCombinationCountList[key] > 50 && mCombinationCountList[key] < 500)
        //        mRankNumberList[TaskRank.X5].Add(key);
        //    else
        //        mRankNumberList[TaskRank.X1].Add(key);
        //}

        //foreach(int key in oddsList.Keys)
        //{
        //    if (oddsList[key] < mRedSumDistribution)
        //        mRankNumberList[TaskRank.X10].Add(key);
        //    else if(oddsList[key] < mBlueSumDistribution && oddsList[key] > mRedSumDistribution)
        //        mRankNumberList[TaskRank.X5].Add(key);
        //    else
        //        mRankNumberList[TaskRank.X1].Add(key);
        //}
    }

    private Dictionary<int, int> CategorizeSumValue(List<List<int>> aPremutatedList)
    {
        Dictionary<int, int> categorizedList = new Dictionary<int, int>();

        for (int i = 0; i < aPremutatedList.Count; i++)
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

    private List<List<int>> GetPermutationList(int aInitialNumber, int aNumberLenght, int aCubeCount)
    {
        IEnumerable<IEnumerable<int>> result = GetPermutationsWithRept(Enumerable.Range(aInitialNumber, aNumberLenght), aCubeCount);

        List<List<int>> lists = result.Select(i => i.ToList()).ToList();
        return lists;
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

