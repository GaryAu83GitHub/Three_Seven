using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum MenuPanelIndex
{
    TITLE_PANEL,
    DIFFICULT_PANEL,
    HIGHSCORE_PANEL,
    OPTION_PANEL,
    QUIT_GAME,
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

    private Dictionary<MenuPanelIndex, MenuPanelBase> mScenePanels = new Dictionary<MenuPanelIndex, MenuPanelBase>();

    private MenuPanelBase mCurrentActiveMenuPanel;

    private MenuPanelIndex mPreviousPanelIndex = MenuPanelIndex.NONE;

    public void AddPanel(MenuPanelIndex aPanelIndex, MenuPanelBase aPanel)
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
            mScenePanels = new Dictionary<MenuPanelIndex, MenuPanelBase>(GetSortedPanels());
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

    private Dictionary<MenuPanelIndex, MenuPanelBase> GetSortedPanels()
    {
        List<MenuPanelIndex> keys = mScenePanels.Keys.ToList();
        keys.Sort();
        Dictionary<MenuPanelIndex, MenuPanelBase> result = new Dictionary<MenuPanelIndex, MenuPanelBase>();
        for (int i = 0; i < keys.Count; i++)
            result.Add(keys[i], mScenePanels[keys[i]]);

        return result;
    }
}
