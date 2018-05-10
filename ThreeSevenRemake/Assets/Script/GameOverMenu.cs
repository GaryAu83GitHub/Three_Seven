using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public static bool GameIsOver = false;

    public TextMeshProUGUI LevelResultText;
    public TextMeshProUGUI TimeSpendText;
    public TextMeshProUGUI BlockLandedText;
    public TextMeshProUGUI TotalScoreText;

    public Fading FadingScript;
    public Button LeaveButton;

    public GameObject GameOverMenuUI;

    private Animator PanelAnimation;

    private void Start()
    {
        PanelAnimation = GameOverMenuUI.GetComponent<Animator>();
        //LeaveButton.onClick.AddListener(LeaveToMainMenu);
        main.finalResult += Result;
    }

    private void OnDisable()
    {
        main.finalResult -= Result;
    }

    public void Result(int aReachedLevel, string aSpendTimeString, int aBlockCount, int aTotalScore)
    {
        GameIsOver = true;
        GameOverMenuUI.SetActive(GameIsOver);
        LevelResultText.text = aReachedLevel.ToString();
        TimeSpendText.text = aSpendTimeString;
        BlockLandedText.text = aBlockCount.ToString();
        TotalScoreText.text = aTotalScore.ToString();
        PanelAnimation.Play("GameOverMenuIn");
        StartCoroutine(GoToStart());
    }

    public void LeaveToMainMenu()
    {
        GameIsOver = false;
        StartCoroutine(GoToStart());
    }

    IEnumerator GoToStart()
    {
        yield return new WaitForSeconds(5);
        float fadeTime = FadingScript.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(0);
    }
}
