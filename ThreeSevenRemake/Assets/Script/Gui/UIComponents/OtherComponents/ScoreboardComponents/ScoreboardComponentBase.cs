using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This is the father class for the component that with the basic UI component is
/// </summary>
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

    /// <summary>
    /// The base update method for all score board component child class and it's main responsible
    /// is to update the display of each child class when the game is playing.
    /// </summary>
    public virtual void Update()
    {
        if (mGameIsPlaying)
            ComponentsDisplay();
    }

    /// <summary>
    /// The virtual method for the child to display their respective graphic
    /// </summary>
    protected virtual void ComponentsDisplay() { }

    /// <summary>
    /// The virtual method for the child class to store in their respective subject
    /// and sub issue value into the reference result data, and at the same time is to reset the
    /// start value each child class has assign to the ResetStartValue method.
    /// This method will be subscrbing with the gatherResultData delegate method in MainGamePanel
    /// </summary>
    /// <param name="aData">The result data which will be recieving the child responsible
    /// subject and issue value</param>
    protected virtual void GatherResultData(ref ResultData aData)
    {
        ResetStartValue();
    }

    /// <summary>
    /// The virtual method for the child class to reset their storing value
    /// </summary>
    public virtual void ResetStartValue() { }
}
