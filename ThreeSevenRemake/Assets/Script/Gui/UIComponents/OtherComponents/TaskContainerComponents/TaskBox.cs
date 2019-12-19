using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskBox : MonoBehaviour
{
    public TextMeshProUGUI TaskValueText;
    public TextMeshProUGUI TaskNumberText;
    //public TextMeshProUGUI MultiValueText;
    public TextMeshProUGUI ScoringTimesText;

    //public Image MultiValueFilling;
    //public Image MultiValueOutline;

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

    //private enum TaskRankColorIndex
    //{
    //    X1_OUTLINE_COLOR,
    //    X1_INLINE_COLOR,
    //    X5_OUTLINE_COLOR,
    //    X5_INLINE_COLOR,
    //    X10_OUTLINE_COLOR,
    //    X10_INLINE_COLOR,
    //}

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
        //if (Input.GetKeyDown(KeyCode.Space))
        //    mAnimator.SetTrigger("TaskScoring");
        //if (Input.GetKeyDown(KeyCode.Return))
        //    mAnimator.SetTrigger("TaskAccomplished");
    }

    public void SetTaskAnimationEvent()
    {
        SetUpTask(mDisplayingTaskData);
    }

    public void CreateNewBlockAnimationEvent()
    {
        //createNewBlock?.Invoke();
    }

    public void DisplayScoring(int aTotalScore, List<Cube> someScoringCube)
    {
        DisplayOperatorDigit(someScoringCube);
        mTaskAccomplished = true;
        mScoringTimes++;
        ScoringTimesText.text = mScoringTimes.ToString();
        //ScoreText.text = aTotalScore.ToString();
        //FormulaText.text = "";
        //Animation.Play();
        mAnimator.SetTrigger("TaskScoring");
    }

    public void SetUpTask(TaskData aData)
    {
        //MultiValueOutline.gameObject.SetActive(true);

        //SetRankCircle(aData.Rank);

        DeactivateFormulaNumberBoxes();
        ActiveNewOperativeBox(aData.LinkedCubes);
        TaskNumberText.text = aData.TaskCountNumber.ToString();
        TaskValueText.text = aData.TaskValue.ToString();
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

    private void SetRankCircle(TaskRank aRank)
    {
        //int temp = 0;
        //if (aRank == TaskRank.X5)
        //    temp = 1;
        //else if (aRank == TaskRank.X10)
        //    temp = 2;

        //MultiValueText.text = aRank.ToString();

        //TaskRankColorIndex colorIndex = (TaskRankColorIndex)(aRank + temp);
        //MultiValueOutline.color = TaskRankColors[(int)colorIndex];
        //MultiValueFilling.color = TaskRankColors[(int)(colorIndex + 1)];

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
        {
            //OperatorBoxes[i].gameObject.SetActive(true);
            OperatorBoxes[i].SetDigitText(someScoringCube[i].Number.ToString());
        }
    }
}
