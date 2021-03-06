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
using System.IO;
using Crosstales.FB;
using System;
using System.Linq;

public class ItemCreation : MonoBehaviour
{
    private Item newItem = new Item();
    public string[] allowedImageFiles;
    private int itemIndex = -1;      //index of item for editing, -1 disables this, meaning a new item is being made

    public List<Stat> newStats = new List<Stat>();
    public static ItemCreation instance;
    private bool changedImage = false;
    private bool editingItem = false;
    private byte[] imageStage;

    [Header("Managers")]
    public LoadingManager loadingManager;
    public DataManager dataManager;
    public SearchManager searchManager;

    [Header("UI")]
    public InputField itemName;
    public InputField itemID;
    public Image itemImage;
    public InputField imagePath;
    public Text imagePathPlaceholder;
    public InputField catagories;
    public InputField description;
    public Transform scrollView;
    public GameObject addPanel;
    public Button createButton;
    public Texture2D defaultIcon;

    [Header("Templates")]
    public GameObject headerTemplate;
    public GameObject fieldTemplate;

    public void Start()
    {
        instance = this;
        RefreshStats();
    }

    private void Update()
    {
        createButton.interactable = (itemName.text != string.Empty);
    }

    public void ToggleItemCreation()
    {
        editingItem = false;
        gameObject.SetActive(!gameObject.activeSelf);
    }

    //toggles the add stat panel
    public void ToggleAddStat()
    {
        addPanel.SetActive(!addPanel.activeSelf);
    }

    public void CreateItem()
    {
        if (itemIndex == -1)
        {
            newItem.itemName = itemName.text;
            newItem.itemID = itemID.text;

            if (catagories.text != string.Empty)
            {
                string[] cat = catagories.text.Split(',');

                for (int i = 0; i < cat.Length; ++i)
                {
                    cat[i] = cat[i].Trim();
                }
                newItem.categories = cat.ToList();
            }
            else if (catagories.text == string.Empty)
            {
                newItem.categories.Clear();
            }

            newItem.stats.Clear();

            foreach (GameObject statObject in GameObject.FindGameObjectsWithTag("StatType"))
            {
                StatType stat = statObject.GetComponent<StatType>();
                if (stat.statName == null)
                {
                    newItem.stats.Add(new Stat("header", stat.statValue.text));
                }
                else
                {
                    newItem.stats.Add(new Stat(stat.statName.text, stat.statValue.text));
                    if (!searchManager.searchOptions.Contains(stat.statName.text))
                    {
                        searchManager.searchOptions.Add(stat.statName.text);
                        searchManager.UpdateDropdown();
                    }
                }
            }

            if(changedImage == false)
            {
                byte[] defaultIconData = ImageConversion.EncodeToPNG(defaultIcon);
                newItem.imageData = defaultIconData;
            }

            newItem.description = description.text;
            dataManager.CreateItem(newItem);
            LoadingManager.UpdateItems();
            RefreshFields();
            changedImage = false;
            gameObject.SetActive(false);
            
        }
        else if(itemIndex != -1)
        {
            SubmitEdit();
        }
    }

    public void EditItem(Item item, int index)
    {
        editingItem = true;
        newItem = item;
        itemIndex = index;

        itemName.text = item.itemName;
        itemID.text = item.itemID;

        byte[] imageData = item.imageData;

        Texture2D texture = new Texture2D(0, 0);
        ImageConversion.LoadImage(texture, imageData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 300.0f);
        itemImage.sprite = sprite;

        string catagoryConstructor = string.Empty;
        if (item.categories.Count != 0)
        {
            foreach (string catagory in item.categories)
            {
                catagoryConstructor += catagory + ", ";
            }
        }

        catagories.text = catagoryConstructor;

        description.text = item.description;

        newStats = item.stats;

        gameObject.SetActive(true);
        RefreshStats(false);
    }

    public void SubmitEdit()
    {
        LoadingManager.openProject.items.Remove(newItem);
        newItem.itemName = itemName.text;
        newItem.itemID = itemID.text;

        if (catagories.text != string.Empty)
        {
            string[] cat = catagories.text.Split(',');

            for (int i = 0; i < cat.Length; ++i)
            {
                cat[i] = cat[i].Trim();
            }
            newItem.categories = cat.ToList();
        }
        else if (catagories.text == string.Empty)
        {
            newItem.categories.Clear();
        }

        newItem.stats.Clear();

        foreach (GameObject statObject in GameObject.FindGameObjectsWithTag("StatType"))
        {
            StatType stat = statObject.GetComponent<StatType>();
            if (stat.statName == null)
            {
                newItem.stats.Add(new Stat("header", stat.statValue.text));
            }
            else
            {
                newItem.stats.Add(new Stat(stat.statName.text, stat.statValue.text));
                if (!searchManager.searchOptions.Contains(stat.statName.text))
                {
                    searchManager.searchOptions.Add(stat.statName.text);
                    searchManager.UpdateDropdown();
                }
            }
        }


       if(editingItem == true && changedImage == true)
        {
            newItem.imageData = imageStage;
            imageStage = null;
        }


        newItem.description = description.text;
        LoadingManager.openProject.items.Insert(itemIndex, newItem);
        LoadingManager.UpdateItems();
        itemIndex = -1;
        RefreshFields();
        gameObject.SetActive(false);
    }

    public void RefreshFields()
    {
        itemName.text = string.Empty;
        itemID.text = string.Empty;
        catagories.text = string.Empty;
        description.text = string.Empty;
        //imagePath.text = string.Empty;
        newItem = new Item();
        //RevertStatsToDefault();
    }

    public void AddField(bool header)
    {
        if(header == true)
        {
            Stat headerStat = new Stat("header", "Header");

            newStats.Add(headerStat);

            RefreshStats();
        }
        else if(header == false)
        {
            Stat valueStat = new Stat("Stat Name", "Value");

            newStats.Add(valueStat);

            RefreshStats();
        }
    }

    public void RevertStatsToDefault()
    {
        newStats.Clear();

        foreach(Stat defaultStat in LoadingManager.openProject.defaultStats)
        {
            newStats.Add(defaultStat);
        }
    }

    public void SaveAsDefaultStats()
    {
        LoadingManager.openProject.defaultStats.Clear();

        foreach(Stat newDefault in newStats)
        {
            LoadingManager.openProject.defaultStats.Add(newDefault);
        }
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
        if (addPanel.activeSelf == true)
            ToggleAddStat();

        foreach (GameObject stat in GameObject.FindGameObjectsWithTag("StatType"))
        {
            StatType statType = stat.GetComponent<StatType>();

            Stat statStage = new Stat();

            if (statType.statName != null)
            {
                statStage = new Stat(statType.statName.text, statType.statValue.text);
            }
            else
            {
                statStage = new Stat("header", statType.statValue.text);
            }

            newStats.Replace(statType.statIndex, statStage);

            Destroy(stat);
        }

        int index = 0;

        foreach (Stat stat in newStats)
        {
            if(stat.name.ToLower() == "header")
            {
                StatType field = Instantiate(headerTemplate, scrollView).GetComponent<StatType>();

                field.name = stat.name;
                field.statValue.text = stat.value;
                field.statIndex = index;
            }
            else
            {
                StatType field = Instantiate(fieldTemplate, scrollView).GetComponent<StatType>();

                field.name = stat.name;
                field.statName.text = stat.name;
                field.statValue.text = stat.value;
                field.statIndex = index;

            }
            index++;
        }
    }

    public void RefreshStats(bool saveStats)
    {
        if (addPanel.activeSelf == true)
            ToggleAddStat();

        ClearStats();

        int index = 0;

        foreach (Stat stat in newStats)
        {
            if (stat.name.ToLower() == "header")
            {
                StatType field = Instantiate(headerTemplate, scrollView).GetComponent<StatType>();

                field.name = stat.name;
                field.statValue.text = stat.value;
                field.statIndex = index;
            }
            else
            {
                StatType field = Instantiate(fieldTemplate, scrollView).GetComponent<StatType>();

                field.name = stat.name;
                field.statName.text = stat.name;
                field.statValue.text = stat.value;
                field.statIndex = index;

            }
            index++;
        }
    }


    public void SelectImage()
    {
        string directory = imagePath.text;

        if(directory == string.Empty)
        {
            directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        string path = FileBrowser.OpenSingleFile("Select an image", directory, allowedImageFiles);

        if(path != string.Empty)
        {
            imagePath.text = path;
            UploadImage();
        }
    }

    public void UploadImage()
    {
        byte[] imageData = File.ReadAllBytes(imagePath.text);

        if (editingItem == false)
        {
            byte[] compressedData = imageData;

            newItem.imageData = compressedData;

            Texture2D texture = new Texture2D(0, 0);
            ImageConversion.LoadImage(texture, newItem.imageData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 300.0f);
            itemImage.sprite = sprite;
            changedImage = true;
        }
        else if(editingItem == true)
        {
            imageStage = imageData;
            Texture2D texture = new Texture2D(0, 0);
            ImageConversion.LoadImage(texture, imageStage);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 300.0f);
            itemImage.sprite = sprite;
            changedImage = true;
        }
    }
}
