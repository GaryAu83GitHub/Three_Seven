using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Script.Tools
{
    public static class FileManager
    {
        public static void SaveHighScoreToFile(PlayThroughcs aPlay)
        {
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/highscore.sav", FileMode.Append);

            HighScoreData data = new HighScoreData(aPlay);

            bf.Serialize(stream, data);
            stream.Close();
        }
        
    }
}
