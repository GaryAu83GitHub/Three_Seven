using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusDisplayObject : MonoBehaviour
{
    public Image BonusImage;
    public TextMeshProUGUI BonusValueText;

    private RectTransform mRT;
    private CanvasGroup mCG;

    private float mActiveValue = 0;

    private void Awake()
    {
        mRT = GetComponent<RectTransform>();
        mCG = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(mRT == null)
            mRT = GetComponent<RectTransform>();
        if(mCG == null)
            mCG = GetComponent<CanvasGroup>();
    }

    public void ActiveBonus(float aCurrentFillingValue)
    {
        if (aCurrentFillingValue >= mActiveValue)
            mCG.alpha = 1;
        else
            mCG.alpha = 0;
    }

    public void SetupBonusObject(Vector2 aPlacePosition, int aBonusValue, float anActiveValue)
    {
        mRT.anchoredPosition += aPlacePosition;
        BonusValueText.text = "x" + aBonusValue.ToString();
        mActiveValue = anActiveValue;
    }
}
