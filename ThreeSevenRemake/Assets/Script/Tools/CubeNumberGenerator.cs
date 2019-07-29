using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CubeNumberGenerator
{
    public static CubeNumberGenerator Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new CubeNumberGenerator();
            return mInstance;
        }
    }
    private static CubeNumberGenerator mInstance;

    public int GetNewRootNumber { get { return GetNewCubeNumberFor(ref mUsedRootNumber); } }
    public int GetNewSubNumber { get { return GetNewCubeNumberFor(ref mUsedSubNumber); } }

    public List<bool> UsedRootNumber { get { return mUsedRootNumber; } }
    private List<bool> mUsedRootNumber = new List<bool>();

    public List<bool> UsedSubNumber { get { return mUsedSubNumber; } }
    private List<bool> mUsedSubNumber = new List<bool>();

    public CubeNumberGenerator()
    {
        for (int i = 0; i < 10; i++)
        {
            mUsedRootNumber.Add(false);
            mUsedSubNumber.Add(false);
        }
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
}
