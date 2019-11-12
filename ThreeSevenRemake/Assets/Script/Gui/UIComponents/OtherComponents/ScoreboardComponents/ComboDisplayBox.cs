using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboDisplayBox : ScoreboardComponentBase
{
    private Animation mComboAnimation;

    public override void Start()
    {
        mComboAnimation = GetComponent<Animation>();
        MainGamePanel.onAddCombo += UpdateCombo;
    }

    private void OnDestroy()
    {
        MainGamePanel.onAddCombo -= UpdateCombo;
    }

    protected override void ComponentsDisplay()
    {

    }

    private void UpdateCombo(int aComboCount)
    {
        if (aComboCount < 1)
            return;

        ValueText.text = aComboCount.ToString();
        mComboAnimation.Play();
    }
}
