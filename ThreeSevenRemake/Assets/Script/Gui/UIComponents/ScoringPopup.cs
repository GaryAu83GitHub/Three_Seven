using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScoringPopup : FloatPopupBase
{
    public List<FormulaBoxNumberComponent> FormulaBoxes;
    public Text ScoreText;

    private Animation mAnimation;

    public override void Start()
    {
        base.Start();
        mAnimation = GetComponent<Animation>();

        for (int i = 0; i < FormulaBoxes.Count - 1; i++)
            FormulaBoxes[i].gameObject.SetActive(false);

        ObjectiveBox.popupAppear += DisplayScore;

    }

    private void OnDestroy()
    {
        ObjectiveBox.popupAppear -= DisplayScore;
    }

    protected override void AppearOnPosition(Vector3 aWorldPosition)
    {
        base.AppearOnPosition(aWorldPosition);
        mRect.anchoredPosition += (Vector2.up * 50f);
    }

    private void DisplayScore(Vector3 aWorldMidPos, List<Cube> someScoringCube, int aDisplayScore)
    {
        for (int i = 0; i < FormulaBoxes.Count - 1; i++)
            FormulaBoxes[i].gameObject.SetActive(false);

        SetupFormualBoxes(someScoringCube);
        AppearOnPosition(aWorldMidPos);
        ScoreText.text = aDisplayScore.ToString();
        mAnimation.Play();
    }

    private void SetupFormualBoxes(List<Cube> someCubes)
    {
        int totalValue = 0;
        for(int i = 0; i < someCubes.Count; i++)
        {
            totalValue += someCubes[i].Number;
            FormulaBoxes[i].gameObject.SetActive(true);
            FormulaBoxes[i].SetCubeValue(someCubes[i].Number);
        }
        FormulaBoxes[FormulaBoxes.Count - 1].SetCubeValue(totalValue, false);
    }
}
