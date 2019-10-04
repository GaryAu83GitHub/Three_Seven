using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TittleMenuPanel : MenuPanelBase
{
    public Fading FadingScript;

    private enum ButtonIndex
    {
        PLAY_BUTTON,
        HIGHSCORE_BUTTON,
        OPTION_BUTTON,
        QUIT_BUTTON
    }
    

    public override void Start()
    {
        Buttons[(int)ButtonIndex.PLAY_BUTTON].onClick.AddListener(PlayGame);
        Buttons[(int)ButtonIndex.HIGHSCORE_BUTTON].onClick.AddListener(DisplayHighscore);
        Buttons[(int)ButtonIndex.OPTION_BUTTON].onClick.AddListener(OptionSetting);
        Buttons[(int)ButtonIndex.QUIT_BUTTON].onClick.AddListener(QuitGame);
    }

    protected override void Input()
    {
        base.Input();
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
