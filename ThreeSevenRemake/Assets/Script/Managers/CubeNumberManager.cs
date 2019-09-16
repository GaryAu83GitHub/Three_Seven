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

    //public int GetNewRootNumber { get { return GetNewCubeNumberFor(ref mUsedRootNumber); } }
    //public int GetNewSubNumber { get { return GetNewCubeNumberFor(ref mUsedSubNumber); } }

    public int GetNewRootNumber { get { return GetNewCubeNumber(); } }
    public int GetNewSubNumber { get { return GetNewCubeNumber(); } }

    public List<bool> UsedRootNumber { get { return mUsedRootNumber; } }
    private List<bool> mUsedRootNumber = new List<bool>();

    public List<bool> UsedSubNumber { get { return mUsedSubNumber; } }
    private List<bool> mUsedSubNumber = new List<bool>();

    private Dictionary<LinkIndexes, NumberGenerator> mNumberGenerators = new Dictionary<LinkIndexes, NumberGenerator>();
    private List<float> mNumberOddsIntervall = new List<float>();

    public CubeNumberManager()
    {
        for (int i = 0; i != Constants.CUBE_MAX_NUMBER; i++)
        {
            mUsedRootNumber.Add(false);
            mUsedSubNumber.Add(false);
        }

        ClearNumberOddsList();
    }

    public void GenerateCubeNumberOddsFor(LinkIndexes aCubeLinkIndex, List<int> someTaskValue)
    {
        mNumberGenerators.Add(aCubeLinkIndex, new NumberGenerator(aCubeLinkIndex, someTaskValue));
    }

    public void GenerateNewCubeNumberOdds(List<TaskData> someTaskDatas)
    {
        ClearNumberOddsList();

        List<float> odds = new List<float>();
        LinkIndexes key = LinkIndexes.MAX;
        int taskValue = 0;

        foreach (TaskData data in someTaskDatas)
        {
            key = (LinkIndexes)data.LinkedCubes - 2;
            taskValue = data.TaskValue;
            List<float> thisDataNumberOdds = new List<float>(mNumberGenerators[key].GetNumberOddsForTaskValue(taskValue));

            for(int i = 0; i < thisDataNumberOdds.Count; i++)
            {
                float oddValue = thisDataNumberOdds[i];

                if (i < odds.Count)
                    odds[i] += oddValue;
                else
                    odds.Add(oddValue);
            }
        }

        float totalSumOfOdds = OddSummary(odds);
        for(int i = 0; i < odds.Count; i++)
        {
            float temp = odds[i];
            odds[i] = Mathf.RoundToInt((temp / totalSumOfOdds) * 100f);
        }

        for(int i = 0; i < odds.Count; i++)
        {
            if (i == 0)
                mNumberOddsIntervall[0] = 0f;
            else
            {
                mNumberOddsIntervall[i] = (mNumberOddsIntervall[i - 1] + odds[i - 1]);
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
        int newNumber = GetCubeNumberRecrusive(0, Random.Range(0, 100));//GetCubeNumberFromIntervall(Random.Range(0, 100));
        //List<int> temp = new List<int>();
        //for (int i = 0; i < 10; i++)
        //    temp.Add(0);
        //for (int i = 0; i < 3000000; i++)
        //{
        //    int index = GetCubeNumberRecrusive(0, Random.Range(0, 100));
        //    temp[index]++;
        //}
        //TempLabClass templab = new TempLabClass(temp);
        //JsonHelper<TempLabClass>.CreateNewJsonFile("testRandom", templab);

        return newNumber;
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

    private float OddSummary(List<float> someOdds)
    {
        float result = 0f;

        for (int i = 0; i < someOdds.Count; i++)
            result += someOdds[i];

        return result;
    }
    
    private int GetCubeNumberRecrusive(int anIndex, int randomValue)
    {
        int currentIndex = anIndex;

        if (currentIndex == mNumberOddsIntervall.Count)
            return currentIndex - 1;

        if (randomValue < mNumberOddsIntervall[currentIndex])
            return currentIndex - 1;

        return GetCubeNumberRecrusive(currentIndex + 1, randomValue);
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
    
    private readonly int mLinkCount = 0;
    public NumberGenerator(LinkIndexes aLinkIndex, List<int> taskValueList)
    {
        mLinkIndex = aLinkIndex;
        mLinkCount = (int)mLinkIndex + 2;
        for(int key = 0; key < taskValueList.Count; key++)
        {
            mTaskNumberOddsList.Add(key, GenerateOdds(key));
        }
        return;
    }

    public List<float> GetNumberOddsForTaskValue(int aTaskValue)
    {
        return mTaskNumberOddsList[aTaskValue];
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
}
