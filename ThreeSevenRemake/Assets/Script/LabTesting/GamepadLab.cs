using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DisplayState
{
    DISPLAY_PRESS,
    DISPLAY_HOLD,
    DISPLAY_PRESS_AND_RELEASE,
    DISPLAY_ANALOGUE,
};

public class GamepadLab : MonoBehaviour
{
    public Text StateText;

    public List<GamePadLabInput> Inputs;

    public delegate void OnSetInputDisplayState(DisplayState aState);
    public static OnSetInputDisplayState SetInputDisplayState;


    XBoxControl control;

    private void Start()
    {
        ControlManager.Ins.DefaultSetting();

        SetDisplayState(DisplayState.DISPLAY_PRESS);

        control = new XBoxControl();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SetDisplayState(DisplayState.DISPLAY_PRESS);
        else if (Input.GetKeyDown(KeyCode.F2))
            SetDisplayState(DisplayState.DISPLAY_HOLD);
        else if (Input.GetKeyDown(KeyCode.F3))
            SetDisplayState(DisplayState.DISPLAY_PRESS_AND_RELEASE);
        else if (Input.GetKeyDown(KeyCode.F4))
            SetDisplayState(DisplayState.DISPLAY_ANALOGUE);
    }

    private void SetDisplayState(DisplayState aState)
    {
        SetInputDisplayState?.Invoke(aState);
        StateText.text = aState.ToString();
    }

    public AnalogueSticks stick;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
        var left = control.InputAnalogueDirection(AnalogueSticks.L_STICK);
        var right = control.InputAnalogueDirection(AnalogueSticks.R_STICK);
        GUILayout.Label(string.Format("L: {0}", left));
        GUILayout.Label(string.Format("R: {0}", right));
        GUILayout.EndArea();
    }
}
