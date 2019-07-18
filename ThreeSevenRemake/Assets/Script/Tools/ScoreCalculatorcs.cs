using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ScoreCalculatorcs
{
    public static int LinkingScoreCalculation(Objectives anObjective, int aNumberCount)
    {
        int objectiveMultiply = 1;
        if (anObjective == Objectives.X10)
            objectiveMultiply = 10;
        else if (anObjective == Objectives.X5)
            objectiveMultiply = 5;
        else
            objectiveMultiply = 1;

        return (/*(GameManager.Instance.CurrentLevel + 1) +*/ (aNumberCount * objectiveMultiply));
    }

    public static int ComboScoreCalculation(int aCombo)
    {
        return (ComboBaseScoreFactory.Instance.GetComboBaseScore(aCombo) * (GameManager.Instance.CurrentLevel + 1));
    }
}
