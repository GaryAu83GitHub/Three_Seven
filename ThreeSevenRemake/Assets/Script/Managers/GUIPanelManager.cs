using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum GUIPanelIndex
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

public class GUIPanelManager
{
    public static GUIPanelManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GUIPanelManager();
            }
            return mInstance;
        }
    }
    private static GUIPanelManager mInstance;

    private Dictionary<GUIPanelIndex, GUIPanelBase> mScenePanels = new Dictionary<GUIPanelIndex, GUIPanelBase>();

    private GUIPanelBase mCurrentActiveMenuPanel;

    private GUIPanelIndex mPreviousPanelIndex = GUIPanelIndex.NONE;

    public void AddPanel(GUIPanelIndex aPanelIndex, GUIPanelBase aPanel)
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
            mScenePanels = new Dictionary<GUIPanelIndex, GUIPanelBase>(GetSortedPanels());
        }
    }

    public void StartWithPanel(GUIPanelIndex aPanelIndex)
    {
        if (!mScenePanels.ContainsKey(aPanelIndex))
        {
            Debug.LogError("The panel with index " + aPanelIndex + " is not registrated in MenuManager");
            return;
        }
        mCurrentActiveMenuPanel = mScenePanels[aPanelIndex];
        mCurrentActiveMenuPanel.Enter();
    }

    public void GoTo(GUIPanelIndex aPanelIndex)
    {
        //Debug.Log(aPanelIndex);
        if (aPanelIndex == GUIPanelIndex.QUIT_GAME)
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

    private Dictionary<GUIPanelIndex, GUIPanelBase> GetSortedPanels()
    {
        List<GUIPanelIndex> keys = mScenePanels.Keys.ToList();
        keys.Sort();
        Dictionary<GUIPanelIndex, GUIPanelBase> result = new Dictionary<GUIPanelIndex, GUIPanelBase>();
        for (int i = 0; i < keys.Count; i++)
            result.Add(keys[i], mScenePanels[keys[i]]);

        return result;
    }
}
