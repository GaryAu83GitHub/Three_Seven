using UnityEngine;
using TMPro;

public class OperatorBoxBase : MonoBehaviour
{
    public TextMeshProUGUI OperatorText;
    public TextMeshProUGUI DigitText;

    public string OperatorString;

    //private Animation mAnimation;
    private Animator mAnimator;
    private CanvasGroup mCG;

    public virtual void Start()
    {
        //mAnimation = GetComponent<Animation>();
        mAnimator = GetComponent<Animator>();
        mCG = GetComponent<CanvasGroup>();
    }

    public virtual void Update()
    {
        
    }

    public void SetDigitText(string aDigitText)
    {
        OperatorText.text = OperatorString;
        DigitText.text = aDigitText;
        
    }

    public void DisplayOff()
    {
        mCG.alpha = 0;
        DigitText.color = Color.clear;
    }

    public void DisplayOn(Color aDisplayColor)
    {
        mCG.alpha = 1;
        DigitText.color = aDisplayColor;
    }

    public void PlayIdleAnimation()
    {
        //mAnimation.Play();
        mAnimator.SetTrigger("DigitIdle");
    }

    public void PlayScoringAnimation()
    {
        mAnimator.SetTrigger("DigitScoring");
    }
}
