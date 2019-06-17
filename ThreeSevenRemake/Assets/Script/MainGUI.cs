using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGUI : MonoBehaviour
{
    public BlockGUI NextBlockGUI;
    public Text ScoreText;
    public Text TimeText;
    public Text LevelText;

    private float mGameTimer = 0f;
    private string mGameTimeString = "";
    private bool mGameIsPlaying;

    // Use this for initialization
    void Awake ()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    private void Start()
    {
        mGameTimer = 0f;
        Oldmain.scoreChanging += UpdateScore;
        Oldmain.levelUpdate += UpdateLevel;
        Oldmain.createNewBlock += TransferNewBlock;
        Oldmain.gameIsPlaying += GameIsPlaying;
        Oldmain.spendTime += SpendingTime;
    }

    private void OnDisable()
    {
        Oldmain.scoreChanging -= UpdateScore;
        Oldmain.levelUpdate -= UpdateLevel;
        Oldmain.createNewBlock -= TransferNewBlock;
        Oldmain.gameIsPlaying -= GameIsPlaying;
        Oldmain.spendTime -= SpendingTime;
    }

    private void Update()
    {
        Clock();
    }

    public void UpdateScore(int aNewScore)
    {
        ScoreText.text = aNewScore.ToString();
    }

    public void UpdateLevel(int aNewLevel)
    {
        LevelText.text = aNewLevel.ToString();
    }

    public void TransferNewBlock(OldBlock aNewBlock)
    {
        aNewBlock.SetCubeNumbers(NextBlockGUI.NewNumber());
    }

    public void GameIsPlaying(bool anIsPlaying)
    {
        mGameIsPlaying = anIsPlaying;
    }

    public string SpendingTime()
    {
        return mGameTimeString;
    }

    private void Clock()
    {
        if (!mGameIsPlaying)
            return;

        mGameTimer += Time.deltaTime;

        int seconds = (int)(mGameTimer % 60);
        int minutes = (int)((mGameTimer / 60) % 60);
        int hours = (int)((mGameTimer / 3600) % 24);

        mGameTimeString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        TimeText.text = mGameTimeString;
    }

}
