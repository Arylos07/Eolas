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

public class ProjectSettings : MonoBehaviour
{
    [Header("Project Info")]
    public Text projectName;
    public Text projectEditDate;
    public Text projectEolasVersion;
    public Text projectItems;

   
    void Update()
    {
        projectName.text = "Project: " + LoadingManager.openProject.projectName;
        projectEditDate.text = "Last edit: " + LoadingManager.openProject.editDate + " UTC";
        projectEolasVersion.text = "Eolas Version: " + LoadingManager.openProject.eolasVersion;
        projectItems.text = "Items: " + LoadingManager.openProject.items.Count;
    }

    public void DeleteProject()
    {
        ProjectManager.DeleteProject(LoadingManager.openProject, LoadingManager.projectFile);
    }
}
