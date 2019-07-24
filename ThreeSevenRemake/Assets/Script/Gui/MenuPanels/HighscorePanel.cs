using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscorePanel : MonoBehaviour
{
    public List<HighscoreListComponent> ListComponents;

    public GameObject ScrollButtonContain;

    public Button UpButton;
    public Button DownButton;
    public Button ExitButton;

    private List<RoundResultData> mHighscores = new List<RoundResultData>();
    private int mCurrentScrollValue = 0;
    private int mMaxScrollValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpButton.onClick.AddListener(UpButtonOnClick);
        DownButton.onClick.AddListener(DownButtonOnClick);
        ExitButton.onClick.AddListener(ExitButtonOnClick);

        MainMenu.openHighscorePanel += UpdateList;

        ScrollButtonContain.SetActive(false);
        //List<RoundResultData> highscores = HighScoreManager.Instance.GetListSortBy(TableCategory.SCORE);
        //for(int i = 0; i < ((highscores.Count < 10) ? highscores.Count : 10); i++)
        //{
        //    ListComponents[i].gameObject.SetActive(true);
        //    ListComponents[i].SetData(i + 1, highscores[i]);
        //}

        UpdateList();
    }

    private void OnDestroy()
    {
        MainMenu.openHighscorePanel -= UpdateList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateList()
    {
        
        mHighscores = new List<RoundResultData>(HighScoreManager.Instance.GetListSortBy(TableCategory.SCORE));
        if (mHighscores.Count > 10)
        {
            ScrollButtonContain.SetActive(true);
            mMaxScrollValue = mHighscores.Count - 10;
            Debug.Log(mMaxScrollValue);
        }

        mCurrentScrollValue = 0;
        UpButton.interactable = false;
        DownButton.interactable = true;

        Display();
    }

    private void Display()
    {
        for (int i = 0; i < ((mHighscores.Count < 10) ? mHighscores.Count : 10); i++)
        {
            ListComponents[i].gameObject.SetActive(true);
            ListComponents[i].SetData(mCurrentScrollValue + (i + 1), mHighscores[mCurrentScrollValue + i]);
        }
    }

    private void UpButtonOnClick()
    {
        mCurrentScrollValue--;
        if (mCurrentScrollValue < 0)
        {
            mCurrentScrollValue = 0;
            UpButton.interactable = false;
        }

        DownButton.interactable = true;
        Display();
    }

    private void DownButtonOnClick()
    {
        mCurrentScrollValue++;
        if (mCurrentScrollValue > mMaxScrollValue)
        {
            mCurrentScrollValue = mMaxScrollValue;
            DownButton.interactable = false;
        }
        UpButton.interactable = true;
        Display();
    }

    private void ExitButtonOnClick()
    {
        gameObject.SetActive(false);
    }
}
