using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBox : MonoBehaviour
{
    public List<TaskFrame> TaskFrames;

    //public GameObject GreenObjective;
    //public GameObject BlueObjective;
    //public GameObject RedObjective;

    //private Text GreenScoreText;
    //private Text BlueScoreText;
    //private Text RedScoreText;

    //private Text GreenFormulaText;
    //private Text BlueFormulaText;
    //private Text RedFormulaText;

    //private Text GreenFrameText;
    //private Text BlueFrameText;
    //private Text RedFrameText;

    //private Animation GreenAnimation;
    //private Animation BlueAnimation;
    //private Animation RedAnimation;

    // Start is called before the first frame update
    void Start()
    {
        BlockManager.achieveScoring += DisplayScoring;
        TaskManager.achieveObjective += SetObjectiveNumbersFor;

        //GreenScoreText = GreenObjective.transform.GetChild(0).GetComponent<Text>();
        //BlueScoreText = BlueObjective.transform.GetChild(0).GetComponent<Text>();
        //RedScoreText = RedObjective.transform.GetChild(0).GetComponent<Text>();

        //GreenFormulaText = GreenObjective.transform.GetChild(1).GetComponent<Text>();
        //BlueFormulaText = BlueObjective.transform.GetChild(1).GetComponent<Text>();
        //RedFormulaText = RedObjective.transform.GetChild(1).GetComponent<Text>();

        //GreenFrameText = GreenObjective.transform.GetChild(3).GetComponent<Text>();
        //BlueFrameText = BlueObjective.transform.GetChild(3).GetComponent<Text>();
        //RedFrameText = RedObjective.transform.GetChild(3).GetComponent<Text>();

        //GreenAnimation = GreenObjective.GetComponent<Animation>();
        //BlueAnimation = BlueObjective.GetComponent<Animation>();
        //RedAnimation = RedObjective.GetComponent<Animation>();
    }

    private void OnDestroy()
    {
        BlockManager.achieveScoring -= DisplayScoring;
        TaskManager.achieveObjective -= SetObjectiveNumbersFor;
    }
    
    public void SetObjectiveNumbersFor(TaskRank anObjective, int aTaskValue)
    {
        //if(anObjective == TaskRank.X1)
        //    GreenFrameText.text = aTaskValue.ToString();
        //else if(anObjective == TaskRank.X5)
        //    BlueFrameText.text = aTaskValue.ToString();
        //else if(anObjective == TaskRank.X10)
        //    RedFrameText.text = aTaskValue.ToString();

        TaskFrames[(int)anObjective].SetTaskValue(aTaskValue);
    }
    
    private void DisplayScoring(TaskRank anObjective, List<Cube> someScoringCube)
    {
        int totalScore = ScoreCalculatorcs.LinkingScoreCalculation(anObjective, someScoringCube.Count);

        TaskFrames[(int)anObjective].DisplayScoring(totalScore, someScoringCube);

        string formula = "";
        
        for(int i = 0; i < someScoringCube.Count; i++)
        {
            
            formula += someScoringCube[i].Number.ToString();

            if (i < someScoringCube.Count - 1)
                formula += "+";
        }

        //if (anObjective == TaskRank.X1)
        //{
        //    GreenScoreText.text = totalScore.ToString();
        //    GreenFormulaText.text = formula;
        //    GreenAnimation.Play();
        //}
        //else if (anObjective == TaskRank.X5)
        //{
        //    BlueScoreText.text = totalScore.ToString();
        //    BlueFormulaText.text = formula;
        //    BlueAnimation.Play();
        //}
        //else if (anObjective == TaskRank.X10)
        //{
        //    RedScoreText.text = totalScore.ToString();
        //    RedFormulaText.text = formula;
        //    RedAnimation.Play();
        //}
        //PlayAnimationOn(anObjective);
    }
}
