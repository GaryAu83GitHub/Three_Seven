using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreListComponent : MonoBehaviour
{
    public Text RankText;
    public Text NameText;
    public Text LevelText;
    public Text ComboText;
    public Text TimeText;
    public Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(int aRank, RoundResultData aData)
    {
        RankText.text = aRank.ToString();
        NameText.text = aData.PlayerName;
        LevelText.text = aData.TotalLevel.ToString();
        ComboText.text = aData.TotalMaxCombo.ToString();

        //int seconds = (int)(aData.TotalTime % 60);
        //int minutes = (int)((aData.TotalTime / 60) % 60);
        //int hours = (int)((aData.TotalTime / 3600) % 60);

        //TimeText.text = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        TimeText.text = TimeTool.TimeString(aData.TotalTime);
        ScoreText.text = aData.TotalScore.ToString();
    }

    public void SetDataBy(TableCategory aCategory, int aRank, SavingResultData aData)
    { }
}
