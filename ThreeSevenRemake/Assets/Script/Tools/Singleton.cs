using UnityEngine;

namespace Assets.Script.Tools
{ 
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T myInstance;

        private static object myLock = new object();

        //private static bool myApplicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (FindObjectOfType(typeof(T)) == null)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) + 
                        "' already destroyed on application quit." +
                        " Won't create again - return null.");
                    return null;
                }

                lock(myLock)
                {
                    if(myInstance == null)
                    {
                        myInstance = (T)FindObjectOfType(typeof(T));

                        if(FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return myInstance;
                        }

                        if (myInstance == null)
                        {
                            GameObject singleton = new GameObject();
                            myInstance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();

                            DontDestroyOnLoad(singleton);

                            Debug.Log("[Singleton] An instance of " + typeof(T) +
                                " is needed in the scene, so '" + singleton +
                                "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log("[Singleton] Using instance already created: " +
                                myInstance.gameObject.name);
                        }
                    }
                    return myInstance;
                }
            }
        }

        public void OnDestroy()
        {
            //myApplicationIsQuitting = true;
        }
    }
}