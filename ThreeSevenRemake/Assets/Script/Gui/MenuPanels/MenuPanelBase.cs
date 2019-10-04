using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelBase : MonoBehaviour
{
    public Image BackgroundImage;
    public GameObject Container;
    public List<Button> Buttons;

    private Animation mAnimation;
    
    public virtual void Start()
    {
        mAnimation = GetComponent<Animation>();
    }

    public virtual void Update()
    {
        Input();
    }

    protected virtual void Input()
    {

    }

    public virtual void Enter()
    {
        Container.SetActive(true);
    }

    public virtual void Exit()
    {
        Container.SetActive(false);
    }
}
