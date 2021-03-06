﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayThroughcs
{
    public string Name { get; } = "";
    public string GameTime { get; } = "";


    public int Level { get; } = 0;
    public int Score { get; } = 0;
    public int InitialValue { get; } = 0;

    private List<bool> mScoringMethod = new List<bool>();
    public List<bool> ScoringMethod { get { return mScoringMethod; } }
    
    public PlayThroughcs()
    {
        Name = GameSettings.Instance.PlayerName;
        GameTime = GamingManager.Instance.GameTimeString;

        Level = LevelManager.Instance.CurrentLevel;
        Score = GamingManager.Instance.CurrentScore;
        InitialValue = GameSettings.Instance.InitialValue;

        for (int i = 0; i < (int)LinkIndexes.MAX; i++)
            mScoringMethod.Add(GameSettings.Instance.EnableScoringMethods[i]);
    }

}
