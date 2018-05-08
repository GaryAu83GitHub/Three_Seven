using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Fading FadingScript;

    public Button PlayButton;
    public Button InstructionButton;
    public Button QuitButton;

    public GameObject InstructionPanel;

    private void Start()
    {
        PlayButton.onClick.AddListener(PlayGame);
        InstructionButton.onClick.AddListener(InstructingGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("Main");
        //FadingScript.gameObject.SetActive(true);
        StartCoroutine(GoToGame());
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
