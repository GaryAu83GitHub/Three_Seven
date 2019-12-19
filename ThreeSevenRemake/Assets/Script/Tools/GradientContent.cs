using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GradientContent
{
    public static GradientContent Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GradientContent();
            }
            return mInstance;
        }
    }
    private static GradientContent mInstance;

    private Dictionary<string, Gradient> mGradientList = new Dictionary<string, Gradient>();

    public Gradient GetGradientBy(string aGradientName)
    {
        if (!mGradientList.ContainsKey(aGradientName))
        {
            Debug.LogError("The gradient with name " + aGradientName + " is not registrated in GradientContent");
            return null;
        }
        return mGradientList[aGradientName];
    }

    public Color GetColor(string aGradientName, float anAtGradientLenght)
    {
        return mGradientList[aGradientName].Evaluate(anAtGradientLenght);
    }

    public void AddGradient(string aName, Color[] someColors, float[] someAlphas, float[] someTimes)
    {
        if (mGradientList.ContainsKey(aName))
            return;

        GradientData newData = new GradientData(aName, someColors, someAlphas, someTimes);

        mGradientList.Add(aName, newData.Gradient);
    }
}

public class GradientData
{
    private readonly string mName = "";
    public string GradientName { get { return mName; } }

    private Gradient mGradient = new Gradient();
    public Gradient Gradient { get { return mGradient; } }

    private readonly Color[] mColors;
    private readonly GradientAlphaKey[] mAlphaKeys;
    private readonly GradientColorKey[] mColorKeys;

    public GradientData(string aGradientName, Color[] someColors, float[] someAlphas, float[] someTimes)
    {
        mName = aGradientName;

        mGradient = new Gradient();
        mColorKeys = new GradientColorKey[someColors.Length];
        mAlphaKeys = new GradientAlphaKey[someColors.Length];

        for(int i = 0; i < someColors.Length; i++)
        {
            mColorKeys[i].color = someColors[i];
            mColorKeys[i].time = someTimes[i];

            mAlphaKeys[i].alpha = someAlphas[i];
            mAlphaKeys[i].time = someTimes[i];
        }

        mGradient.SetKeys(mColorKeys, mAlphaKeys);
    }

    public bool Equal(GradientData aData)
    {
        //Check for null and compare run-time types.
        if ((aData == null) || !this.GetType().Equals(aData.GetType()))
        {
            return false;
        }
        else
        {
            GradientData g = (GradientData)aData;
            return (mName == g.mName) && (mGradient == g.mGradient);
        }
    }
}
