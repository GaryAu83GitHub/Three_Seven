using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MathTools
{
    /// <summary>
    /// Formula for Exponential calculation of math formula N = N0*e^kt
    /// </summary>
    /// <param name="aDefaultValue">the initial value (N0)</param>
    /// <param name="aTime">the time had pass (t)</param>
    /// <param name="aConstant">the rate coefficient, k for growth rate, -k for decay rate</param>
    /// <returns></returns>
    public static double ExponentialFormula(double aDefaultValue, double aConstant, double aTime)
    {
        return aDefaultValue * Math.Exp(aConstant * aTime);
    }

    public static bool Modulus(int aDividend, int aDevisor)
    {
        return ((aDividend % aDevisor) == 0 ? true : false);
    }

    public static List<List<int>> GetPermutationList(int aMinValue, int aMaxValue, int aLenght)
    {
        IEnumerable<IEnumerable<int>> result = GetPermutationsWithRept(Enumerable.Range(aMinValue, aMaxValue), aLenght);
        
        return result.Select(i => i.ToList()).ToList();
    }

    static IEnumerable<IEnumerable<T>> GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetPermutationsWithRept(list, length - 1)
            .SelectMany(t => list,
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }
}
