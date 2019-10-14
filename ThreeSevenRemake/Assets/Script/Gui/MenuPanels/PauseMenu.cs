using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script.Tools;
using TMPro;

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
        {
            MenuButton.onClick.AddListener(Surrender);
            MenuButton.GetComponentInChildren<TextMeshProUGUI>().text = "SURRENDER";
        }
        else
        {
            MenuButton.onClick.AddListener(LeaveToMain);
            MenuButton.GetComponentInChildren<TextMeshProUGUI>().text = "LEAVE";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        if (ControlManager.Ins.GamePause())
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

    private void Surrender()
    {
        MenuButton.onClick.RemoveListener(Surrender);
        BlockManager.Instance.SurrenderGameRound();
    }

    public void LeaveToMain()
    {
        Time.timeScale = 1;
        GameIsPause = false;
        leaveTheGame?.Invoke();
        //SceneManager.LoadScene("Menu");
        //StartCoroutine(GoToStart());
        MenuButton.onClick.RemoveListener(LeaveToMain);
        ScreenTransistor.Instance.FadeToSceneWithIndex(0);
    }

    IEnumerator GoToStart()
    {
        float fadeTime = FadingScript.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(0);
    }
}
