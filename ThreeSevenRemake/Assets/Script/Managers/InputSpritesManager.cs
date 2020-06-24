using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is responsible to managing the input sprites of the game.
/// It'll first rescieve the input sprites from the Awake function
/// in the InputSpriteContainer class when the game starts.
/// 
/// With GetSprite functions for each type of control this class can be called
/// through the game without worrying if the InputSpriteContainer class get
/// destroyed during the change scene
/// </summary>
public class InputSpritesManager
{
    public static InputSpritesManager Instance
    {
        get
        {
            if (mInstance == null) { mInstance = new InputSpritesManager(); }
            return mInstance;
        }
    }
    private static InputSpritesManager mInstance;


    private Dictionary<KeyCode, Sprite> mKeyboardKeys = new Dictionary<KeyCode, Sprite>();
    private Dictionary<XBoxButton, Sprite> mXBoxButtons = new Dictionary<XBoxButton, Sprite>();

    public void LoadKeyboardSprites(List<Sprite> someKeyboardSpirtes)
    {
        List<KeyCode> values = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToList();
        foreach (Sprite sprite in someKeyboardSpirtes)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if(sprite.name.Length == values[i].ToString().Length &&
                    sprite.name.Contains(values[i].ToString()) 
                    && !mKeyboardKeys.ContainsKey(values[i]))
                {
                    mKeyboardKeys.Add(values[i], sprite);
                    break;
                }
            }
        }
    }

    public Sprite GetKeyboard(KeyCode aKeyCode)
    {
        if (!mKeyboardKeys.ContainsKey(aKeyCode))
            return null;
        return mKeyboardKeys[aKeyCode];
    }

    public void LoadXBoxSprites(List<Sprite> someXBoxSpirtes)
    {
        List<XBoxButton> values = Enum.GetValues(typeof(XBoxButton)).Cast<XBoxButton>().ToList();
        foreach(Sprite sprite in someXBoxSpirtes)
        {
            for(int i = 0; i < values.Count; i++)
            {
                if (sprite.name.Length == values[i].ToString().Length &&
                    sprite.name.Contains(values[i].ToString())
                    && !mXBoxButtons.ContainsKey(values[i]))
                {
                    mXBoxButtons.Add(values[i], sprite);
                    break;
                }
            }
        }
    }

    public Sprite GetXboxButton(XBoxButton aButton)
    {
        if (!mXBoxButtons.ContainsKey(aButton))
            return null;
        return mXBoxButtons[aButton];
    }
}
