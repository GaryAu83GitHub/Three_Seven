using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script.Tools;

public class MainMenu : MonoBehaviour
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

    private void Start()
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

        //GameRoundData p1 = new GameRoundData
        //{
        //    CurrentLevel = 12,
        //    CurrentScore = 5500,
        //    MaxCombo = 10,
        //    GameTime = 450f,
        //    LandedBlockCount = 40,
        //    EnableScoringMethods = new List<bool>() { false, true, false, true }
        //};

        //GameRoundData p2 = new GameRoundData
        //{
        //    CurrentLevel = 15,
        //    CurrentScore = 7500,
        //    MaxCombo = 6,
        //    GameTime = 600f,
        //    LandedBlockCount = 40,
        //    EnableScoringMethods = new List<bool>() { false, false, true, true }
        //};

        //HighScoreManager.Instance.Add("Benjamin", p1);
        //HighScoreManager.Instance.Add("Anita", p2);
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("Main");
        //FadingScript.gameObject.SetActive(true);
        //StartCoroutine(GoToGame());
        //ScreenTransistor.Instance.FadeToSceneWithIndex(1);
        GameSettingPanel.SetActive(true);
        GameRoundManager.Instance.CreateNewData();
        displaySettingPanel?.Invoke(Setting_Index.SET_LINK);

        //for(int i = 0; i < 10; i++)
        //{
        //    GameRoundData data = new GameRoundData
        //    {
        //        PlayerName = "Test table " + (i + 1),
        //        CurrentLevel = Random.Range(0, 50),
        //        CurrentScore = Random.Range(5000, 10000),
        //        MaxCombo = Random.Range(2, 10),
        //        GameTime = Random.Range(200, 1500),
        //        LandedBlockCount = Random.Range(50, 100),
        //        EnableScoringMethods = new List<bool>() { true, true, true, true }
        //    };
        //    HighScoreManager.Instance.Add(data);
        //}

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
