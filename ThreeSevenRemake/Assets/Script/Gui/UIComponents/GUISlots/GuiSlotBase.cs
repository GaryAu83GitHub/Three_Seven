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

    }

    public virtual void Start()
    {
        mCG = GetComponent<CanvasGroup>();
    }

    public virtual void Update()
    {

    }

    public virtual void ActivatingSlot(bool isActive)
    {
        mSlotActivate = isActive;
    }
}
