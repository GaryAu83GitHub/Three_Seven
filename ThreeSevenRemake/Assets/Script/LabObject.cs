using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabObject : MonoBehaviour
{
    private Camera mCamera;

    public delegate void OnMoving(Vector3 aScreenPos);
    public static OnMoving moving;

    public delegate void OnDisplayText(string aDisplayText);
    public static OnDisplayText display;

    void Start()
    {
        mCamera = Camera.main;
    }

    private void OnDestroy()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * .1f;
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * .1f;

        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.up * .1f;
        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.down * .1f;

        if (Input.GetKeyDown(KeyCode.Space))
            display?.Invoke("Hello World!");

        moving?.Invoke(transform.position);
    }
}
