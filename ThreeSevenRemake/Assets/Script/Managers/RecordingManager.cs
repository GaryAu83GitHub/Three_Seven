using System;
using System.Collections.Generic;
using System.Linq;

public class RecordingManager
{
    public static RecordingManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new RecordingManager();
            }
            return mInstance;
        }
    }
    private static RecordingManager mInstance;

    
}
