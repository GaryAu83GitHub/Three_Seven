using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Tools;

public class SettingMenu : MonoBehaviour
{
    public Button EasyButton;
    public Button NormalButton;
    public Button HardButton;

    public Slider SpeedSlider;

    public Button StartButton;
    public Button LeaveButton;

    private List<bool> mDifficultyBools = new List<bool>();
    
    // Start is called before the first frame update
    void Start()
    {
        EasyButton.onClick.AddListener(SetToEasy);
        NormalButton.onClick.AddListener(SetToNormal);
        HardButton.onClick.AddListener(SetToHard);

        StartButton.onClick.AddListener(StartButtonOnClick);
        LeaveButton.onClick.AddListener(LeaveButtonOnClick);

        mDifficultyBools.Add(false);
        mDifficultyBools.Add(false);
        mDifficultyBools.Add(false);

        SetDifficulties(GameSettings.Instance.Difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToEasy()
    {
        SetDifficulties(Difficulties.EASY);
    }

    public void SetToNormal()
    {
        SetDifficulties(Difficulties.NORMAL);
    }

    public void SetToHard()
    {
        SetDifficulties(Difficulties.HARD);
    }

    public void SetDroprate()
    {
        GameSettings.Instance.SetStartDropSpeed((int)SpeedSlider.value);
    }

    private void StartButtonOnClick()
    {
        ScreenTransistor.Instance.FadeToSceneWithIndex(1);
        //gameObject.SetActive(false);
    }

    private void LeaveButtonOnClick()
    {
        gameObject.SetActive(false);
    }

    private void SetDifficulties(Difficulties aDifficulty)
    {
        ResetDifficulty();
        mDifficultyBools[(int)aDifficulty] = true;

        EasyButton.interactable = !mDifficultyBools[(int)Difficulties.EASY];
        NormalButton.interactable = !mDifficultyBools[(int)Difficulties.NORMAL];
        HardButton.interactable = !mDifficultyBools[(int)Difficulties.HARD];

        GameSettings.Instance.SetDifficulty(aDifficulty);
    }

    private void ResetDifficulty()
    {
        for (int i = 0; i < mDifficultyBools.Count; i++)
            mDifficultyBools[i] = false;
    }
}
