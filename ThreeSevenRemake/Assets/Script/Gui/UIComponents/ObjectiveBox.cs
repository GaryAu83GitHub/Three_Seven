using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBox : MonoBehaviour
{
    public List<TaskFrame> TaskFrames;

    void Start()
    {
        BlockManager.achieveScoring += DisplayScoring;
        TaskManager.achieveObjective += SetObjectiveNumbersFor;
    }

    private void OnDestroy()
    {
        BlockManager.achieveScoring -= DisplayScoring;
        TaskManager.achieveObjective -= SetObjectiveNumbersFor;
    }
    
    public void SetObjectiveNumbersFor(TaskRank anObjective, int aTaskValue)
    {
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
    }
}
