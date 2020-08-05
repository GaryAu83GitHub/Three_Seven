using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsystemLab : MonoBehaviour
{
    SimpleInputTest SIT;

    int pressCount = 0;

    public bool isPressed = false;
    
    private void Awake()
    {
        SIT = new SimpleInputTest();
        SIT.GamePad.North_Press.performed += x => isPressed = x.ReadValue<bool>();
        
    }

    private void OnEnable()
    {
        SIT.Enable();
    }

    private void OnDisable()
    {
        SIT.Disable();
    }

    private void NorthPress()
    {
        pressCount++;
        Debug.Log("North button pressed " + pressCount.ToString());
    }

    private void SouthHold()
    {
        Debug.Log("South button hold");
    }
}
