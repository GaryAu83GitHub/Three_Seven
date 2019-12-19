using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimeTool
{
    public static string TimeString(float aTimeValue)
    {
        string mTimeInText = "";

        int seconds = (int)(aTimeValue % 60);
        int minutes = (int)((aTimeValue / 60) % 60);
        int hours = (int)((aTimeValue / 3600) % 60);

        if (hours > 0)
            mTimeInText = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        else
            mTimeInText = string.Format("{0:00}:{1:00}", minutes, seconds);

        return mTimeInText;
    }
}
