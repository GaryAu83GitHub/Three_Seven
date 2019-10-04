using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum MenuPanelIndex
{
    TITLE_PANEL,
    HIGHSCORE_PANEL,
    OPTION_PANEL,
}

public class MenuManager : MonoBehaviour
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

    public List<MenuPanelIndex> Indexes;
    public List<MenuPanelBase> Panels;

    private Dictionary<MenuPanelIndex, MenuPanelBase> mScenePanels = new Dictionary<MenuPanelIndex, MenuPanelBase>();

    private MenuPanelBase mCurrentActiveMenuPanel;

    private void Start()
    {
        
    }

    public void GoTo(MenuPanelIndex aPanelIndex)
    {
        mCurrentActiveMenuPanel.Exit();
        mCurrentActiveMenuPanel = mScenePanels[aPanelIndex];
        mCurrentActiveMenuPanel.Enter();
    }

    public void LoadSceneMenuPanels()
    {
        mScenePanels.Clear();
        for (int i = 0; i < Indexes.Count; i++)
            mScenePanels.Add(Indexes[i], Panels[i]);
    }
}
