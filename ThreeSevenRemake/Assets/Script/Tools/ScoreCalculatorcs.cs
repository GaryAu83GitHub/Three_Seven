using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ScoreType
{
    ORIGINAL_BLOCK_LANDING,
    LINKING,
    COMBO,
}

public static class ScoreCalculatorcs
{
    public static int OriginalBlockLandingScoreCalculation()
    {
        return (Constants.ORIGINAL_BLOCK_LANDING_SCORE * (GameManager.Instance.CurrentLevel + 1));
    }

    public static int LinkingScoreCalculation(TaskRank anObjective, int aNumberCount)
    {
        int objectiveMultiply = 1;
        if (anObjective == TaskRank.X10)
            objectiveMultiply = 10;
        else if (anObjective == TaskRank.X5)
            objectiveMultiply = 5;
        else
            objectiveMultiply = 1;

        return (aNumberCount * objectiveMultiply);
    }

    public static int ComboScoreCalculation(int aCombo)
    {
        if (aCombo < 1)
            return 0;

        return (ComboBaseScoreFactory.Instance.GetComboBaseScore(aCombo) * (GameManager.Instance.CurrentLevel + 1));
    }
}
