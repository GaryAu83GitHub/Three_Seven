using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultSlotBase : MonoBehaviour
{
    public TextMeshProUGUI IssueHeaderText;
    public TextMeshProUGUI ValueText;
    public TextMeshProUGUI RankText;
    public Image MedalImage;

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public void SetValue(string aDisplayResult)
    {
        if(ValueText != null)
            ValueText.text = aDisplayResult;
    }
}
