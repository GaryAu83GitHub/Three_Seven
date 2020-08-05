using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DisplayState
{
    DISPLAY_PRESS,
    DISPLAY_HOLD,
    DISPLAY_PRESS_AND_RELEASE,
};

public class GamepadLab : MonoBehaviour
{
    public Text StateText;

    public List<GamePadLabInput> Inputs;

    public delegate void OnSetInputDisplayState(DisplayState aState);
    public static OnSetInputDisplayState SetInputDisplayState;

    private void Start()
    {
        ControlManager.Ins.DefaultSetting();

        SetDisplayState(DisplayState.DISPLAY_PRESS);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SetDisplayState(DisplayState.DISPLAY_PRESS);
        else if (Input.GetKeyDown(KeyCode.F2))
            SetDisplayState(DisplayState.DISPLAY_HOLD);
        else if (Input.GetKeyDown(KeyCode.F3))
            SetDisplayState(DisplayState.DISPLAY_PRESS_AND_RELEASE);
    }

    private void SetDisplayState(DisplayState aState)
    {
        SetInputDisplayState?.Invoke(aState);
        StateText.text = aState.ToString();
    }
}
