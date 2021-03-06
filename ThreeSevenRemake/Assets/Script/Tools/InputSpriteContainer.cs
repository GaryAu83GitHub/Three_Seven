﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class work like a factory class that will be attached to a prefab object and will be load in
/// all the available input sprites
/// It'll be subscribing a "change input sprite" delegate method from the SetKeybindSlot object and return
/// the requesting input sprite.
/// </summary>
public class InputSpriteContainer : MonoBehaviour
{
    public List<Sprite> KeyboardKeys;
    public List<Sprite> XboxButtons;

    private void Awake()
    {
        InputSpritesManager.Instance.LoadKeyboardSprites(KeyboardKeys);
        InputSpritesManager.Instance.LoadXBoxSprites(XboxButtons);
    }
}
