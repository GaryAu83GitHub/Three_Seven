using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskFrame : MonoBehaviour
{
    public GameObject TaskValueBox;
    public List<FormulaBoxNumberComponent> NumberBox;
    public List<Color> TaskRankColors;

    private enum TaskRankColorIndex
    {
        X1_OUTLINE_COLOR,
        X1_INLINE_COLOR,
        X5_OUTLINE_COLOR,
        X5_INLINE_COLOR,
        X10_OUTLINE_COLOR,
        X10_INLINE_COLOR,
    }

    private Text ScoreText;
    private Text FormulaText;
    private Text FrameText;
    private Animation Animation;
    private Image CircleOutline;
    private Image CircleInline;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText = TaskValueBox.transform.GetChild(0).GetComponent<Text>();
        FormulaText = TaskValueBox.transform.GetChild(1).GetComponent<Text>();
        FrameText = TaskValueBox.transform.GetChild(3).GetComponent<Text>();
        Animation = TaskValueBox.GetComponent<Animation>();

        CircleOutline = TaskValueBox.GetComponent<Image>();
        CircleInline = TaskValueBox.transform.GetChild(2).GetComponent<Image>();

        DeactivateFormulaNumberBoxes();
    }
    
    public void SetTaskValue(int aTaskValue, int aTaskCubeCount)
    {
        DeactivateFormulaNumberBoxes();
        ActiveFormulaNumberBoxes(aTaskCubeCount);
        FrameText.text = aTaskValue.ToString();
    }

    public void SetUpTask(TaskData aData)
    {
        SetCircleColor(aData.Rank);

        DeactivateFormulaNumberBoxes();
        ActiveFormulaNumberBoxes(aData.LinkedCubes);
        FrameText.text = aData.TaskValue.ToString();
    }

    public void PlayAnimation()
    {
        Animation.Play();
    }

    public void DisplayScoring(int aTotalScore, List<Cube> someScoringCube)
    {
        ActiveFormulaNumberBoxes(someScoringCube);

        ScoreText.text = aTotalScore.ToString();
        FormulaText.text = "";
        Animation.Play();
    }

    private void DeactivateFormulaNumberBoxes()
    {
        for (int i = 0; i < NumberBox.Count; i++)
        {
            NumberBox[i].gameObject.SetActive(false);
        }
    }

    private void ActiveFormulaNumberBoxes(List<Cube> someScoringCube)
    {
        for (int i = 0; i < someScoringCube.Count; i++)
        {
            NumberBox[i].gameObject.SetActive(true);
            NumberBox[i].SetCubeValue(someScoringCube[i].Number);
        }
    }

    private void ActiveFormulaNumberBoxes(int aCubeCount)
    {
        for (int i = 0; i < aCubeCount; i++)
        {
            NumberBox[i].gameObject.SetActive(true);
            NumberBox[i].SetCubeValue(-1, false);
        }
    }

    private void SetCircleColor(TaskRank aRank)
    {
        int temp = 0;
        if (aRank == TaskRank.X5)
            temp = 1;
        else if (aRank == TaskRank.X10)
            temp = 2;

        TaskRankColorIndex colorIndex = (TaskRankColorIndex)(aRank + temp);
        CircleOutline.color = TaskRankColors[(int)colorIndex];
        CircleInline.color = TaskRankColors[(int)(colorIndex + 1)];

    }

}

