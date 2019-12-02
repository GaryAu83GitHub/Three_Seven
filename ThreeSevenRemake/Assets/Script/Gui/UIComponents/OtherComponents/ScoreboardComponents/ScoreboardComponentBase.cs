using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardComponentBase : MonoBehaviour
{
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI ValueText;

    private bool mGameIsPlaying = true;

    public virtual void Start()
    {
        MainGamePanel.gatherResultData += GatherResultData;

        ResetStartValue();
    }

    public virtual void OnDestroy()
    {
        MainGamePanel.gatherResultData -= GatherResultData;
    }

    public virtual void Update()
    {
        if (mGameIsPlaying)
            ComponentsDisplay();
    }

    protected virtual void ComponentsDisplay()
    {

    }

    protected virtual void GatherResultData(ref ResultData aData)
    { }

    public virtual void ResetStartValue()
    { }
}
