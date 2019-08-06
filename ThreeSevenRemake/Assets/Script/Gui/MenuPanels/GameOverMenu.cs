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
    public TextMeshProUGUI MaxComboText;

    public Fading FadingScript;
    public Button LeaveButton;
    public GameObject LeavePanel;

    public GameObject GameOverMenuUI;

    public delegate void OnLeaveTheGame();
    public static OnLeaveTheGame leaveTheGame;

    private Animator PanelAnimation;
    private float myAnimationDuration = 5f;
    private int mScoreCounter = 0;
    private bool mShowResult = false;

    private void Start()
    {
        PanelAnimation = GameOverMenuUI.GetComponent<Animator>();
        AnimationClip[] clips = PanelAnimation.runtimeAnimatorController.animationClips;
        myAnimationDuration = clips[0].length;
        LeaveButton.onClick.AddListener(LeaveToMainMenu);
        
        DevelopeMain.finalResult += Result;
        LeavePanel.SetActive(false);
    }

    private void OnDisable()
    {
        
        DevelopeMain.finalResult -= Result;
    }

    private void Update()
    {
        if (mShowResult)
        {
            ShowResult();
            if(Input.anyKeyDown)
            {
                mScoreCounter = GameManager.Instance.CurrentScore;
                TotalScoreText.text = mScoreCounter.ToString();
            }

        }
    }

    public void Result()
    {
        GameIsOver = true;
        GameOverMenuUI.SetActive(GameIsOver);
        LevelResultText.text = LevelManager.Instance.CurrentLevel.ToString();
        TimeSpendText.text = GameManager.Instance.GameTimeString;
        BlockLandedText.text = GameManager.Instance.LandedBlockCount.ToString();
        MaxComboText.text = GameManager.Instance.MaxCombo.ToString();
        PanelAnimation.Play("GameOverMenuIn");
        StartCoroutine(DisplayResult());

        
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
        if (GameSettings.Instance.PlayerName.Length > 0)
        {
            HighScoreManager.Instance.Add(GameManager.Instance.GetRoundData());
        }
        leaveTheGame?.Invoke();
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

    IEnumerator DisplayResult()
    {
        yield return new WaitForSeconds(myAnimationDuration);
        mShowResult = true;
    }

    private void ShowResult()
    {
        if (mScoreCounter < GameManager.Instance.CurrentScore)
        {
            mScoreCounter++;
            TotalScoreText.text = mScoreCounter.ToString();
        }
        else
            LeavePanel.SetActive(true);
    }
}
