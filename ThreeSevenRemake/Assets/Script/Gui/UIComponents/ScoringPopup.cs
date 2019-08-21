using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScoringPopup : FloatPopupBase
{
    public Text ScoreText;

    private Animation mAnimation;

    public override void Start()
    {
        base.Start();
        mAnimation = GetComponent<Animation>();

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

    private void DisplayScore(Vector3 aWorldMidPos, int aDisplayScore)
    {
        AppearOnPosition(aWorldMidPos);
        ScoreText.text = aDisplayScore.ToString();
        mAnimation.Play();
    }
}
