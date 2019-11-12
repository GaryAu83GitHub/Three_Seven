using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassicScoreBoardSlot : GuiSlotBase
{
    public List<ScoreboardComponentBase> Components;

    public TextMeshProUGUI TimeText;

    public TextMeshProUGUI LevelText;
    public Image LevelBar;
    public Image LevelUpArrow;
    public Image LevelDownArrow;

    public TextMeshProUGUI ComboText;
    public Image ComboBar;

    public override void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
        
    }

    public override void Update()
    {
        base.Update();
    }
}
