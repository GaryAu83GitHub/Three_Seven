using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum DataStringIndex
{
    PLAYER_NAME,
    PLAYER_TIME,
    MAX
}

public enum DataIntergerIndex
{
    PLAYER_LEVEL,
    PLAYER_SCORE,
    PLAYER_INITIAL_VALUE,
    MAX
}

public enum DataBooleanIndex
{
    PLAYER_2_CUBE_SCORING,
    PLAYER_3_CUBE_SCORING,
    PLAYER_4_CUBE_SCORING,
    PLAYER_5_CUBE_SCORING,
    MAX
}

[Serializable]
public class HighScoreData
{
    public string[] dataStrings;
    public int[] dataIntergers;
    public float[] dataFloats;
    public bool[] dataBooleans;

    public string GetPlayerName { get { return dataStrings[(int)DataStringIndex.PLAYER_NAME]; } }
    public string GetPlayerTime { get { return dataStrings[(int)DataStringIndex.PLAYER_TIME]; } }

    public int GetPlayerLevel { get { return dataIntergers[(int)DataIntergerIndex.PLAYER_LEVEL]; } }
    public int GetPlayerScore { get { return dataIntergers[(int)DataIntergerIndex.PLAYER_SCORE]; } }
    public int GetPlayerInitialValue { get { return dataIntergers[(int)DataIntergerIndex.PLAYER_INITIAL_VALUE]; } }

    public bool GetPlayerTwoCubeScoring { get { return dataBooleans[(int)DataBooleanIndex.PLAYER_2_CUBE_SCORING]; } }
    public bool GetPlayerThreeCubeScoring { get { return dataBooleans[(int)DataBooleanIndex.PLAYER_3_CUBE_SCORING]; } }
    public bool GetPlayerFourCubeScoring { get { return dataBooleans[(int)DataBooleanIndex.PLAYER_4_CUBE_SCORING]; } }
    public bool GetPlayerFiveCubeScoring { get { return dataBooleans[(int)DataBooleanIndex.PLAYER_5_CUBE_SCORING]; } }

    public HighScoreData(PlayThroughcs aPlay)
    {
        dataStrings[(int)DataStringIndex.PLAYER_NAME] = aPlay.Name;
        dataStrings[(int)DataStringIndex.PLAYER_TIME] = aPlay.GameTime;

        dataIntergers[(int)DataIntergerIndex.PLAYER_LEVEL] = aPlay.Level;
        dataIntergers[(int)DataIntergerIndex.PLAYER_SCORE] = aPlay.Score;
        dataIntergers[(int)DataIntergerIndex.PLAYER_INITIAL_VALUE] = aPlay.InitialValue;

        for (int i = 0; i < (int)DataBooleanIndex.MAX; i++)
            dataBooleans[i] = aPlay.ScoringMethod[i];
    }
}
