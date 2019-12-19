using Assets.Script.Tools;
using System.Collections;
using UnityEngine;

public class TitleSceneMain : MonoBehaviour
{
    void Start()
    {
        TittleMenuPanel.startTheGame += StartTheGame;
        //GUIPanelManager.Instance.StartWithPanel(GUIPanelIndex.TITLE_PANEL);
        StartCoroutine(MenyAppear());
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
        GameRoundManager.Instance.SetUpGameRound(GameMode.SURVIVAL);
        StartCoroutine(StartGameRound());
    }

    private IEnumerator StartGameRound()
    {
        yield return new WaitForSeconds(.5f);
        ScreenTransistor.Instance.FadeToSceneWithIndex(1);
    }

    private IEnumerator MenyAppear()
    {
        yield return new WaitForSeconds(1.5f);
        GUIPanelManager.Instance.StartWithPanel(GUIPanelIndex.TITLE_PANEL);
    }
}
