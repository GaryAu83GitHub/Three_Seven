using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Tools;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    public List<SettingPanelBase> SettingPanels;

    private Dictionary<Setting_Index, SettingPanelBase> mPanelList = new Dictionary<Setting_Index, SettingPanelBase>();

    private Setting_Index mCurrentDisplaySettingPanelIndex = Setting_Index.NONE;

    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        MainMenu.displaySettingPanel -= DisplayPanel;
        SettingPanelBase.displaySettingPanel -= DisplayPanel;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        MainMenu.displaySettingPanel += DisplayPanel;
        SettingPanelBase.displaySettingPanel += DisplayPanel;

        for (int i = 0; i < SettingPanels.Count; i++)
        {
            mPanelList.Add(SettingPanels[i].PanelIndex, SettingPanels[i]);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
    }

    public void DisplayPanel(Setting_Index anIndex)
    {
        if (anIndex == Setting_Index.FINISH_SETTING)
        {
            StartCoroutine(StartGameRound());
        }
        else if(anIndex == Setting_Index.LEAVE_TO_TITLE)
        {
            mPanelList[mCurrentDisplaySettingPanelIndex].SlideOutToRight();
            mCurrentDisplaySettingPanelIndex = Setting_Index.NONE;

            StartCoroutine(ReturnToTitle());
        }
        else
        { 
            if(mCurrentDisplaySettingPanelIndex == Setting_Index.NONE)
            {
                this.gameObject.SetActive(true);
                mPanelList[anIndex].InitBaseValue();
                mPanelList[anIndex].SlideInFromRight();
                mCurrentDisplaySettingPanelIndex = anIndex;
            }
            else if(anIndex > mCurrentDisplaySettingPanelIndex)
            {
                mPanelList[mCurrentDisplaySettingPanelIndex].SlideOutToLeft();
                mPanelList[anIndex].InitBaseValue();
                mPanelList[anIndex].SlideInFromRight();
                mCurrentDisplaySettingPanelIndex = anIndex;
            }
            else if(anIndex < mCurrentDisplaySettingPanelIndex)
            {
                mPanelList[mCurrentDisplaySettingPanelIndex].SlideOutToRight();
                mPanelList[anIndex].SlideInFromLeft();
                mCurrentDisplaySettingPanelIndex = anIndex;
            }
        }
    }

    private IEnumerator ReturnToTitle()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    private IEnumerator StartGameRound()
    {
        yield return new WaitForSeconds(.5f);
        //Objective.Instance.PrepareObjectives();
        GameRoundManager.Instance.SetUpGameRound();
        ScreenTransistor.Instance.FadeToSceneWithIndex(1);
    }
}
