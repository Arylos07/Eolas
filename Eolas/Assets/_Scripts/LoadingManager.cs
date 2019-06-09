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
    public static FileStream projectFile;

    public static bool madeChanges = false;
    public static LoadingManager instance;

    //used for reverting project to original version
    public Project currentSave;

    [Header("Managers")]
    public DataManager dataManager;
    public ItemCreation itemCreation;
    public SearchManager searchManager;

    [Header("Loading UI")]
    public GameObject loadingPanel;
    public Text loadingText;

    [Space(10)]
    public GameObject progressBarParent;
    public Image loadingBar;

    public GameObject confirmClose;
    public GameObject confirmQuit;

    [Header("Prefabs")]
    public GameObject itemPrefab;

    [Space(10)]
    public Transform itemList;

    // An int used as an enum to determine how the user wants unsaved changes handled.
    // Could have used an enum here, but was too lazy at the time.
    private int close = -1;     // -1 disabled, 0 don't close, 1 don't save and close, 2 save and close

    // Start is called before the first frame update
    void Start()
    {
        madeChanges = false;
        instance = this;
        StartCoroutine(LoadProject());
    }

    private void Update()
    {
        if(madeChanges == true)
        {
            dataManager.projectTitle.text = openProject.projectName + "*";
        }
        else if(madeChanges == false)
        {
            dataManager.projectTitle.text = openProject.projectName;
        }
    }

    public void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        if (close == -1)
        {
            StartCoroutine(_CloseEolas());
        }
        else
        {
            projectFile.Close();
            Application.Quit();
        }
#endif
    }

    IEnumerator _CloseEolas()
    {
        if (madeChanges == true)
        {
            close = -1;
            confirmClose.SetActive(true);

            while (close == -1)
            {
                yield return null;
            }

            if (close == 2)
            {
                RequestSave();
                projectFile.Close();
                Application.Quit();
            }
            else if (close == 1)
            {
                //discard and close
                projectFile.Close();
                Application.Quit();
            }
            else if (close == 0)
            {
                //cancel
                close = -1;
                confirmClose.SetActive(false);
            }
        }
        else
        {
            projectFile.Close();
            Application.Quit();
        }
    }

    public void CloseProject()
    {
        StartCoroutine(_CloseProject());
    }

    IEnumerator _CloseProject()
    {
        if (madeChanges == true)
        {
            close = -1;
            confirmClose.SetActive(true);

            while (close == -1)
            {
                yield return null;
            }

            if (close == 2)
            {
                //save and close
                RequestSave();
                projectFile.Close();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
            else if (close == 1)
            {
                //discard and close
                projectFile.Close();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
            else if (close == 0)
            {
                //cancel
                close = -1;
                confirmClose.SetActive(false);
            }
        }

        else
        {
            RequestSave();
            projectFile.Close();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        close = -1;
    }

    public void ConfirmClose(int confirm)
    {
        close = confirm;
    }

    public static void UpdateItems()
    {
        instance._UpdateItems();
    }

    public void _UpdateItems()
    {
        foreach(GameObject oldButton in GameObject.FindGameObjectsWithTag("ItemButton"))
        {
            Destroy(oldButton);
        }

        madeChanges = true;

        foreach (Item item in openProject.items)
        {
            ItemButton itemButton = Instantiate(itemPrefab, itemList).GetComponent<ItemButton>();

            itemButton.item = item;
        }
    }

    IEnumerator LoadProject()
    {
        //delays are to allow any other behaviours time to reference anything from this function as well as display to user the progress of loading.

        loadingText.text = "Loading Project " + openProject.projectName + "...";
        projectFile = new FileStream(openProject.projectPath, FileMode.Open, FileAccess.ReadWrite);
        currentSave = openProject;
        dataManager.projectTitle.text = openProject.projectName;
        yield return new WaitForSeconds(1);

        loadingText.text = "Loading Project Defaults...";
        itemCreation.newStats = openProject.defaultStats;
        dataManager.editor = editorName;
        yield return new WaitForSeconds(2);

        if (openProject.items.Count != 0)
        {
            loadingText.text = "Fetching items...";
            yield return new WaitForSeconds(0.5f);

            progressBarParent.SetActive(true);

            int i = 0;
            foreach (Item item in openProject.items)
            {
                i++;
                loadingText.text = "Loading item " + i + "/" + openProject.items.Count + "...";
                ItemButton itemButton = Instantiate(itemPrefab, itemList).GetComponent<ItemButton>();

                yield return itemButton.item = item;

                foreach (Stat stat in item.stats)
                {
                    if (stat.name != "header" && !searchManager.searchOptions.Contains(stat.name))
                    {
                        searchManager.searchOptions.Add(stat.name);
                    }
                }


                loadingBar.fillAmount = ((float)i / (float)openProject.items.Count);

                yield return null;
            }
        }

        loadingText.text = "Finishing up...";
        itemCreation.imagePathPlaceholder.text = openProject.projectPath;
        itemCreation.gameObject.SetActive(false);
        searchManager.UpdateDropdown();
        yield return new WaitForSeconds(1);
        progressBarParent.SetActive(false);
        loadingPanel.SetActive(false);
    }

    public static void RequestSave()
    {
        ProjectManager.SaveProject(openProject, projectFile);
    }
}
