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

    private Stack<TurnData> mRecordings = new Stack<TurnData>();

    public int RecordCount { get { return mRecordings.Count; } }

    public void Record(TurnData aRecordData)
    {
        if(mRecordings.Any())
            mRecordings.Pop();

        mRecordings.Push(aRecordData);
    }

    public TurnData Rewind()
    {   
        return mRecordings.Pop();
    }
}
