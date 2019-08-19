using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaBoxNumberComponent : MonoBehaviour
{
    public Text NumberText;
    public Image CubeBox;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCubeValue(int aNumber)
    {
        NumberText.gameObject.SetActive(true);
        CubeBox.gameObject.SetActive(true);

        NumberText.text = aNumber.ToString();
        CubeBox.color = SupportTools.GetCubeHexColorOf(aNumber);
    }

    public void DeactivateCube()
    {
        NumberText.gameObject.SetActive(false);
        CubeBox.gameObject.SetActive(false);
    }
}
