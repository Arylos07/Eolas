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
using UnityEngine.EventSystems;

public class ProjectButton : MonoBehaviour
{
    public Project project;

    [Header("UI")]
    public Text projectName;
    public Text projectEditor;
    public Text projectEditDate;
    public Image buttonImage;
    public Color selectedColour;

    private void Update()
    {
        if(MainMenu.instance.selectedProject != null)
        {
            if(MainMenu.instance.selectedProject == project)
            {
                buttonImage.color = selectedColour;
            }
            else
            {
                buttonImage.color = Color.white;
            }
        }
    }

    public void Display()
    {
        gameObject.name = project.projectName;
        projectName.text = project.projectName;
        projectEditor.text = project.editorName;
        projectEditDate.text = project.editDate;
    }

    public void SelectProject()
    {
        gameObject.GetComponent<Button>().Select();
        MainMenu.instance.selectedProject = project;
    }

    public void OpenProjectDirectory()
    {
        MainMenu.instance.OpenDirectory(project.projectPath);
    }
}
