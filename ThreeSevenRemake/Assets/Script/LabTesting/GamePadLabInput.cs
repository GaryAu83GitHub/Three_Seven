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
public class GamePadLabInput : MonoBehaviour
{
    public bool isButton;
    public bool isAxis;
    public GPB aName;
    public List<XBoxButton> Buttons;
    public AnalogueSticks Stick;
    
    private Vector3 startPos;
    private Transform thisTransform;
    private MeshRenderer mr;

    private List<string> nameList;

    private XBoxControl mControl;

    private DisplayState mCurrentDisplayState;

    private float mVisibleTimer = 0f;
    private const float VISIBLE_TIME = .05f;

    private bool mIsInputHold = false;
    private XBoxButton mCurrentHoldButton = XBoxButton.NONE;

    private void Awake()
    {
        GamepadLab.SetInputDisplayState += SetInputDisplayState;

        mControl = new XBoxControl();
        mCurrentDisplayState = DisplayState.DISPLAY_HOLD;
    }

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

    private void OnDestroy()
    {
        GamepadLab.SetInputDisplayState -= SetInputDisplayState;
    }

    // Update is called once per frame
    void Update()
    {
        if (mCurrentDisplayState == DisplayState.DISPLAY_PRESS)
            CheckForInputPress();
        else if (mCurrentDisplayState == DisplayState.DISPLAY_HOLD)
            CheckForInputHold();
        else if (mCurrentDisplayState == DisplayState.DISPLAY_PRESS_AND_RELEASE)
            CheckForInputPressAndRelease();
        else if (mCurrentDisplayState == DisplayState.DISPLAY_ANALOGUE)
            CheckForAnalogue();
    }

    public void ButtonPressed(bool aButtonPressed)
    {
        mr.enabled = !aButtonPressed;
    }

    private void SetInputDisplayState(DisplayState aState)
    {
        if (mVisibleTimer > 0f)
            return;

        mCurrentDisplayState = aState;
    }

    private void StartButtonVisibleTimer(bool isButtonPress)
    {
        if (isButtonPress)
            mVisibleTimer = VISIBLE_TIME;
    }

    public void AxisOnMove(Vector2 aMovement)
    {
        Vector3 direction = new Vector3(aMovement.x, aMovement.y, 0f);
        thisTransform.position = startPos + direction;
    }

    private void SetButtonHold(XBoxButton theHoldButton)
    {
        mCurrentHoldButton = theHoldButton;
        if (theHoldButton != XBoxButton.NONE)
            mIsInputHold = true;
        else
            mIsInputHold = false;
    }

    private void CheckForInputPress()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (mVisibleTimer <= 0f && mControl.InputPress(Buttons[i]))
                StartButtonVisibleTimer(true);
        }
        DisplayPress();
    }

    private void DisplayPress()
    {
        if (mVisibleTimer > 0f)
        {
            mr.enabled = false;
            mVisibleTimer -= Time.deltaTime;
            if (mVisibleTimer <= 0f)
                mVisibleTimer = 0f;
        }
        else
            mr.enabled = true;
    }

    private void CheckForInputHold()
    {
        if(mCurrentHoldButton == XBoxButton.NONE)
        { 
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (mControl.InputHold(Buttons[i]))
                {
                    SetButtonHold(Buttons[i]);
                    break;
                }
            }
        }
        else
        {
            int currentHoldButtonIndex = Buttons.IndexOf(mCurrentHoldButton);
            if (!mControl.InputHold(Buttons[currentHoldButtonIndex]))
                SetButtonHold(XBoxButton.NONE);
        }
        DisplayHold();
    }

    private void DisplayHold()
    {
        if(mIsInputHold)
            mr.enabled = false;
        else
            mr.enabled = true;
    }

    private void CheckForInputPressAndRelease()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (mVisibleTimer <= 0f && mControl.InputPressAndHold(Buttons[i]))
                StartButtonVisibleTimer(true);
        }
        DisplayPressAndRelease();
    }

    private void DisplayPressAndRelease()
    {
        DisplayPress();
    }

    private void CheckForAnalogue()
    {
        if (Stick == AnalogueSticks.NONE)
            return;

        DisplayAnalogue();
    }

    private void DisplayAnalogue()
    {
        Vector3 inputDirection = Vector3.zero;

        inputDirection.x = mControl.InputAnalogueDirection(Stick).x;
        inputDirection.y = mControl.InputAnalogueDirection(Stick).y;

        thisTransform.position = startPos + inputDirection;
    }

    private void MoveAnalogueSphere(Vector2Int aDir)
    {
        Vector3 inputDirection = Vector3.zero;

        thisTransform.position = startPos + new Vector3(aDir.x, aDir.y, 0f);

    }
}
