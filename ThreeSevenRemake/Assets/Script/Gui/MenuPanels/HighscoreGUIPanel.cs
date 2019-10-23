using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreGUIPanel : MenuPanelBase
{
    private enum ButtonIndex
    {
        EXIT_BUTTON,
    }

    private enum SortMode
    {
        BY_NAME,
        BY_LEVEL,
        BY_SCORE,
        BY_COMBO,
        BY_TIME,
    }

    private int mCurrentSortMode = 0;

    public override void Start()
    {
        mPanelIndex = MenuPanelIndex.HIGHSCORE_PANEL;

        //Buttons[(int)ButtonIndex.EXIT_BUTTON].onClick.AddListener(ExitHighscore);

        base.Start();
    }

    protected override void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    {
        if (ControlManager.Ins.MenuCancelButtonPressed())
            ExitHighscore();
    }



    private void ExitHighscore()
    {
        //gameObject.SetActive(false);
        MenuManager.Instance.GoTo(MenuPanelIndex.TITLE_PANEL);
    }
}
