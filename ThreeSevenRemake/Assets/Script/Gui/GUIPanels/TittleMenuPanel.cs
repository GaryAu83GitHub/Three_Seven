﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TittleMenuPanel : MenuEnablePanelBase
{
    public Fading FadingScript;
    //public Text tempText;

    public delegate void OnStartTheGame();
    public static OnStartTheGame startTheGame;

    private enum ButtonIndex
    {
        PLAY_BUTTON,
        HIGHSCORE_BUTTON,
        OPTION_BUTTON,
        QUIT_BUTTON
    }
    
    private GUIPanelIndex mGoToIndex = GUIPanelIndex.NONE;


    public override void Start()
    {
        mPanelIndex = GUIPanelIndex.TITLE_PANEL;

        Buttons[(int)ButtonIndex.PLAY_BUTTON].onClick.AddListener(PlayGame);
        Buttons[(int)ButtonIndex.HIGHSCORE_BUTTON].onClick.AddListener(DisplayHighscore);
        Buttons[(int)ButtonIndex.OPTION_BUTTON].onClick.AddListener(OptionSetting);
        Buttons[(int)ButtonIndex.QUIT_BUTTON].onClick.AddListener(QuitGame);

        base.Start();
    }

    public override void Update()
    {
        base.Update();
        //tempText.text = mGoToIndex.ToString();
    }

    protected override void NavigateMenuButtons(CommandIndex theIncreaseCommand, CommandIndex theDecreaseCommand)
    {
        base.NavigateMenuButtons(CommandIndex.NAVI_DOWN, CommandIndex.NAVI_UP);
    }

    protected override void SelectButtonPressed()
    {
        switch(mCurrentSelectButtonIndex)
        {
            case (int)ButtonIndex.PLAY_BUTTON:
                startTheGame?.Invoke();
                mGoToIndex = GUIPanelIndex.NONE;
                break;
            case (int)ButtonIndex.HIGHSCORE_BUTTON:
                mGoToIndex = GUIPanelIndex.HIGHSCORE_PANEL;
                break;
            case (int)ButtonIndex.OPTION_BUTTON:
                mGoToIndex = GUIPanelIndex.OPTION_PANEL;
                break;
            case (int)ButtonIndex.QUIT_BUTTON:
                mGoToIndex = GUIPanelIndex.QUIT_GAME;
                break;
            default:
                mGoToIndex = GUIPanelIndex.NONE;
                break;
        }
        GUIPanelManager.Instance.GoTo(mGoToIndex);
    }
       
    public override void Enter()
    {
        base.Enter();
        //mGoToIndex = MenuPanelIndex.NONE;
        SetSelectedButton(0);
    }

    public void PlayGame()
    {
        //GameSettingPanel.SetActive(true);
        GameRoundManager.Instance.CreateNewData();
        //displaySettingPanel?.Invoke(Setting_Index.SET_DIFFICULTY);
    }

    public void DisplayHighscore()
    {
        //openHighscorePanel?.Invoke();
        //HighScorePanel.SetActive(true);
    }

    public void OptionSetting()
    {
        //OptionPanel.SetActive(true);
        //openOptionPanel?.Invoke();
    }

    public void InstructingGame()
    {
        //InstructionPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InstructingOut()
    {
        //InstructionPanel.SetActive(false);
    }

    IEnumerator GoToGame()
    {
        float fadeTime = FadingScript.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(1);
    }
}
