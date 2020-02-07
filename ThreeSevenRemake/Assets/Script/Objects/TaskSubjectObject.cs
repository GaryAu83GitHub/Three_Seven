using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaskSubjectObject
{
    public int LinkCubes { get { return mLinkedCubeCount; } }
    private readonly int mLinkedCubeCount = 0;

    private readonly Dictionary<Difficulties, int> mFlowValueList = new Dictionary<Difficulties, int>()
    {
        {Difficulties.EASY, 2 },
        {Difficulties.NORMAL, 2 },
        {Difficulties.HARD, 2 },
    };
    
    private readonly Difficulties mSelectedDifficulty = Difficulties.EASY;

    private Dictionary<int, int> mCombinationCountList = new Dictionary<int, int>();
    private Dictionary<int, bool> mUsedTaskValue = new Dictionary<int, bool>();

    private readonly int mMaxValue = 1;
    private int mCurrentEnableTaskValue = 5;

    public TaskSubjectObject()
    {

    }

    public TaskSubjectObject(int aCubeCount)
    {
        mLinkedCubeCount = aCubeCount;
        mMaxValue += 9 * aCubeCount;
        switch(mLinkedCubeCount)
        {
            case 2:
                mCurrentEnableTaskValue = 9;
                break;
            case 3:
                mCurrentEnableTaskValue = 8;
                break;
            case 4:
                mCurrentEnableTaskValue = 7;
                break;
            case 5:
                mCurrentEnableTaskValue = 6;
                break;
        }

        mSelectedDifficulty = GameSettings.Instance.Difficulty;

        Initilize(GetPermutationList(0, 10, aCubeCount));
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
    
    public void SetUsedTaskValue(int aSelectUseValue)
    {
        mUsedTaskValue[aSelectUseValue] = true;
    }

    /// <summary>
    /// This function is use for reseting the boolian list of used task value when
    /// the list only have one item in the list that is false.
    /// Every value in the list will be set to false once this fuction is called
    /// except the using indexes
    /// </summary>
    /// <param name="someExceptedIndexes"></param>
    public void ResetUsedTaskValue(List<int> someExceptedIndexes)
    {
        for (int i = 0; i < mUsedTaskValue.Count; i++)
        {
            if (!someExceptedIndexes.Contains(i))
                mUsedTaskValue[i] = false;
            else
                mUsedTaskValue[i] = true;
        }
    }

    public void ChangeTaskValue(int aLevelUpCount)
    {
        switch(mLinkedCubeCount)
        {
            case 2:
                ExpandTaskValue(aLevelUpCount, mFlowValueList[mSelectedDifficulty]);
                break;
            case 3:
                ExpandTaskValue(aLevelUpCount, mFlowValueList[mSelectedDifficulty]);
                break;
            case 4:
                ExpandTaskValue(aLevelUpCount, mFlowValueList[mSelectedDifficulty]);
                break;
            case 5:
                ExpandTaskValue(aLevelUpCount, mFlowValueList[mSelectedDifficulty]);
                break;
        }
    }

    private void ExpandTaskValue(int aLevelUpCount, int aDevisor)
    {
        if (MathTools.Modulus(aLevelUpCount, aDevisor) &&
            mCurrentEnableTaskValue + 1 != mMaxValue)
        {
            mCurrentEnableTaskValue++;
            mUsedTaskValue.Add(mCurrentEnableTaskValue, false);
        }
    }

    private void Initilize(List<List<int>> aPremutatedList)
    {
        CubeNumberManager.Instance.GenerateCubeNumberUsedCountFor((LinkIndexes)(mLinkedCubeCount - 2), aPremutatedList);
        mCombinationCountList = new Dictionary<int, int>(CategorizeSumValue(aPremutatedList));

        for (int i = 0; i < mCurrentEnableTaskValue; i++)
            mUsedTaskValue.Add(i, false);
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

