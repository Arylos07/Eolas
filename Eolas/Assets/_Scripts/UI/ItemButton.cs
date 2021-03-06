﻿#region copyright
// Author: Michael Cox 2019 for Eolas. 
// All code made available open-source at https://github.com/Arylos07/Eolas
// Eolas is protected by Creative Commons CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)
// You may use Eolas for commercial projects, however recompiling and distribution of Eolas for commercial use
// is not allowed without the exclusive permission from the author.
//------------------------------
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item item;
    [Header("UI")]
    public Text itemName;
    public Image itemImage;
    public GameObject searchConditionsPanel;
    public Text conditionText;

    public void Start()
    {
        gameObject.name = item.itemName;
        itemName.text = item.itemName;
        Texture2D texture = new Texture2D(1, 1);
        ImageConversion.LoadImage(texture, item.imageData);

        Sprite sprite = null;
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        itemImage.sprite = sprite;
    }

    IEnumerator _Start()
    {
        yield return gameObject.name = item.itemName;
        yield return itemName.text = item.itemName;
        Texture2D texture = new Texture2D(1, 1);
        yield return ImageConversion.LoadImage(texture, item.imageData);

        Sprite sprite = null;
        yield return sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        yield return itemImage.sprite = sprite;
        yield return null;
    }

    public void DisplayItemInfo()
    {
        ItemDisplay.DisplayItem(item);
    }
}
