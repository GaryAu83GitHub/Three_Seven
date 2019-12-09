using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : GUIPanelBase
{
    private enum ResultIssues
    {
        SCORE,
        CHAIN,
        TASKS,
        LEVEL,
        ODDS,
        TIME,
    }

    public List<ResultSlotBase> ResultSlots;
    public GameObject NameInput;
    public TMP_InputField PlayerNameInputField;

    public delegate void OnActiveRegistrateButtons(bool anActivation);
    public static OnActiveRegistrateButtons activeRegistrateButtons;

    public delegate void OnLeaveGameScene();
    public static OnLeaveGameScene leaveGameScene;

    private Animator mAnimator;
    private ResultData mResultData = new ResultData();

    private string mPlayerName = "";
    private bool mTempLeaveButtonEnable = false;

    public override void Start()
    {
        mPanelIndex = GUIPanelIndex.RESULT_PANEL;
        base.Start();

        MainGamePanel.sendingResultData += SetUpResult;
        SaveResultButtonsSlot.selectSaveRusultBotton += SelectSaveRusultBotton;

        mAnimator = GetComponent<Animator>();

        NameInput.SetActive(false);
        PlayerNameInputField.onValueChanged.AddListener(delegate { PlayerNameInputFieldOnValueChange(PlayerNameInputField); });

        mTempLeaveButtonEnable = false;
    }

    private void OnDestroy()
    {
        MainGamePanel.sendingResultData -= SetUpResult;
        SaveResultButtonsSlot.selectSaveRusultBotton -= SelectSaveRusultBotton;
    }

    public override void Update()
    {
        base.Update();
        ConfirmButtonInput();
    }

    private void ConfirmButtonInput()
    {
        //if (!NameInput.activeInHierarchy)
        //    return;
        //if (!mTempLeaveButtonEnable)
        //    return;

        if (ControlManager.Ins.MenuConfirmButtonPressed())
        {
            if (mPlayerName.Length > 0)
            {
                mResultData.SetPlayerName(mPlayerName);
                // recording to highscore
                HighScoreManager.Instance.AddNewScore(mPlayerName, mResultData);
            }

            mAnimator.SetTrigger("ExitNameInput");
            //mAnimator.SetBool("LeaveGame", true);
        }
    }

    public void ActiveRegistrateButtonsAnimationEvent()
    {
        activeRegistrateButtons?.Invoke(true);
        //mAnimator.SetTrigger("ExitNameInput");
        //mTempLeaveButtonEnable = true;
    }

    public void LeaveGameSceneAnimationEvent()
    {
        //gameObject.SetActive(false);
        Container.SetActive(false);
        leaveGameScene?.Invoke();
    }

    public void DeactivateNameInputAnimationEvent()
    {
        NameInput.SetActive(false);
        mAnimator.SetBool("LeaveGame", true);
    }

    private void PlayerNameInputFieldOnValueChange(TMP_InputField anInput)
    {
        mPlayerName = anInput.text;
    }


    private void SetUpResult(ResultData aData)
    {
        mResultData = new ResultData(aData);

        ResultSlots[(int)ResultIssues.SCORE].SetValue(aData.GainScores.ToString());
        ResultSlots[(int)ResultIssues.CHAIN].SetValue(aData.LongestChains.ToString());
        ResultSlots[(int)ResultIssues.TASKS].SetValue(aData.CompletedTasks.ToString());
        ResultSlots[(int)ResultIssues.LEVEL].SetValue(aData.ReachedLevels.ToString());
        ResultSlots[(int)ResultIssues.ODDS].SetValue((Mathf.Round(aData.AverageOdds * 100) / 100f).ToString());
        ResultSlots[(int)ResultIssues.TIME].SetValue(aData.TimeString);
    }

    private void SelectSaveRusultBotton(int aSelectIndex)
    {
        if(aSelectIndex == 0)
        {
            NameInput.SetActive(true);
            // trigger animation to call forth name input
            mAnimator.SetTrigger("EnterNameInput");
            PlayerNameInputField.Select();
        }
        else if(aSelectIndex == 1)
        {
            // trigger animation to put away the result panel
            mAnimator.SetBool("LeaveGame", true);
        }
    }
}
