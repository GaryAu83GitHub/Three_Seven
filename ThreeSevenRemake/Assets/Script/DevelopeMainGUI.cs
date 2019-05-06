using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevelopeMainGUI : MonoBehaviour
{
    public BlockGUI NextBlockGUI;
    public Text ScoreText;
    public Text TimeText;
    public Text LevelText;

    private float mGameTimer = 0f;
    private string mGameTimeString = "";
    private bool mGameIsPlaying;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    // Start is called before the first frame update
    private void Start()
    {
        DevelopeMain.scoreChanging += UpdateScore;
        DevelopeMain.levelUpdate += UpdateLevel;
        DevelopeMain.createNewBlock += TransferNewBlock;
        DevelopeMain.gameIsPlaying += GameIsPlaying;
    }

    private void OnDisable()
    {
        DevelopeMain.scoreChanging -= UpdateScore;
        DevelopeMain.levelUpdate -= UpdateLevel;
        DevelopeMain.createNewBlock -= TransferNewBlock;
        DevelopeMain.gameIsPlaying -= GameIsPlaying;
    }

    // Update is called once per frame
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

    public void TransferNewBlock(Block aNewBlock)
    {
        aNewBlock.SetCubeNumbers(NextBlockGUI.NewNumber());
    }

    public void GameIsPlaying(bool anIsPlaying)
    {
        mGameIsPlaying = anIsPlaying;
    }

    private void Clock()
    {
        if (!mGameIsPlaying)
            return;

        mGameTimer += Time.deltaTime;

        int seconds = (int)(mGameTimer % 60);
        int minutes = (int)((mGameTimer / 60) % 60);
        int hours = (int)((mGameTimer / 3600) % 60);

        mGameTimeString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        TimeText.text = mGameTimeString;
    }
}
