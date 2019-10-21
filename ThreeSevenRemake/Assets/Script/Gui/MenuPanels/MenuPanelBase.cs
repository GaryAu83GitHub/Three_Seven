using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelBase : MonoBehaviour
{
    protected enum ButtonSpriteIndex
    {
        DEFAULT,
        SELECTED,
        PICKED,
    }

    public Image BackgroundImage;
    public GameObject Container;
    public List<Button> Buttons;

    public List<Sprite> ButtonStatesSprites;

    protected int mCurrentSelectButtonIndex = 0;
    protected int mPreviousSelectedButtonIndex = -1;

    private Animation mAnimation;
    private Button mCurrentSelectedButton;

    private void Awake()
    {
        ControlManager.Ins.DefaultSetting();
    }

    public virtual void Start()
    {
        mAnimation = GetComponent<Animation>();
        mCurrentSelectedButton = Buttons[mCurrentSelectButtonIndex];
        //ChangeSelectedButtonSprite(ButtonSpriteIndex.SELECTED);
        SwithCurrentSelectButton();
    }

    public virtual void Update()
    {
        CheckInput();
    }

    public virtual void Enter()
    {
        Container.SetActive(true);
    }

    public virtual void Exit()
    {
        Container.SetActive(false);
    }

    protected virtual void CheckInput()
    {
        //if (ControlManager.Ins.KeyPress(CommandIndex.NAVI_DOWN))
        //    mCurrentSelectButtonIndex++;
        //if (ControlManager.Ins.KeyPress(CommandIndex.NAVI_UP))
        //    mCurrentSelectButtonIndex--;

        NavigateMenuButtons();
        
    }

    protected virtual void NavigateMenuButtons(CommandIndex theIncreaseCommand = CommandIndex.NAVI_DOWN, CommandIndex theDecreaseCommand = CommandIndex.NAVI_UP)
    {
        bool increaseButtonPressed = ControlManager.Ins.MenuNavigation(theIncreaseCommand);
        bool decreaseButtonPressed = ControlManager.Ins.MenuNavigation(theDecreaseCommand);
        

        if (increaseButtonPressed || decreaseButtonPressed)
        {
            int increament = 0;
            if (increaseButtonPressed)
                increament++;
            if (decreaseButtonPressed)
                increament--;

            mPreviousSelectedButtonIndex = mCurrentSelectButtonIndex;
            mCurrentSelectButtonIndex += increament;

            if (mCurrentSelectButtonIndex < 0)
                mCurrentSelectButtonIndex = Buttons.Count - 1;
            else if (mCurrentSelectButtonIndex >= Buttons.Count)
                mCurrentSelectButtonIndex = 0;

            SwithCurrentSelectButton();
        }
    }

    private void SwithCurrentSelectButton()
    {

        //ChangeSelectedButtonSprite(ButtonSpriteIndex.DEFAULT);
        //mCurrentSelectedButton = Buttons[mCurrentSelectButtonIndex];
        //ChangeSelectedButtonSprite(ButtonSpriteIndex.SELECTED);
        ChangeSelectedButtonSprite(mPreviousSelectedButtonIndex, ButtonSpriteIndex.DEFAULT);
        ChangeSelectedButtonSprite(mCurrentSelectButtonIndex, ButtonSpriteIndex.SELECTED);
        mCurrentSelectedButton = Buttons[mCurrentSelectButtonIndex];
    }

    private void ChangeSelectedButtonSprite(int aButtonIndex, ButtonSpriteIndex aStateSprite)
    {
        if (aButtonIndex < 0 || aButtonIndex >= Buttons.Count)
        {
            //Buttons[aButtonIndex].image.sprite = ButtonStatesSprites[(int)ButtonSpriteIndex.DEFAULT];
            return;
        }
        Buttons[aButtonIndex].image.sprite = ButtonStatesSprites[(int)aStateSprite];
    }
}
