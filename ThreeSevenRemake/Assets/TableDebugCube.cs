using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableDebugCube : MonoBehaviour
{
    public Text CubeText;


    private float width = 28f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCube(Color aColor, string aText)
    {
        GetComponent<Image>().color = aColor;
        CubeText.text = aText;
    }

    public void SetPosition(float aX, float aY)
    {
        Vector3 parentsPos = transform.parent.GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = new Vector3(parentsPos.x + (aX * width), parentsPos.y + (aY * width), parentsPos.z);
    }
}
