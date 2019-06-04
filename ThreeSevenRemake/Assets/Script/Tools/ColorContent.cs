using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Tools
{
    public class ColorContent
    {
        public static ColorContent Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new ColorContent();

                return mInstance;
            }
        }
        private static ColorContent mInstance;

        private Dictionary<string, Color> mColorList = new Dictionary<string, Color>();

        public Color GetColorBy(string aHexCode)
        {
            if (mColorList.ContainsKey(aHexCode))
                AddColorByHexCode(aHexCode);
            return mColorList[aHexCode];
        }

        public Color GetColorBy(Color aColor)
        {
            string red = FloatNormalizedToHex(aColor.r);
            string green = FloatNormalizedToHex(aColor.g);
            string blue = FloatNormalizedToHex(aColor.b);

            string hexCode = red + green + blue;
            if (mColorList.ContainsKey(hexCode))
                AddColorByHexCode(hexCode);
            return mColorList[hexCode];
        }

        private void AddColorByHexCode(string aHexCode)
        {
            float red = HexToFloatNormalized(aHexCode.Substring(0, 2));
            float green = HexToFloatNormalized(aHexCode.Substring(2, 2));
            float blue = HexToFloatNormalized(aHexCode.Substring(4, 2));

            mColorList.Add(aHexCode, new Color(red, green, blue));
        }

        private int HexToDec(string aHex)
        {
            int dec = System.Convert.ToInt32(aHex, 16);
            return dec;
        }

        private string DecToHex(int aValue)
        {
            return aValue.ToString("X2");
        }

        private string FloatNormalizedToHex(float aValue)
        {
            return DecToHex(Mathf.RoundToInt(aValue * 255f));
        }

        private float HexToFloatNormalized(string aHex)
        {
            return HexToDec(aHex) / 255f;
        }
    }
}
