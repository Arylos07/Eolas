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
using System.Collections.ObjectModel;
using System.IO;
using System;
using System.Linq;

public class LoadingManager : MonoBehaviour
{
    public static Project openProject;
    public static string editorName;
    FileStream projectFile;

    [Header("Managers")]
    public DataManager dataManager;
    public ItemCreation itemCreation;

    [Header("Loading UI")]
    public GameObject loadingPanel;
    public Text loadingText;

    [Header("Prefabs")]
    public GameObject itemPrefab;

    [Space(10)]
    public Transform itemList;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadProject());
        print("start");
    }

    IEnumerator LoadProject()
    {
        //delays are to allow any other behaviours time to reference anything from this function as well as display to user the progress of loading.

        loadingText.text = "Loading Project " + openProject.projectName + "...";
        projectFile = new FileStream(openProject.projectPath, FileMode.Open, FileAccess.Read, FileShare.None);    //lock file to prevent corruption
        dataManager.projectTitle.text = openProject.projectName;
        yield return new WaitForSeconds(1);

        loadingText.text = "Loading Project Defaults...";
        itemCreation.newStats = openProject.defaultStats;
        dataManager.editor = editorName;
        yield return new WaitForSeconds(2);

        loadingText.text = "Fetching items...";
        yield return new WaitForSeconds(0.5f);

        int i = 0;
        foreach (Item item in openProject.items)
        {
            i++;
            loadingText.text = "Loading item " + i + "/" + openProject.items.Count + "...";
            ItemButton itemButton = Instantiate(itemPrefab, itemList).GetComponent<ItemButton>();

            itemButton.item = item;
            yield return null;
        }

        loadingText.text = "Finishing up...";
        itemCreation.imagePath.text = openProject.projectPath;
        yield return new WaitForSeconds(1);
        loadingPanel.SetActive(false);
    }
}
