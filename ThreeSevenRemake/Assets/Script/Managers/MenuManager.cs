using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum MenuPanelIndex
{
    // Start Scene
    TITLE_PANEL,
    DIFFICULT_PANEL,
    HIGHSCORE_PANEL,
    OPTION_PANEL,
    QUIT_GAME,
    // Ingame scene
    MAIN_GAME_PANEL,
    PAUSE_PANEL,
    RESULT_PANEL,
    BACK_TO_START,
    NONE
}

public class MenuManager
{
    public static MenuManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new MenuManager();
            }
            return mInstance;
        }
    }
    private static MenuManager mInstance;

    private Dictionary<MenuPanelIndex, GUIPanelBase> mScenePanels = new Dictionary<MenuPanelIndex, GUIPanelBase>();

    private GUIPanelBase mCurrentActiveMenuPanel;

    private MenuPanelIndex mPreviousPanelIndex = MenuPanelIndex.NONE;

    public void AddPanel(MenuPanelIndex aPanelIndex, GUIPanelBase aPanel)
    {
        if (mScenePanels.ContainsKey(aPanelIndex))
            return;

        mScenePanels.Add(aPanelIndex, aPanel);
        mScenePanels[aPanelIndex].Exit();
        //if (mScenePanels.Count == 1)
        //    mCurrentActiveMenuPanel = mScenePanels[aPanelIndex];
        //if (mScenePanels.Count > 1)
        //    mScenePanels[aPanelIndex].Exit();
        if (mScenePanels.Count > 1)
        {
            mScenePanels = new Dictionary<MenuPanelIndex, GUIPanelBase>(GetSortedPanels());
        }
    }

    public void StartWithPanel(MenuPanelIndex aPanelIndex)
    {
        if (!mScenePanels.ContainsKey(aPanelIndex))
        {
            Debug.LogError("The panel with index " + aPanelIndex + " is not registrated in MenuManager");
            return;
        }
        mCurrentActiveMenuPanel = mScenePanels[aPanelIndex];
        mCurrentActiveMenuPanel.Enter();
    }

    public void GoTo(MenuPanelIndex aPanelIndex)
    {
        //Debug.Log(aPanelIndex);
        if (aPanelIndex == MenuPanelIndex.QUIT_GAME)
        {
            Application.Quit();
            return;
        }
        else if (!mScenePanels.ContainsKey(aPanelIndex))
            return;

        mPreviousPanelIndex = mCurrentActiveMenuPanel.PanelIndex;

        mCurrentActiveMenuPanel.Exit();
        mCurrentActiveMenuPanel = mScenePanels[aPanelIndex];
        mCurrentActiveMenuPanel.Enter();
    }

    public void GoBack()
    {
        mCurrentActiveMenuPanel.Exit();
        mCurrentActiveMenuPanel = mScenePanels[mPreviousPanelIndex];
        mCurrentActiveMenuPanel.Enter();
    }

    private Dictionary<MenuPanelIndex, GUIPanelBase> GetSortedPanels()
    {
        List<MenuPanelIndex> keys = mScenePanels.Keys.ToList();
        keys.Sort();
        Dictionary<MenuPanelIndex, GUIPanelBase> result = new Dictionary<MenuPanelIndex, GUIPanelBase>();
        for (int i = 0; i < keys.Count; i++)
            result.Add(keys[i], mScenePanels[keys[i]]);

        return result;
    }
}
