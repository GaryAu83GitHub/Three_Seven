using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiSlotBase : MonoBehaviour
{
    public TextMeshProUGUI TitleText;

    protected CanvasGroup mCG;
    protected bool mSlotActivate = true;

    public virtual void Awake()
    {
        mCG = GetComponent<CanvasGroup>();
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {

    }

    public virtual void ActivatingSlot(bool isActive)
    {
        mSlotActivate = isActive;
    }
}
