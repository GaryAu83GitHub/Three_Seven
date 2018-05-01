using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public BlockGUI NextBlockGUI;
    public Text ScoreText;
    public Text TimeText;
    public Text LevelText;

    private float mGameTimer = 0f;

    // Use this for initialization
    void Awake ()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    private void Start()
    {
        main.scoreChanging += UpdateScore;
        main.levelUpdate += UpdateLevel;
        main.createNewBlock += TransferNewBlock;
    }

    private void OnDisable()
    {
        main.scoreChanging -= UpdateScore;
        main.levelUpdate -= UpdateLevel;
        main.createNewBlock -= TransferNewBlock;
    }

    private void Update()
    {
        mGameTimer += Time.deltaTime;

        int seconds = (int)(mGameTimer % 60);
        int minutes = (int)((mGameTimer / 60) % 60);
        int hours = (int)((mGameTimer / 3600) % 24);

        string timerString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        TimeText.text = timerString;
    }

    public void UpdateScore(int aNewScore)
    {
        ScoreText.text = aNewScore.ToString();
    }

    public void UpdateLevel(int aNewLevel)
    {
        LevelText.text = aNewLevel.ToString();
    }

    public void TransferNewBlock(Block aNewBlock)
    {
        aNewBlock.SetCubeNumbers(NextBlockGUI.NewNumber());
    }



}
