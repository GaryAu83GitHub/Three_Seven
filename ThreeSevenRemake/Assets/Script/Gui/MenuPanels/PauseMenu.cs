using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script.Tools;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public Fading FadingScript;

    public Button ResumeButton;
    public Button MenuButton;

    public GameObject PauseMenuUI;

    public delegate void OnLeaveTheGame();
    public static OnLeaveTheGame leaveTheGame;


    private void Start()
    {
        ResumeButton.onClick.AddListener(Resume);
        MenuButton.onClick.AddListener(LeaveToMain);

        if (GameSettings.Instance.PlayerName.Any())
            MenuButton.GetComponentInChildren<Text>().text = "Give Up";
        else
            MenuButton.GetComponentInChildren<Text>().text = "Leave";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
                Resume();
            else
                Pause();

            
        }
    }

    private void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
        
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
        
    }

    public void LeaveToMain()
    {
        Time.timeScale = 1;
        GameIsPause = false;
        leaveTheGame?.Invoke();
        //SceneManager.LoadScene("Menu");
        //StartCoroutine(GoToStart());
        ScreenTransistor.Instance.FadeToSceneWithIndex(0);
    }

    IEnumerator GoToStart()
    {
        float fadeTime = FadingScript.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(0);
    }
}
