using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FloatPopupBase : MonoBehaviour
{
    protected RectTransform mRect;
    protected float mScaleFactor = 0f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        mRect = GetComponent<RectTransform>();
        mScaleFactor = GetComponentInParent<Canvas>().scaleFactor;
    }
    
    protected virtual void AppearOnPosition(Vector3 aWorldPosition)
    {
        mRect.anchoredPosition = GetScreenPositionAt(aWorldPosition);
    }

    protected Vector2 GetScreenPositionAt(Vector3 aTargetPosition)
    {
        Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(aTargetPosition);
        return new Vector2(worldToScreenPos.x / mScaleFactor, worldToScreenPos.y / mScaleFactor);
    }
    // camera pos (2.25,4.5,-10)
}
