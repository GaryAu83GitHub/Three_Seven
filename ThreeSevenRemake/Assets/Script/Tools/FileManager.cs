using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Script.Tools
{
    public class FileManager
    {
        public static FileManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new FileManager();
                }
                return mInstance;
            }
        }
        private static FileManager mInstance;


    }
}
