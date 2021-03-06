﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabMenu : MonoBehaviour
{
    public Text labText;
    public Image Seconds;
    public Image Minutes;
    public Image SecondLine;
    public Image MinuteLine;

    public GameObject BlockObject;

    public int TopValue = 1;
    public int DownValue = 1;

    public delegate void OnCreateNewBlock(Block aNewBlock);
    public static OnCreateNewBlock createNewBlock;

    public delegate void OnSwapTheBlock(Block aNewBlock);
    public static OnSwapTheBlock swapTheBlock;

    public delegate void OnChangePreviewOrder();
    public static OnChangePreviewOrder changePreviewOrder;

    private Block mCurrentBlock;
    //private float waitTime = 2.0f;
    //private float timer = 0.0f;
    //private float visualTime = 0.0f;
    //private float sectionValue = 1f / 60f;
    //private float lineRotateIntervall = 360f / 60f;
    //private int second = 0;
    //private int minute = 0;
    //private int nextSecond = 0;
    //private int nextMinute = 1;
    //private float secondDegree = 0f;
    //private float minuteDegree = 0f;

    //private RectTransform secondLineTransform;
    //private RectTransform minuteLineTransform;

    //private bool mDropBlock = false;

    private float mLerpValue = 0f;
    private Gradient mGradient = new Gradient();

    private GradientColorKey[] gck;
    private GradientAlphaKey[] gak;

    private int mLevel = 1;
    private int mBonus = 1;

    private void Awake()
    {
        GenerateScoreCombinationPositions.Instance.GenerateCombinationPositions();
        TaskManagerNew.Instance.PrepareNewTaskSubjects();
        TaskManagerNew.Instance.StartFirstSetOfTask();

        
    }
    // Start is called before the first frame update
    void Start()
    {
        //secondLineTransform = SecondLine.GetComponent<RectTransform>();
        //minuteLineTransform = MinuteLine.GetComponent<RectTransform>();
        GUIPanelManager.Instance.StartWithPanel(GUIPanelIndex.MAIN_GAME_PANEL/*TITLE_PANEL*/);
        //CreateNewBlock();

        gck = new GradientColorKey[3];
        gck[0].color = Color.red;
        gck[0].time = 0f;
        gck[1].color = Color.white;
        gck[1].time = .5f;
        gck[2].color = Color.green;
        gck[2].time = 1f;

        gak = new GradientAlphaKey[3];
        gak[0].alpha = 1f;
        gak[0].time = 0f;
        gak[1].alpha = 1f;
        gak[1].time = .5f;
        gak[2].alpha = 1f;
        gak[2].time = 1f;

        mGradient.SetKeys(gck, gak);

        labText.text = "Level: " + mLevel.ToString() + "\n Bonus: " + mBonus.ToString();

        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);

        TaskManagerNew.Instance.StartFirstSetOfTask();
        CreateNewBlock();
    }

    private IEnumerator DropBlock()
    {
        yield return new WaitForSeconds(.5f);
        CreateNewBlock();
        //mDropBlock = false;
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;

        ////labText.text = timer.ToString();

        //if (timer >= 1)
        //{
        //    second++;
        //    Clock();
        //    timer = 0;
        //}
        //if (second >= 60)
        //{
        //    minute++;
        //    Clock();
        //    second = 0;
        //    nextSecond = 1;
        //}
        //if (minute >= 60)
        //{
        //    minute = 0;
        //    nextMinute = 1;
        //}

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            mLerpValue += .01f;
            if (mLerpValue > 1f)
                mLerpValue = 1f;

            mLevel++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            mLerpValue -= .01f;
            if (mLerpValue < 0f)
                mLerpValue = 0f;

            mLevel--;
            if (mLevel < 0)
                mLevel = 0;
        }
        if (mLevel % 10 == 0)
            mBonus = (1 + (mLevel / 10));
            labText.text = "Level: " + mLevel.ToString() + "\n Bonus: " + mBonus.ToString();//mLerpValue.ToString();
        labText.color = mGradient.Evaluate(mLerpValue);//Color.Lerp(Color.white, Color.black,mLerpValue /*Mathf.PingPong(mLerpValue, 1)*/);

        //labText.text = ((int)minute).ToString() + " : " + ((int)second).ToString();
        ////Seconds.fillAmount = (int)timer * sectionValue;
        ////Minutes.fillAmount = minute * sectionValue;
        if (ControlManager.Ins.DropBlockInstantly() && mCurrentBlock != null)
        {
            Destroy(mCurrentBlock.gameObject);
            StartCoroutine(DropBlock());
            //mDropBlock = true;
            //CreateNewBlock();
        }
        if (ControlManager.Ins.SwapPreview())
            swapTheBlock?.Invoke(mCurrentBlock);
        if (ControlManager.Ins.ChangePreview())
            changePreviewOrder?.Invoke();

        //if (mDropBlock)
        //    mCurrentBlock.DropDown();
    }

    private void Clock()
    {
        //if (second >= nextSecond)
        //{
        //    secondLineTransform.Rotate(new Vector3(0, 0, -lineRotateIntervall));
        //    nextSecond++;
        //}
        //if (minute >= nextMinute)
        //{
        //    minuteLineTransform.Rotate(new Vector3(0, 0, -lineRotateIntervall));
        //    nextMinute++;
        //}
    }

    private void CreateNewBlock()
    {
        GameObject newBlock = Instantiate(BlockObject, Vector3.zero, Quaternion.identity);
        
        createNewBlock?.Invoke(newBlock.GetComponent<Block>());

        if (newBlock.GetComponent<Block>() != null)
        {
            mCurrentBlock = newBlock.GetComponent<Block>();
        }
    }
}
