using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Script.Tools;

public class GameOverMenu : MonoBehaviour
{
    public static bool GameIsOver = false;

    public TextMeshProUGUI LevelResultText;
    public TextMeshProUGUI TimeSpendText;
    public TextMeshProUGUI BlockLandedText;
    public TextMeshProUGUI TotalScoreText;

    public Fading FadingScript;
    public Button LeaveButton;
    public GameObject LeavePanel;

    public GameObject GameOverMenuUI;

    private Animator PanelAnimation;
    private float myAnimationDuration = 5f;

    private void Start()
    {
        PanelAnimation = GameOverMenuUI.GetComponent<Animator>();
        AnimationClip[] clips = PanelAnimation.runtimeAnimatorController.animationClips;
        myAnimationDuration = clips[0].length;
        LeaveButton.onClick.AddListener(LeaveToMainMenu);
        main.finalResult += Result;
        LeavePanel.SetActive(false);
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
        StartCoroutine(LeavePanelAppear());
    }

    public void LeaveToMainMenu()
    {
        GameIsOver = false;
        LeavePanel.SetActive(false);
        ScreenTransistor.Instance.FadeToSceneWithIndex(0);
        //StartCoroutine(GoToStart());
    }

    IEnumerator LeavePanelAppear()
    {
        yield return new WaitForSeconds(myAnimationDuration);
        LeavePanel.SetActive(true);
    }

    IEnumerator GoToStart()
    {
        //yield return new WaitForSeconds(5);
        LeavePanel.SetActive(false);
        float fadeTime = FadingScript.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(0);
    }
}
