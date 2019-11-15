using Assets.Script.Tools;
using System.Collections;
using UnityEngine;

public class TitleSceneMain : MonoBehaviour
{
    void Start()
    {
        TittleMenuPanel.startTheGame += StartTheGame;
        GUIPanelManager.Instance.StartWithPanel(GUIPanelIndex.TITLE_PANEL);
    }

    private void OnDestroy()
    {
        TittleMenuPanel.startTheGame -= StartTheGame;
    }

    void Update()
    {
        
    }

    private void StartTheGame()
    {
        GameRoundManager.Instance.SetUpGameRound();
        StartCoroutine(StartGameRound());
    }

    private IEnumerator StartGameRound()
    {
        yield return new WaitForSeconds(.5f);
        ScreenTransistor.Instance.FadeToSceneWithIndex(1);
    }
}
