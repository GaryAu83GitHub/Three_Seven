using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GPB
{
    A_Button,
    B_Button,
    X_Button,
    Y_Button,
    L_Shoulder,
    R_Shoulder,
    Back_Button,
    Start_Button,
    L_Thumb_Button,
    R_Thumb_Button,
    L_Stick,
    Trigger,
    R_Stick,
    DPad,
}
public class GamepadLab : MonoBehaviour
{
    public bool isButton;
    public bool isAxis;
    public GPB aName;

    private Vector3 startPos;
    private Transform thisTransform;
    private MeshRenderer mr;

    private List<string> nameList;

    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
        startPos = thisTransform.position;
        mr = thisTransform.GetComponent<MeshRenderer>();

        nameList = new List<string>()
        {
            "A_Button",
            "B_Button",
            "X_Button",
            "Y_Button",
            "L_Shoulder",
            "R_Shoulder",
            "Back_Button",
            "Start_Button",
            "L_Thumb_Button",
            "R_Thumb_Button",
            "L_Stick_Hori_Axis",
            "L_Stick_Vert_Axis",
            "Trigger_Axis",
            "R_Stick_Hori_Axis",
            "R_Stick_Vert_Axis",
            "DPad_Hori_Axis",
            "DPad_Vert_Axis",
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (isButton)
        {
            mr.enabled = !Input.GetButton(nameList[(int)aName]);
        }

        if (isAxis)
        {
            Vector3 inputDirection = Vector3.zero;
            if (aName == GPB.L_Thumb_Button)
            {
                inputDirection.x = Input.GetAxis(nameList[10]);
                inputDirection.y = Input.GetAxis(nameList[11]);
            }
            if (aName == GPB.Trigger)
            {
                inputDirection.x = Input.GetAxis(nameList[12]);
            }
            if (aName == GPB.R_Thumb_Button)
            {
                inputDirection.x = Input.GetAxis(nameList[13]);
                inputDirection.y = Input.GetAxis(nameList[14]);
            }
            if (aName == GPB.DPad)
            {
                inputDirection.x = Input.GetAxis(nameList[15]);
                inputDirection.y = Input.GetAxis(nameList[16]);
            }
            thisTransform.position = startPos + inputDirection;
        }

        Debug.Log("Left stick (" + Input.GetAxis(nameList[10]).ToString() + " : " + Input.GetAxis(nameList[11]).ToString() + ")");
        Debug.Log("Trigger (" + Input.GetAxis(nameList[12]).ToString() + ")");
        Debug.Log("Right stick (" + Input.GetAxis(nameList[13]).ToString() + " : " + Input.GetAxis(nameList[14]).ToString() + ")");
        Debug.Log("DPad (" + Input.GetAxis(nameList[15]).ToString() + " : " + Input.GetAxis(nameList[16]).ToString() + ")");
    }
}
