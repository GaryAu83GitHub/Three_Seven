using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveableGUIText : MonoBehaviour
{
    public Text TestText;

    private RectTransform mRect;
    private Animation mAnimation;
    private float mScaleFactor = 0f;

    void Start()
    {
        LabObject.moving += ObjectOnMove;
        LabObject.display += ObjectOnDisplayContain;

        mRect = GetComponent<RectTransform>();
        mAnimation = GetComponent<Animation>();
        mScaleFactor = GetComponentInParent<Canvas>().scaleFactor;
    }

    private void OnDestroy()
    {
        LabObject.moving -= ObjectOnMove;
        LabObject.display -= ObjectOnDisplayContain;
    }
    
    private void ObjectOnMove(Vector3 aTargetPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(aTargetPosition);
        mRect.anchoredPosition = new Vector2(screenPos.x / mScaleFactor, screenPos.y / mScaleFactor) + (Vector2.up * 25);
        
    }

    private void ObjectOnDisplayContain(string aDisplayText)
    {
        if (mAnimation.isPlaying)
            return;

        TestText.text = aDisplayText;
        mAnimation.Play();
    }
}
