using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float[] noiseValues;
    void Start()
    {
        //Random.Range(0, 100);
        noiseValues = new float[10];
        for (int i = 0; i < noiseValues.Length; i++)
        {
            noiseValues[i] = Random.Range(0, 100);//Random.value;
            Debug.Log(noiseValues[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
