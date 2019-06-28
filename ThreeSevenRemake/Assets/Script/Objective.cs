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
    private Dictionary<Objectives, int> mActiveObjectives = new Dictionary<Objectives, int>();
    private Dictionary<Objectives, List<int>> mObjectiveNumbersList = new Dictionary<Objectives, List<int>>();
    private Dictionary<Objectives, bool> mObjectiveAchieveList = new Dictionary<Objectives, bool>();

    private int mMaxLimitObjectiveValue = 18;
    private int mCurrentLimitObjectiveValue = 0;
    public int CurrentLimetObjectiveValue { get { return mCurrentLimitObjectiveValue; } }
    
    public Objective()
    {
        mObjectiveNumbersList.Add(Objectives.X1, new List<int>());
        mObjectiveNumbersList.Add(Objectives.X5, new List<int>());
        mObjectiveNumbersList.Add(Objectives.X10, new List<int>());
        
        foreach (Objectives obj in mObjectiveNumbersList.Keys)
        {
            mObjectiveAchieveList.Add(obj, false);
            mActiveObjectives.Add(obj, 0);
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
                mActiveObjectives[obj] = GetObjectiveFrom(obj);
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

    public void SetMaxLimitObjectiveValue(int aMaxValue)
    {
        mMaxLimitObjectiveValue = aMaxValue;
    }

    public void SetStartObjectiveValue(int aStartValue)
    {
        mCurrentLimitObjectiveValue = aStartValue;
    }

    public void IncreaseObjectiveValue()
    {
        if ((mCurrentLimitObjectiveValue + 1) > mMaxLimitObjectiveValue)
            return;

        mCurrentLimitObjectiveValue++;
        mUsedObjectiveNumbers.Add(true);
    }

    public void PrepareObjectives()
    {
        Dictionary<int, int> combinationCount = IterateCombination();
        
        for (int i = 0; i <= mCurrentLimitObjectiveValue; i++)
            mUsedObjectiveNumbers.Add(false);

        int mostCombination = combinationCount.Values.Max();
        foreach (int key in combinationCount.Keys)
        {
            if (mostCombination.ToString().Length == 4)
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
                if (combinationCount[key].ToString().Length == 1)
                    mObjectiveNumbersList[Objectives.X10].Add(key);
                if (combinationCount[key].ToString().Length == 2)
                    mObjectiveNumbersList[Objectives.X5].Add(key);
                if (combinationCount[key].ToString().Length == 3)
                    mObjectiveNumbersList[Objectives.X1].Add(key);
            }
            else
            {
               if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreCubeCount.THREE_CUBES))
                {
                    if (key <= 3 || key >= 24) // 1 to 3 combination
                        mObjectiveNumbersList[Objectives.X10].Add(key);
                    if ((key >= 4 && key <= 9) || (key >= 18 && key <= 23)) // 4 to 7 combination
                        mObjectiveNumbersList[Objectives.X5].Add(key);
                    if (key >= 10 && key <= 17)   // 
                        mObjectiveNumbersList[Objectives.X1].Add(key);
                }
               else
                {
                    if (key <= 2 || key >= 16) // 1 to 3 combination
                        mObjectiveNumbersList[Objectives.X10].Add(key);
                    if ((key >= 3 && key <= 6) || (key >= 12 && key <= 15)) // 4 to 7 combination
                        mObjectiveNumbersList[Objectives.X5].Add(key);
                    if (key >= 7 && key <= 11)   // 
                        mObjectiveNumbersList[Objectives.X1].Add(key);
                }
            }
        }

        foreach (Objectives obj in mObjectiveNumbersList.Keys)
        {
            mObjectiveAchieveList[obj] = false;
            mActiveObjectives[obj] = GetObjectiveFrom(obj);
            achiveObjective?.Invoke(obj, mActiveObjectives[obj]);
        }
        return;
    }

    private List<int> ResetObjectiveNumberFor(Objectives anObjective)
    {
        List<int> availableObjective = new List<int>();
        foreach (int i in mObjectiveNumbersList[anObjective])
        {
            if (i > mCurrentLimitObjectiveValue)
                break;
            mUsedObjectiveNumbers[i] = false;
            availableObjective.Add(i);
        }

        return availableObjective;
    }

    private int GetObjectiveFrom(Objectives anObjective)
    {
        List<int> avaiableObjective = new List<int>();
        
        for(int i = 0; i < mUsedObjectiveNumbers.Count; i++)
        {
            if (mObjectiveNumbersList[anObjective].Contains(i) && !mUsedObjectiveNumbers[i])
                avaiableObjective.Add(i);
        }

        if (!avaiableObjective.Any())
            avaiableObjective = ResetObjectiveNumberFor(anObjective);

        int availableCount = avaiableObjective.Count;
        int selectedIndex = Random.Range(0, availableCount);
        int selectedValue = avaiableObjective[selectedIndex];

        return selectedValue;
    }

    private Dictionary<int, int> IterateCombination()
    {
        Dictionary<int, int> combinationList = new Dictionary<int, int>();
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreCubeCount.TWO_CUBES))
            IterateTwoCubesCombination(ref combinationList);
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreCubeCount.THREE_CUBES))
            IterateThreeCubesCombination(ref combinationList);
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreCubeCount.FOUR_CUBES))
            IterateFourCubesCombination(ref combinationList);
        if (GameSettings.Instance.IsScoringMethodActiveTo(ScoreCubeCount.FIVE_CUBES))
            IterateFiveCubesCombination(ref combinationList);

        return combinationList;
    }

    private void IterateTwoCubesCombination(ref Dictionary<int, int> someObjectives)
    {
        int sum = 0;
        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                sum = a + b;
                if (!someObjectives.ContainsKey(sum))
                    someObjectives.Add(sum, 0);

                someObjectives[sum]++;
            }
        }
    }

    private void IterateThreeCubesCombination(ref Dictionary<int, int> someObjectives)
    {
        int sum = 0;
        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                for (int c = 0; c < 10; c++)
                {
                    sum = a + b + c;
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
        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                for (int c = 0; c < 10; c++)
                {
                    for (int d = 0; d < 10; d++)
                    {
                        sum = a + b + c + d;
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

