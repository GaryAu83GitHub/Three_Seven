using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskBox : MonoBehaviour
{
    public TextMeshProUGUI TaskValueText;
    public TextMeshProUGUI TaskNumberText;
    public TextMeshProUGUI ScoringTimesText;

    public List<OperatorBoxBase> OperatorBoxes;

    public List<Color> TaskRankColors;

    public delegate void OnCreateNewBlock();
    public static OnCreateNewBlock createNewBlock;

    private bool mTaskAccomplished = false;
    public bool TaskAccomplished { get { return mTaskAccomplished; } }
    
    private int mScoringTimes = 0;
    public int ScoringTimes { get { return mScoringTimes; } }

    private Animator mAnimator;
    private CanvasGroup mCG;

    private int mActiveDigit = 0;
    private TaskData mDisplayingTaskData = new TaskData();

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mCG = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
    }

    void Update()
    {
    }

    public void SetTaskAnimationEvent()
    {
        SetUpTask(mDisplayingTaskData);
    }

    public void CreateNewBlockAnimationEvent()
    {
        TaskManagerNew.Instance.TaskIsChanging = false;
    }

    public void DisplayScoring(int aTotalScore, List<Cube> someScoringCube)
    {
        DisplayOperatorDigit(someScoringCube);
        mTaskAccomplished = true;
        mScoringTimes++;
        ScoringTimesText.text = mScoringTimes.ToString();
        mAnimator.SetTrigger("TaskScoring");
    }

    public void SetUpTask(TaskData aData)
    {
        if (!mDisplayingTaskData.Equals(aData))
            mDisplayingTaskData = new TaskData(aData);

        DeactivateFormulaNumberBoxes();
        ActiveNewOperativeBox(mDisplayingTaskData.LinkedCubes);
        TaskNumberText.text = mDisplayingTaskData.TaskCountNumber.ToString();
        TaskValueText.text = mDisplayingTaskData.TaskValue.ToString();
        mTaskAccomplished = false;
        mScoringTimes = 0;
    }

    public void AssignNewTaskData(TaskData aNextTaskData)
    {
        mDisplayingTaskData = aNextTaskData;
        mAnimator.SetTrigger("TaskAccomplished");
    }

    public void PlayDigitIdleAnimation()
    {
        int selectDigit = Random.Range(0, OperatorBoxes.Count);
        OperatorBoxes[selectDigit].PlayIdleAnimation();
    }

    public void PlayDigitScoringAnimation()
    {   
        for (int i = 0; i < OperatorBoxes.Count; i++)
            OperatorBoxes[i].PlayScoringAnimation();
    }

    public void PlayScoringAnimation()
    {
        mAnimator.SetTrigger("TaskScoring");
        PlayDigitScoringAnimation();
    }

    public void PlayAccomplishAnimaiton()
    {
        mAnimator.SetTrigger("TaskAccomplished");
    }

    private void DeactivateFormulaNumberBoxes()
    {
        for (int i = 0; i < OperatorBoxes.Count; i++)
            OperatorBoxes[i].DisplayOff();
    }

    private void ActiveNewOperativeBox(int aDigitCount)
    {
        mActiveDigit = aDigitCount;
        for (int i = 0; i < mActiveDigit; i++)
        {
            OperatorBoxes[i].DisplayOn(TaskRankColors[mActiveDigit-2]);
            OperatorBoxes[i].SetDigitText("_");
        }
    }

    private void DisplayOperatorDigit(List<Cube> someScoringCube)
    {
        for (int i = 0; i < someScoringCube.Count; i++)
            OperatorBoxes[i].SetDigitText(someScoringCube[i].Number.ToString());
    }
}
