using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameRoundManager
{
    public static GameRoundManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new GameRoundManager();
            return mInstance;
        }
    }
    private static GameRoundManager mInstance;

    private GameRoundData mGameRoundData = new GameRoundData();
    public GameRoundData Data { get { return mGameRoundData; } }

    public void CreateNewData()
    {
        mGameRoundData = new GameRoundData();
    }

    public void SetUpGameRound()
    {
        for (int i = 0; i < 4; i++)
            GameSettings.Instance.EnableScoringMethods[i] = mGameRoundData.EnableScoringMethods[i];

        GenerateScoreCombinationPositions.Instance.GenerateCompinationPositions();
        TaskManagerNew.Instance.PrepareNewTaskSubjects();
    }
}
