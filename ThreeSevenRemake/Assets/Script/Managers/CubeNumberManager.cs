using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CubeNumberManager
{
    public static CubeNumberManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new CubeNumberManager();
            return mInstance;
        }
    }
    private static CubeNumberManager mInstance;

    public int GetNewRootNumber { get { return GetNewCubeNumber(); } }
    public int GetNewSubNumber { get { return GetNewCubeNumber(); } }

    public delegate void OnUpdateCubeNumberIntervall(Dictionary<int, int> anOriginalIntervall, Dictionary<int, int> aCurrentIntervall);
    public static OnUpdateCubeNumberIntervall updateIntervall;

    public List<bool> UsedRootNumber { get { return mUsedRootNumber; } }
    private List<bool> mUsedRootNumber = new List<bool>();

    public List<bool> UsedSubNumber { get { return mUsedSubNumber; } }
    private List<bool> mUsedSubNumber = new List<bool>();

    private Dictionary<LinkIndexes, NumberGenerator> mNumberGenerators = new Dictionary<LinkIndexes, NumberGenerator>();
    private List<float> mNumberOddsIntervall = new List<float>();

    /// <summary>
    /// This is use for storing the original counts of each number when new task is created
    /// </summary>
    private Dictionary<int, int> mOriginalNumberCounts = new Dictionary<int, int>();
    /// <summary>
    /// This is use for storing the current counts of each number when their counts reduce when they been selected once
    /// </summary>
    private Dictionary<int, int> mCurrentNumberCounts = new Dictionary<int, int>();
    public Dictionary<int, int> CurrentNumberCounts { get { return mCurrentNumberCounts; } }


    public CubeNumberManager()
    {
        for (int i = 0; i != Constants.CUBE_MAX_NUMBER; i++)
        {
            mUsedRootNumber.Add(false);
            mUsedSubNumber.Add(false);
        }

        ClearNumberOddsList();
        ClearUseableNumberList();
    }

    public void GenerateCubeNumberOddsFor(LinkIndexes aCubeLinkIndex, List<int> someTaskValue)
    {
        mNumberGenerators.Add(aCubeLinkIndex, new NumberGenerator(aCubeLinkIndex, someTaskValue));
    }

    public void GenerateCubeNumberUsedCountFor(LinkIndexes aLinkIndex, List<List<int>> someTaskValueList)
    {
        if (!mNumberGenerators.ContainsKey(aLinkIndex))
            mNumberGenerators.Add(aLinkIndex, new NumberGenerator(aLinkIndex, someTaskValueList));
        else
            mNumberGenerators[aLinkIndex] = new NumberGenerator(aLinkIndex, someTaskValueList);
    }

    public void GenerateNewUseableCubeNumberFor(List<TaskData> someTaskDatas)
    {
        ClearUseableNumberList();
        List<int> odds = new List<int>();
        List<int> keys = new List<int>();
        List<int> counts = new List<int>();

        LinkIndexes key = LinkIndexes.MAX;
        int taskValue = 0;

        foreach (TaskData data in someTaskDatas)
        {
            key = (LinkIndexes)data.LinkedCubes - 2;
            taskValue = data.TaskValue;
            List<int> thisDataNumberCounts = new List<int>(mNumberGenerators[key].GetNumberCountListForTaskValue(taskValue));

            for (int i = 0; i < thisDataNumberCounts.Count; i++)
            {
                if(!keys.Contains(i))
                {
                    keys.Add(i);
                    counts.Add(thisDataNumberCounts[i]);
                }
                else
                {
                    counts[i] += thisDataNumberCounts[i];
                }
            }
        }

        for (int i = 0; i < keys.Count; i++)
        {
            if (counts[i] != 0)
            {
                mOriginalNumberCounts.Add(keys[i], counts[i]);
                mCurrentNumberCounts.Add(keys[i], counts[i]);
            }
        }
        return;
    }

    private int GetNewCubeNumberFor(ref List<bool> someUsedNumber)
    {
        int newNumber = GetRandomUnusedNumberFrom(ref someUsedNumber);

        someUsedNumber[newNumber] = true;
        //if (!someUsedNumber.Contains(false))
        //{
        //    for (int i = 0; i < someUsedNumber.Count; i++)
        //        someUsedNumber[i] = false;
        //}

        return newNumber;
    }

    private int GetNewCubeNumber()
    {
        Dictionary<int, float> intervall = GenerateNumbersInterval();

        int keyNumber = GetCubeNumberRecrusiveOf(intervall, 0, Random.Range(0, 100));
        if (mCurrentNumberCounts[keyNumber] > 0)
            mCurrentNumberCounts[keyNumber]--;

        if (mCurrentNumberCounts[keyNumber] == 0)
            mCurrentNumberCounts.Remove(keyNumber);

        if (!mCurrentNumberCounts.Any())
        {
            mCurrentNumberCounts = new Dictionary<int, int>(mOriginalNumberCounts);
            GenerateNewUseableCubeNumberFor(TaskManagerNew.Instance.ActiveTask);
        }
        updateIntervall?.Invoke(mOriginalNumberCounts, mCurrentNumberCounts);
        return keyNumber;
    }

    private int GetCubeNumberFromIntervall(int aRandomizedValue)
    {
        int selectedNumber = 0;

        for (int i = 0; i < mNumberOddsIntervall.Count; i++)
        {
            if (aRandomizedValue >= mNumberOddsIntervall[i] && aRandomizedValue < mNumberOddsIntervall[i + 1])
            {
                selectedNumber = i;
                break;
            }
        }

        return selectedNumber;
    }

    private int GetRandomUnusedNumberFrom(ref List<bool> someUsedNumber)
    {
        List<int> ununsedNumberIndeces = new List<int>();

        for(int index = 0; index < someUsedNumber.Count; index++)
        {
            if (!someUsedNumber[index])
                ununsedNumberIndeces.Add(index);
        }

        if (ununsedNumberIndeces.Count == 1)
        {   
            for (int i = 0; i < someUsedNumber.Count; i++)
                someUsedNumber[i] = false;

            someUsedNumber[ununsedNumberIndeces[0]] = true;
            return ununsedNumberIndeces[0];
        }

        return ununsedNumberIndeces[SupportTools.RNG(0, ununsedNumberIndeces.Count)];
    }

    private void ClearNumberOddsList()
    {
        mNumberOddsIntervall.Clear();
        for (int i = 0; i < 10; i++)
            mNumberOddsIntervall.Add(0f);
    }

    private void ClearUseableNumberList()
    {
        mOriginalNumberCounts.Clear();
        mCurrentNumberCounts.Clear();
    }

    private float OddSummary(List<float> someOdds)
    {
        float result = 0f;

        for (int i = 0; i < someOdds.Count; i++)
            result += someOdds[i];

        return result;
    }

    private int TotalSummary(List<int> someCounts)
    {
        int result = 0;

        for (int i = 0; i < someCounts.Count; i++)
            result += someCounts[i];

        return result;
    }

    private int GetCubeNumberRecrusiveOf(Dictionary<int, float> someIntervalls, int anIndex, int randomValue)
    {
        int currentIndex = anIndex;

        if (currentIndex == someIntervalls.Count)
            return someIntervalls.ElementAt(currentIndex - 1).Key;

        if (randomValue < someIntervalls.ElementAt(currentIndex).Value)
            return someIntervalls.ElementAt(currentIndex - 1).Key;

        return GetCubeNumberRecrusiveOf(someIntervalls, currentIndex + 1, randomValue);
    }

    // This will be replace by GetCubeNumberRecrusiveOf
    private int GetCubeNumberRecrusive(int anIndex, int randomValue)
    {
        int currentIndex = anIndex;

        if (currentIndex == mNumberOddsIntervall.Count)
            return currentIndex - 1;

        if (randomValue < mNumberOddsIntervall[currentIndex])
            return currentIndex - 1;

        return GetCubeNumberRecrusive(currentIndex + 1, randomValue);
    }

    private Dictionary<int, float> GenerateNumbersInterval()
    {
        int[] odds = mCurrentNumberCounts.Values.ToArray();

        float totalSum = TotalSummary(odds.ToList());
        for (int i = 0; i < odds.Length; i++)
        {
            float temp = odds[i];
            odds[i] = Mathf.RoundToInt((temp / totalSum) * 100f);
        }

        Dictionary<int, float> intervall = new Dictionary<int, float>();

        for (int i = 0; i < mCurrentNumberCounts.Keys.Count; i++)
        {
            int key = mCurrentNumberCounts.Keys.ToList()[i];
            if (i == 0)
                intervall.Add(key, 0);
            else
            {
                intervall.Add(key, (intervall.ElementAt(i - 1).Value + odds[i - 1]));
            }
        }
        return intervall;
    }
}

public class NumberGenerator
{
    public LinkIndexes LinkIndex { get { return mLinkIndex; } }
    private readonly LinkIndexes mLinkIndex = 0;

    /// <summary>
    /// This dictionary is to hold an list of each cube numbers odds for generating depending on each Task value of the number of cube links
    /// Exp, play select to challenge 2 cube link addition, the task value intervall are between 0 - 18. And each cube number have a different procentage of use on each value.
    /// </summary>
    private Dictionary<int, List<float>> mTaskNumberOddsList = new Dictionary<int, List<float>>();
    private readonly List<List<int>> mNumberCountList = new List<List<int>>();
    
    private readonly int mLinkCount = 0;
    public NumberGenerator(LinkIndexes aLinkIndex, List<int> taskValueList)
    {
        mLinkIndex = aLinkIndex;
        mLinkCount = (int)mLinkIndex + 2;
        for(int key = 0; key < taskValueList.Count; key++)
        {
            mTaskNumberOddsList.Add(key, GenerateOdds(key));
        }
    }

    public NumberGenerator(LinkIndexes aLinkIndex, List<List<int>> aTaskValueList)
    {
        mLinkIndex = aLinkIndex;
        mLinkCount = (int)mLinkIndex + 2;

        mNumberCountList = NumberCategorize(aTaskValueList);
    }

    public List<float> GetNumberOddsForTaskValue(int aTaskValue)
    {
        return mTaskNumberOddsList[aTaskValue];
    }

    public List<int> GetNumberCountListForTaskValue(int aTaskValue)
    {
        return mNumberCountList[aTaskValue];
    }

    private List<float> GenerateOdds(int aTaskValue)
    {
        List<float> oddsList = new List<float>();
        float median = (float)aTaskValue / (float)mLinkCount;
        float odds = 0f;

        for (int i = 0; i < 10; i++)
        {
            odds = 10f - Mathf.Abs(median - (float)i);
            oddsList.Add(odds);
        }

        return oddsList;
    }

    private List<List<int>> NumberCategorize(List<List<int>> aTaskValueList)
    {
        Dictionary<int, List<List<int>>> summary = new Dictionary<int, List<List<int>>>();

        for (int i = 0; i < aTaskValueList.Count; i++)
        {
            aTaskValueList[i].Sort();
            int sumKey = RecrusiveSum(0, 0, aTaskValueList[i]);
            if (!summary.ContainsKey(sumKey))
                summary.Add(sumKey, new List<List<int>>());

            if (!BothListAreMatched(summary[sumKey], aTaskValueList[i]))
                summary[sumKey].Add(aTaskValueList[i]);
        }

        List<List<int>> result = new List<List<int>>();
        foreach (int key in summary.Keys)
        {
            int[] numberCount = new int[10];
            for (int i = 0; i < summary[key].Count; i++)
            {
                for (int j = 0; j < summary[key][i].Count; j++)
                {
                    int index = summary[key][i][j];
                    numberCount[index]++;
                }
            }
            result.Add(numberCount.ToList());
        }

        return result;
    }

    static int RecrusiveSum(int anIndex, int aSumSoFar, List<int> someValue)
    {
        int currentSum = aSumSoFar + someValue[anIndex];
        int nextIndex = anIndex + 1;

        if (nextIndex >= someValue.Count)
            return currentSum;

        return RecrusiveSum(nextIndex, currentSum, someValue);
    }

    static bool BothListAreMatched(List<List<int>> anOrgiginalList, List<int> testingList)
    {
        for (int i = 0; i < anOrgiginalList.Count; i++)
        {
            if (anOrgiginalList[i].SequenceEqual(testingList))
                return true;
        }
        return false;
    }
}
