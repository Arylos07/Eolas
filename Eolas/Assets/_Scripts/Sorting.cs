using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Sorting : MonoBehaviour
{
    public Dropdown sortMode;

    public void SortItems()
    {
        if(sortMode.options[sortMode.value].text.ToLower() == "alphabetical (ascending)")
        {
            LoadingManager.openProject.items.Sort(new Comparison<Item>((x, y) => x.itemName.CompareTo(y.itemName)));
        }
        else if (sortMode.options[sortMode.value].text.ToLower() == "alphabetical (descending)")
        {
            LoadingManager.openProject.items.Sort(new Comparison<Item>((y, x) => x.itemName.CompareTo(y.itemName)));
        }

        LoadingManager.UpdateItems();
        LoadingManager.madeChanges = true;
        sortMode.value = 0;
    }
}
