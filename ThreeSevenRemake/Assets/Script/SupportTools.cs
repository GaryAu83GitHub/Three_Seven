using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTools
{
    public static Color GetCubeColorOf(int aCubeNumber)
    {
        Vector3 color = new Vector3();

        switch (aCubeNumber)
        {
            case 1:
                color = new Vector3(148f, 0f, 211f);  // violet 
                break;
            case 2:
                color = new Vector3(75f, 0f, 130f);  // indigo
                break;
            case 3:
                color = new Vector3(0f, 0f, 255f);  // blue
                break;
            case 4:
                color = new Vector3(0f, 255f, 0f);  // green
                break;
            case 5:
                color = new Vector3(255f, 255f, 0f);  // yellow
                break;
            case 6:
                color = new Vector3(255f, 127f, 0f);  // orange
                break;
            case 7:
                color = new Vector3(255f, 0f, 0f);  // red
                break;
            default:
                color = new Vector3(255f, 255f, 255f);  // white
                break;
        }
        return new Color(color.x / 255f, color.y / 255f, color.z / 255f);
    }

    public static int RNG(int aMinValue, int aMaxValue)
    {
        
        return Random.Range(aMinValue, aMaxValue);
    }
}
