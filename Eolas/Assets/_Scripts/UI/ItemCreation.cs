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

    public List<object> defaultStats = new List<object>();

    [Header("UI")]
    public InputField itemName;
    public InputField itemID;
    public Image itemImage;
    public InputField imagePath;
    public Text catagories;
    public InputField catagory;
    public InputField description;
    public Transform scrollView;

    [Header("Templates")]
    public GameObject headerTemplate;
    public GameObject boolTemplate;
    public GameObject intTemplate;
    public GameObject floatTemplate;
    public GameObject stringTemplate;

    public void Start()
    {
        HeaderProperty h = new HeaderProperty();
        h.header = "Test";
        defaultStats.Add(h);

        foreach (object defaultField in defaultStats)
        {
            if(defaultField.GetType() == (typeof(HeaderProperty)))
            {
                StatType type = Instantiate(headerTemplate, scrollView).GetComponent<StatType>();
                HeaderProperty header = (HeaderProperty)defaultField;
                type.name = header.header;

                newItem.stats.Add(header);
            }
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
