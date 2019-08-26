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
        TaskManager.achieveObjective += SetTaskNumbersFor;
    }

    private void OnDestroy()
    {
        BlockManager.achieveScoring -= DisplayScoring;
        TaskManager.achieveObjective -= SetTaskNumbersFor;
    }
    
    public void SetTaskNumbersFor(TaskRank anObjective, TaskData aTaskData/*int aTaskValue*/)
    {
        //TaskFrames[(int)anObjective].SetTaskValue(aTaskValue);
        TaskFrames[(int)anObjective].SetTaskValue(aTaskData.Number, aTaskData.CubeCount);
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
