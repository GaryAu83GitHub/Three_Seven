using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaBoxNumberComponent : MonoBehaviour
{
    public Image CubeBox;
    public Text NumberText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCubeValue(int aNumber = -1, bool displayBox = true)
    {
        NumberText.gameObject.SetActive(true);
        CubeBox.gameObject.SetActive(true);

        CubeBox.enabled = displayBox;
        CubeBox.color = SupportTools.GetCubeHexColorOf(aNumber);
        NumberText.color = Color.black;
        NumberText.text = aNumber.ToString();

        if (aNumber == -1)
        {
            CubeBox.enabled = false;
            NumberText.color = Color.white;
            NumberText.text = "?";
        }
    }

    public void DeactivateCube()
    {
        NumberText.gameObject.SetActive(false);
        CubeBox.gameObject.SetActive(false);
    }
}
