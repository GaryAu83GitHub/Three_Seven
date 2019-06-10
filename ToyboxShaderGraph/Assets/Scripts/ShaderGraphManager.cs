using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderGraphManager : MonoBehaviour
{
    public Material dissolveMat;
    public float mTime = 0f;

    void Start()
    {
        dissolveMat.SetColor("Color_BC34DD5C", Color.red);    
    }
    
    void Update()
    {
     
    }
}
