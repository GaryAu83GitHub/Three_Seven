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
    // Start is called before the first frame update
    void Start()
    {
        mRT = GetComponent<RectTransform>();
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
