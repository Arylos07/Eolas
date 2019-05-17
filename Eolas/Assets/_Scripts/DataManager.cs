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

public class DataManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject createItemPanel;
    public Text projectTitle;

    [Space(10)]
    public string editor;

    public void ToggleGameObject(GameObject toggleObject)
    {
        toggleObject.SetActive(!toggleObject.activeSelf);
    }

    public void CreateItem(Item newItem)
    {
        LoadingManager.openProject.items.Add(newItem);
    }
}
