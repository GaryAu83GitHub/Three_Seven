using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script.Tools;

public class MainMenu : MenuEnablePanelBase
{
    public Fading FadingScript;

    public Button PlayButton;
    public Button OptionButton;
    public Button HighScoreTableButton;
    public Button InstructionButton;
    public Button QuitButton;

    public GameObject OptionPanel;
    public GameObject InstructionPanel;
    public GameObject GameSettingPanel;
    public GameObject HighScorePanel;

    public delegate void OnDisplaySettingPanel(Setting_Index aPanelIndex);
    public static OnDisplaySettingPanel displaySettingPanel;

    public delegate void OnOpenOptionPanel();
    public static OnOpenOptionPanel openOptionPanel;

    public delegate void OnOpenHighscorePanel();
    public static OnOpenHighscorePanel openHighscorePanel;

    private void Awake()
    {
        ControlManager.Ins.DefaultSetting();   
    }
    
    public override void Start()
    {
        PlayButton.onClick.AddListener(PlayGame);
        HighScoreTableButton.onClick.AddListener(DisplayHighscore);
        InstructionButton.onClick.AddListener(InstructingGame);
        OptionButton.onClick.AddListener(OptionSetting);
        QuitButton.onClick.AddListener(QuitGame);

        InstructionPanel.SetActive(false);
        OptionPanel.SetActive(false);
        GameSettingPanel.SetActive(false);
        HighScorePanel.SetActive(false);
    }
    
    //private void OnGUI()
    //{
    //    Event e = Event.current;
    //    if(e.isKey)
    //        Debug.Log("Detected key code: " + e.keyCode);
    //}

    protected override void CheckInput()
    {
        base.CheckInput();
        if (ControlManager.Ins.KeyPress(CommandIndex.CONFIRM))
            PlayGame();
    }

    public void PlayGame()
    {
        GameSettingPanel.SetActive(true);
        GameRoundManager.Instance.CreateNewData();
        displaySettingPanel?.Invoke(Setting_Index.SET_DIFFICULTY);
    }

    public void DisplayHighscore()
    {
        openHighscorePanel?.Invoke();
        HighScorePanel.SetActive(true);
    }

    public void OptionSetting()
    {
        OptionPanel.SetActive(true);
        openOptionPanel?.Invoke();
    }

    public void InstructingGame()
    {
        InstructionPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InstructingOut()
    {
        InstructionPanel.SetActive(false);
    }

    IEnumerator GoToGame()
    {
        float fadeTime = FadingScript.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(1);
    }
}
