#region copyright
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
using System;
using System.IO;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Item item = new Item();
    public static ItemDisplay instance;
    private int itemIndex = 0;

    [Header("Managers")]
    public LoadingManager loadingManager;
    public DataManager dataManager;

    [Header("UI")]
    public GameObject itemDisplayPanel;
    public InputField itemName;
    public InputField itemID;
    public Image itemImage;
    public InputField catagories;
    private string catagoryConstructor = string.Empty;
    public Text description;
    public Transform scrollView;
    public GameObject deletePanel;

    [Header("Templates")]
    public GameObject headerTemplate;
    public GameObject fieldTemplate;

    private void Start()
    {
        instance = this;
    }

    public void DeleteItem()
    {
        LoadingManager.openProject.items.Remove(item);
        LoadingManager.UpdateItems();
        deletePanel.SetActive(false);
        CloseDisplay();
        LoadingManager.madeChanges = true;
    }

    public static void DisplayItem(Item displayItem)
    {
        instance._DisplayItem(displayItem);
    }

    public void EditItem()
    {
        ItemCreation.instance.EditItem(item, itemIndex);
        itemDisplayPanel.SetActive(false);
    }

    public void ClearStats()
    {
        foreach (GameObject stat in GameObject.FindGameObjectsWithTag("StatType"))
        {
            Destroy(stat);
        }
    }

    public void RefreshStats()
    {
        ClearStats();

        int index = 0;

        foreach (Stat stat in item.stats)
        {
            if (stat.name.ToLower() == "header")
            {
                StatType field = Instantiate(headerTemplate, scrollView).GetComponent<StatType>();

                field.name = stat.name;
                field.statValueText.text = stat.value;
                field.statIndex = index;
            }
            else
            {
                StatType field = Instantiate(fieldTemplate, scrollView).GetComponent<StatType>();

                field.name = stat.name;
                field.statNameText.text = stat.name;
                field.statValueText.text = stat.value;
                field.statIndex = index;

            }
            index++;
        }
    }


    public void _DisplayItem(Item displayItem)
    {
        dataManager.ItemIndex(displayItem);
        item = displayItem;

        itemName.text = item.itemName;
        if (item.itemID != "-1")
        {
            itemID.gameObject.SetActive(true);
            itemID.text = item.itemID;
        }
        else if (item.itemID == "-1")
        {
            itemID.gameObject.SetActive(false);
        }

        byte[] imageData = item.imageData;

        Texture2D texture = new Texture2D(0, 0);
        ImageConversion.LoadImage(texture, imageData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 300.0f);
        itemImage.sprite = sprite;

        catagoryConstructor = string.Empty;
        if (item.categories.Count != 0)
        {
            foreach (string catagory in item.categories)
            {
                catagoryConstructor += catagory + ", ";
            }
            catagories.gameObject.SetActive(true);
        }
        else if(item.categories.Count == 0)
        {
            catagories.gameObject.SetActive(false);
        }

        catagories.text = catagoryConstructor;
        description.text = item.description;
        itemDisplayPanel.SetActive(true);
        RefreshStats();
    }

    public void CloseDisplay()
    {
        item = null;
        itemName.text = string.Empty;
        itemID.text = string.Empty;
        itemImage.sprite = null;
        catagoryConstructor = string.Empty;
        catagories.text = string.Empty;
        description.text = string.Empty;
        itemDisplayPanel.SetActive(false);
    }
}
