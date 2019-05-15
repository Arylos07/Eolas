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
using UnityEngine.UI;
using System.IO;
using Crosstales.FB;
using System;

public class ItemCreation : MonoBehaviour
{
    private Item newItem = new Item();
    public string[] allowedImageFiles;

    public List<Stat> newStats = new List<Stat>();
    public static ItemCreation instance;

    [Header("UI")]
    public InputField itemName;
    public InputField itemID;
    public Image itemImage;
    public InputField imagePath;
    public Text catagories;
    public InputField catagory;
    public InputField description;
    public Transform scrollView;
    public GameObject addPanel;

    [Header("Templates")]
    public GameObject headerTemplate;
    public GameObject fieldTemplate;

    public void Start()
    {
        instance = this;
        RefreshStats();
    }

    public void ToggleAdd()
    {
        addPanel.SetActive(!addPanel.activeSelf);
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
            ToggleAdd();

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
            ToggleAdd();

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
        newItem.imageData = imageData;

        Texture2D texture = new Texture2D(0, 0);
        ImageConversion.LoadImage(texture, newItem.imageData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 300.0f);
        itemImage.sprite = sprite;
    }
}
