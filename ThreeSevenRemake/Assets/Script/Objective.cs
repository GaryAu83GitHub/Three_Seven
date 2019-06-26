using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Objectives
{
    X1,
    X5,
    X10,
}

public class Objective
{
    public static Objective Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new Objective();
            return mInstance;
        }
    }
    private static Objective mInstance;

    public delegate void OnAchiveObjective(Objectives anObjective, int anObjectiveNumber);
    public static OnAchiveObjective achiveObjective;

    private List<bool> mUsedObjectiveNumbers = new List<bool>();
    private List<int> mCurrentMaxObjectiveCount = new List<int>();
    private Dictionary<Objectives, int> mActiveObjectives = new Dictionary<Objectives, int>();
    private Dictionary<Objectives, List<int>> mObjectiveNumbersList = new Dictionary<Objectives, List<int>>();
    private Dictionary<Objectives, bool> mObjectiveAchieveList = new Dictionary<Objectives, bool>();
    
    public Objective()
    {
        mObjectiveNumbersList.Add(Objectives.X1, new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 }); // count 27
        mObjectiveNumbersList.Add(Objectives.X5, new List<int>() { 4, 5, 6, 7, 8, 36, 37, 38, 39, 40 }); // count 10
        mObjectiveNumbersList.Add(Objectives.X10, new List<int>() { 0, 1, 2, 3, 41, 42, 43, 44, 45 }); // count 9
        
        for(int i = 0; i < 46; i++)
        {
            mUsedObjectiveNumbers.Add(false);
        }

        foreach (Objectives obj in mObjectiveNumbersList.Keys)
        {
            mObjectiveAchieveList.Add(obj, false);
            mActiveObjectives.Add(obj, mObjectiveNumbersList[obj][Random.Range(0, mObjectiveNumbersList[obj].Count)]);
            achiveObjective?.Invoke(obj, mActiveObjectives[obj]);
        }
    }

    public void StartFirstSetOfObjective()
    {
        foreach (Objectives obj in mObjectiveNumbersList.Keys)
        {
            achiveObjective?.Invoke(obj, mActiveObjectives[obj]);
        }
    }

    public bool AchiveObjective(int aCheckingNumber)
    {
        if (!mActiveObjectives.ContainsValue(aCheckingNumber))
            return false;

        mUsedObjectiveNumbers[aCheckingNumber] = true;

        Objectives obj = mActiveObjectives.FirstOrDefault(x => x.Value == aCheckingNumber).Key;
        mObjectiveAchieveList[obj] = true;
        return true;
    }

    public void ChangeObjective()
    {
        if (!mObjectiveAchieveList.ContainsValue(true))
            return;

        foreach (Objectives obj in mObjectiveAchieveList.Keys.ToList())
        {
            if (mObjectiveAchieveList[obj])
            {
                mObjectiveAchieveList[obj] = false;
                CheckForNeedToResetNumberFor(obj);
                mActiveObjectives[obj] = GetNewAchieveNumber(obj, mActiveObjectives[obj]);
                achiveObjective?.Invoke(obj, mActiveObjectives[obj]);

            }
        }
    }

    public int ObjectiveAchieveBonus()
    {
        int bonus = 0;
        if (mObjectiveAchieveList[Objectives.X1])
            bonus++;
        if (mObjectiveAchieveList[Objectives.X5])
            bonus += 5;
        if (mObjectiveAchieveList[Objectives.X10])
            bonus += 10;

        return bonus;
    }

    private int GetNewAchieveNumber(Objectives anObjective, int aPreviousNumber)
    {
        int newAchieveNumber = aPreviousNumber;
        
        while(mUsedObjectiveNumbers[newAchieveNumber])
        {
            newAchieveNumber = mObjectiveNumbersList[anObjective][Random.Range(0, mObjectiveNumbersList[anObjective].Count)];
        }

        return newAchieveNumber;
    }

    private void CheckForNeedToResetNumberFor(Objectives anObjective)
    {
        foreach(int a in mObjectiveNumbersList[anObjective])
        {
            if (!mUsedObjectiveNumbers[a])
                return;
        }

        ResetObjectiveNumberFor(anObjective);
    }

    private void ResetObjectiveNumberFor(Objectives anObjective)
    {
        for (int i = 0; i < mObjectiveNumbersList[anObjective].Count; i++)
            mUsedObjectiveNumbers[mObjectiveNumbersList[anObjective][i]] = false;
    }

    public void PrepareObjectives()
    {
        Dictionary<int, int> combinationCount = new Dictionary<int, int>();

        for(int i = 0; i < 10; i++)
        {
            IterateTwoCubesCombination(i, ref combinationCount);
        }

        for (int i = 0; i < combinationCount.Count; i++)
        {
            mUsedObjectiveNumbers.Add(false);
        }

        int mostCombination = combinationCount.Values.Max();
        foreach(int key in combinationCount.Keys)
        {
            if(mostCombination.ToString().Length == 4)
            {
                if (combinationCount[key].ToString().Length == 1 || combinationCount[key].ToString().Length == 2)
                    mObjectiveNumbersList[Objectives.X10].Add(key);
                if (combinationCount[key].ToString().Length == 3)
                    mObjectiveNumbersList[Objectives.X5].Add(key);
                if (combinationCount[key].ToString().Length == 4)
                    mObjectiveNumbersList[Objectives.X1].Add(key);
            }
            else if (mostCombination.ToString().Length == 3)
            {
                if (combinationCount[key].ToString().Length == 1 || combinationCount[key].ToString().Length == 2)
                    mObjectiveNumbersList[Objectives.X5].Add(key);
                if (combinationCount[key].ToString().Length == 3)
                    mObjectiveNumbersList[Objectives.X1].Add(key);
            }
            else
                mObjectiveNumbersList[Objectives.X1].Add(key);

        }
        return;
    }

    private void IterateTwoCubesCombination(int aFirstValue, ref Dictionary<int, int> someObjectives)
    {
        for (int i = 0; i < 10; i++)
        {
            int sum = aFirstValue + i;
            if (!someObjectives.ContainsKey(sum))
                someObjectives.Add(sum, 0);

            someObjectives[sum]++;
            if(!GameSettings.Instance.IsTheseScoringSettingEquals(true, false, false, false))
                IterateThreeCubesCombination(aFirstValue, i, ref someObjectives);
        }
    }

    private void IterateThreeCubesCombination(int aFirstValue, int aSecondValue, ref Dictionary<int, int> someObjectives)
    {
        for (int i = 0; i < 10; i++)
        {
            int sum = aFirstValue + aSecondValue + i;
            if (!someObjectives.ContainsKey(sum))
                someObjectives.Add(sum, 0);

            someObjectives[sum]++;
            if (!GameSettings.Instance.IsTheseScoringSettingEquals(true, true, false, false))
                IterateFourCubesCombination(aFirstValue, aSecondValue, i, ref someObjectives);
        }
    }

    private void IterateFourCubesCombination(int aFirstValue, int aSecondValue, int aThirdValue, ref Dictionary<int, int> someObjectives)
    {
        for (int i = 0; i < 10; i++)
        {
            int sum = aFirstValue + aSecondValue + aThirdValue + i;
            if (!someObjectives.ContainsKey(sum))
                someObjectives.Add(sum, 0);

            someObjectives[sum]++;
            if (!GameSettings.Instance.IsTheseScoringSettingEquals(true, true, true, false))
                IterateFiveCubesCombination(aFirstValue, aSecondValue, aThirdValue, i, ref someObjectives);
        }
    }

    private void IterateFiveCubesCombination(int aFirstValue, int aSecondValue, int aThirdValue, int aFourthValue, ref Dictionary<int, int> someObjectives)
    {
        for (int i = 0; i < 10; i++)
        {
            int sum = aFirstValue + aSecondValue + aThirdValue + aFourthValue + i;
            if (!someObjectives.ContainsKey(sum))
                someObjectives.Add(sum, 0);

            someObjectives[sum]++;
        }
    }
}

