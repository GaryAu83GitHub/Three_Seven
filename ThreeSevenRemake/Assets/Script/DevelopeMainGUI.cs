﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevelopeMainGUI : MonoBehaviour
{
    public BlockGUI NextBlockGUI;
    public Text ScoreText;
    public Text TimeText;
    public Text LevelText;
    //public Image TestImage;
    public GameObject DebugPanel;

    private float mGameTimer = 0f;
    private string mGameTimeString = "";
    private bool mGameIsPlaying;

    private float mLerpValue = 0f;

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

        //TestImage.color = Color.green;
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

        if(Input.GetKeyDown(KeyCode.P))
        {
            if (DebugPanel.GetComponent<CanvasGroup>().alpha >= 1f)
                DebugPanel.GetComponent<CanvasGroup>().alpha = 0f;
            else
                DebugPanel.GetComponent<CanvasGroup>().alpha = 1f;
            //mLerpValue += .125f;
            //if (mLerpValue > 1)
            //    mLerpValue = 0f;
            //TestImage.color = Color.Lerp(Color.green, Color.red, mLerpValue);
        }
    }

    public void UpdateScore(int aNewScore)
    {
        ScoreText.text = aNewScore.ToString();
    }

    public void UpdateLevel(int aNewLevel)
    {
        LevelText.text = aNewLevel.ToString();
    }

    public void TransferNewBlock(BlockDeveloping aNewBlock)
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
