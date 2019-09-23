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
}
