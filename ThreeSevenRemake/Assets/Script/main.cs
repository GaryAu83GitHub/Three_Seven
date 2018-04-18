using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

    private void Awake()
    {
        GridManager.Instance.GenerateGrid();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
