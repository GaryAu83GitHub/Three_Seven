using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscorePanel : MonoBehaviour
{
    public List<HighscoreListComponent> ListComponents;
    public Button ExitButton;

    // Start is called before the first frame update
    void Start()
    {
        ExitButton.onClick.AddListener(ExitButtonOnClick);

        MainMenu.openHighscorePanel += DisplayList;

        List<RoundResultData> highscores = HighScoreManager.Instance.GetListSortBy(TableCategory.SCORE);
        for(int i = 0; i < ((highscores.Count < 10) ? highscores.Count : 10); i++)
        {
            ListComponents[i].gameObject.SetActive(true);
            ListComponents[i].SetData(i + 1, highscores[i]);
        }
    }

    private void OnDestroy()
    {
        MainMenu.openHighscorePanel -= DisplayList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayList()
    {
        List<RoundResultData> highscores = HighScoreManager.Instance.GetListSortBy(TableCategory.SCORE);
        for (int i = 0; i < ((highscores.Count < 10) ? highscores.Count : 10); i++)
        {
            ListComponents[i].gameObject.SetActive(true);
            ListComponents[i].SetData(i + 1, highscores[i]);
        }
    }

    private void ExitButtonOnClick()
    {
        gameObject.SetActive(false);
    }
}
