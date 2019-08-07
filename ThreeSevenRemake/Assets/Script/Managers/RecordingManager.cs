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

    public Stack<TurnData> mRecordings = new Stack<TurnData>();

    public void Record(TurnData aRecordData)
    {
        mRecordings.Push(aRecordData);
    }

    public TurnData Rewind()
    {
        mRecordings.Pop();
        return mRecordings.Peek();
    }
}
