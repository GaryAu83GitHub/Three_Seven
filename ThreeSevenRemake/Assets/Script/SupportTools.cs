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
                color = new Vector3(180, 95f, 211f);  // violet 
                break;
            case 2:
                color = new Vector3(95f, 95f, 255f);  // indigo
                break;
            case 3:
                color = new Vector3(95f, 242f, 255f);  // blue
                break;
            case 4:
                color = new Vector3(115f, 255f, 95f);  // green
                break;
            case 5:
                color = new Vector3(255f, 251f, 95f);  // yellow
                break;
            case 6:
                color = new Vector3(255f, 178f, 95f);  // orange
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

    public static Vector3 GetCubeColorVectorOf(int aCubeNumber)
    {
        Vector3 color = new Vector3();

        switch (aCubeNumber)
        {
            case 1:
                color = new Vector3(180, 95f, 211f);  // violet 
                break;
            case 2:
                color = new Vector3(95f, 95f, 255f);  // indigo
                break;
            case 3:
                color = new Vector3(95f, 242f, 255f);  // blue
                break;
            case 4:
                color = new Vector3(115f, 255f, 95f);  // green
                break;
            case 5:
                color = new Vector3(255f, 251f, 95f);  // yellow
                break;
            case 6:
                color = new Vector3(255f, 178f, 95f);  // orange
                break;
            case 7:
                color = new Vector3(255f, 0f, 0f);  // red
                break;
            default:
                color = new Vector3(255f, 255f, 255f);  // white
                break;
        }
        return new Vector3(color.x / 255f, color.y / 255f, color.z / 255f);
    }

    public static int RNG(int aMinValue, int aMaxValue)
    {
        
        return Random.Range(aMinValue, aMaxValue);
    }
}
