using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBox : MonoBehaviour
{
    public List<TaskFrame> TaskFrames;

    public delegate void OnPopupAppear(Vector3 aTargetPosition, List<Cube> someScoringCube, int aDisplayScore);
    public static OnPopupAppear popupAppear;

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

        Vector3 midPos = GetMidPointBetweenScoringCubes(someScoringCube[0].transform.position, someScoringCube[someScoringCube.Count - 1].transform.position);
        popupAppear?.Invoke(midPos, someScoringCube, totalScore);

        TaskFrames[(int)anObjective].DisplayScoring(totalScore, someScoringCube);
    }

    private Vector3 GetMidPointBetweenScoringCubes(Vector3 firstCubeWorldPos, Vector3 lastCubeWorldPos)
    {
        return Vector3.Lerp(firstCubeWorldPos, lastCubeWorldPos, .5f);
    }
}
