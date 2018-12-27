using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Script.Tools
{
    public class ScreenTransistor : Singleton<ScreenTransistor>
    {
        public Animator animator;

        private int mySceneIndex = 0;
        private string mySceneName = "";

        private bool myFadeToSceneWithName = false;

        protected ScreenTransistor() { }

        private void Update()
        {
            
        }

        public void FadeToSceneWithIndex(int aSceneIndex)
        {
            myFadeToSceneWithName = false;
            mySceneIndex = aSceneIndex;
            //animator.SetTrigger("FadeOut");
            OnFadeComplete();
        }

        public void FadeToSceneWithName(string aSceneName)
        {
            myFadeToSceneWithName = true;
            mySceneName = aSceneName;
            //animator.SetTrigger("FadeOut");
            OnFadeComplete();
        }

        public void OnFadeComplete()
        {
            if (myFadeToSceneWithName)
                SceneManager.LoadScene(mySceneName);
            else
                SceneManager.LoadScene(mySceneIndex);
        }
    }
}
