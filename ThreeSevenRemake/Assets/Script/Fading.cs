using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public Texture2D FadeOutTexture;
    public float FadeSpeed = .8f;

    private int mDrawDepth = -1000;
    private float mAlpha = 1f;
    private int mFadeDir = -1;

    private void Start()
    {
        //FadeOutTexture.alphaIsTransparency = true;
    }

    private void OnGUI()
    {
        mAlpha += mFadeDir * FadeSpeed * Time.deltaTime;
        mAlpha = Mathf.Clamp01(mAlpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, mAlpha);
        GUI.depth = mDrawDepth;
        GUI.DrawTexture(new Rect(-(Screen.width / 2), -(Screen.height / 2), Screen.width * 2, Screen.height * 2), FadeOutTexture);
    }

    public float BeginFade(int aDirection)
    {
        mFadeDir = aDirection;
        return FadeSpeed;
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }
}
