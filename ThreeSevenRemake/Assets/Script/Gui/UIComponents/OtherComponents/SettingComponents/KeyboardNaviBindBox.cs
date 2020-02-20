using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardNaviBindBox : MonoBehaviour
{
    public TextMeshProUGUI BindText;
    public Image IconImage;

    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;

    private Image mBackgroundImage;


    void Start()
    {
        mBackgroundImage = GetComponent<Image>();        
    }

    public void SetBindingText(string aBindingText)
    {
        BindText.text = aBindingText;
    }

    public void BoxSelected(bool isSelected)
    {
        if (isSelected)
            mBackgroundImage.sprite = SelectedSprite;
        else
            mBackgroundImage.sprite = UnSelectedSprite;
    }
}
