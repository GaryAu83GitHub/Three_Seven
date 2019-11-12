using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanelBase : MonoBehaviour
{
    protected enum ButtonSpriteIndex
    {
        DEFAULT,
        SELECTED,
    }

    public Image BackgroundImage;
    public GameObject Container;

    public GUIPanelIndex PanelIndex { get { return mPanelIndex; } }
    protected GUIPanelIndex mPanelIndex = GUIPanelIndex.NONE;

    private void Awake()
    {
        ControlManager.Ins.DefaultSetting();
    }

    public virtual void Start()
    {
        GUIPanelManager.Instance.AddPanel(mPanelIndex, this);
    }

    public virtual void Update()
    {
    }

    public virtual void Enter()
    {
        this.gameObject.SetActive(true);
        Container.SetActive(true);
    }

    public virtual void Exit()
    {
        this.gameObject.SetActive(false);
        Container.SetActive(false);
    }

    protected virtual void ChangePanel(GUIPanelIndex aPanelIndex)
    {
        GUIPanelManager.Instance.GoTo(aPanelIndex);
    }
}
