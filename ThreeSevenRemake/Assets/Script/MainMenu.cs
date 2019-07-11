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
    public Button InstructionButton;
    public Button QuitButton;

    public GameObject InstructionPanel;
    public GameObject GameSettingPanel;

    public delegate void OnDisplaySettingPanel(Setting_Issue aPanelIndex);
    public static OnDisplaySettingPanel displaySettingPanel;

    private void Start()
    {
        PlayButton.onClick.AddListener(PlayGame);
        InstructionButton.onClick.AddListener(InstructingGame);
        QuitButton.onClick.AddListener(QuitGame);

        InstructionPanel.SetActive(false);
        GameSettingPanel.SetActive(false);
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("Main");
        //FadingScript.gameObject.SetActive(true);
        //StartCoroutine(GoToGame());
        //ScreenTransistor.Instance.FadeToSceneWithIndex(1);
        //GameSettingPanel.SetActive(true);
        displaySettingPanel?.Invoke(Setting_Issue.SET_NAME);
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
