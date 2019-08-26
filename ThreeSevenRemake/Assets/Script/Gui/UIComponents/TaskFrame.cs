using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskFrame : MonoBehaviour
{
    public GameObject TaskValueBox;
    public List<FormulaBoxNumberComponent> NumberBox;

    private Text ScoreText;
    private Text FormulaText;
    private Text FrameText;
    private Animation Animation;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText = TaskValueBox.transform.GetChild(0).GetComponent<Text>();
        FormulaText = TaskValueBox.transform.GetChild(1).GetComponent<Text>();
        FrameText = TaskValueBox.transform.GetChild(3).GetComponent<Text>();
        Animation = TaskValueBox.GetComponent<Animation>();

        DeactivateFormulaNumberBoxes();
    }
    
    public void SetTaskValue(int aTaskValue, int aTaskCubeCount)
    {
        DeactivateFormulaNumberBoxes();
        ActiveFormulaNumberBoxes(aTaskCubeCount);
        FrameText.text = aTaskValue.ToString();
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
            NumberBox[i].SetCubeValue();
        }
    }

}
