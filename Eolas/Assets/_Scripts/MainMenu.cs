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
using System;
using Crosstales.FB;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public Project selectedProject = null;
    private int open = -1;

    [Header("UI")]
    public Text openOrCreateText;
    public Button openOrCreateButton;
    public Text createProjectText;
    public InputField projectNameField;
    public InputField editorNameField;
    public InputField projectPathField;
    public Transform projectsScrollView;
    public InputField currentEditorName;
    public GameObject versionWarningPanel;
    public Text versionWarningText;

    [Header("Panels")]
    public GameObject projectsPanel;
    public GameObject buttonsPanel;
    public GameObject creationPanel;

    [Header("Project properties")]
    public string projectName;
    public string editorName;
    public string projectPath;

    [Header("Prefabs")]
    public GameObject projectButton;

    [Header("Log")]
    public Transform messageLog;
    public GameObject message;

    public static void Exception(string error)
    {
        GameObject go = Instantiate(instance.message, instance.messageLog);
        Text messageText = go.GetComponent<Text>();
        messageText.color = Color.red;
        messageText.text = error;
    }

    public void OpenGithub()
    {
        Application.OpenURL("https://github.com/Arylos07/Eolas");
    }

    private void Start()
    {
        ProjectManager.projects.Clear();
        projectPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Eolas\\Projects";
        projectPathField.text = projectPath;
        ProjectManager.InitDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Eolas\\Projects");
        instance = this;
    }

    public void OpenProject()
    {
        LoadingManager.openProject = selectedProject;
        LoadingManager.editorName = editorName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Data");
    }

    public void ShowProjectButtons()
    {
        foreach (GameObject button in GameObject.FindGameObjectsWithTag("ProjectButton"))
        {
            Destroy(button);
        }

        foreach(Project project in ProjectManager.projects)
        {
            ProjectButton button = Instantiate(projectButton, projectsScrollView).GetComponent<ProjectButton>();

            button.project = project;
            button.Display();
        }
    }

    public void OpenDirectory(string path)
    {
        path = path.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }

    public void OpenOrCreateProject()
    {
        StartCoroutine(_OpenOrCreateProject());
    }

    public void ContinueOpen(int value)
    {
        open = value;
    }

    IEnumerator _OpenOrCreateProject()
    {
        if (selectedProject == null || selectedProject.projectName == string.Empty)
        {
            editorNameField.text = currentEditorName.text;
            ToggleCreationPanel();
        }
        else
        {
            if (selectedProject.eolasVersion != Application.version)
            {
                versionWarningText.text = "The selected project was made in Eolas version " + selectedProject.eolasVersion + ", which does not match the current version: "
                    + Application.version + ".\n\nThis may lead to issues with your project; it is recommended to back up your project before opening this project.";
                versionWarningPanel.SetActive(true);

                Exception("Version Mismatch - selected project version: " + selectedProject.eolasVersion + ", current editor: " + Application.version);

                while(open == -1)
                {
                    yield return null;
                }

                if(open == 1)
                {
                    versionWarningPanel.SetActive(false);
                    OpenProject();
                }
                else if(open == 0)
                {
                    versionWarningPanel.SetActive(false);
                }
            }
            else if(selectedProject.eolasVersion == Application.version)
            {
                OpenProject();
            }
        }

        open = -1;
    }

    public void ToggleGameObject(GameObject toggleObject)
    {
        toggleObject.SetActive(!toggleObject.activeSelf);
    }

    public void ToggleCreationPanel()
    {
        projectsPanel.SetActive(!projectsPanel.activeSelf);
        creationPanel.SetActive(!creationPanel.activeSelf);

        selectedProject = null;

        ShowProjectButtons();
    }

    public void LoadProject()
    {
        string path = FileBrowser.OpenSingleFile("eolas");

        if (path != string.Empty)
        {
            selectedProject = ProjectManager.LoadProjectFile(path);
            SaveConfig();
            ShowProjectButtons();
        }
    }

    public void LoadProject(string path)
    {
        StartCoroutine(_LoadProject(path));
    }

    public IEnumerator _LoadProject(string path)
    {
        ProjectManager.LoadConfig(false);

        yield return new WaitForSeconds(1);

        currentEditorName.text = ProjectManager.editorName;

        selectedProject = ProjectManager.LoadProjectFile(path);

        SaveConfig();

        if (editorNameField.text == string.Empty)
        {
            editorNameField.text = System.Environment.MachineName;    //get computer's name if loading directly from file and no config is loaded
        }

        OpenOrCreateProject();

        if(selectedProject.eolasVersion != Application.version)
        {
            CommandLine.instance.initPanel.SetActive(false);
        }
    }

    public void CreateNewProject()
    {
        ProjectManager.SaveNewProject(projectName, editorName, projectPath);
        currentEditorName.text = editorName;
    }

    public void SelectDirectory()
    {
        string path = FileBrowser.OpenSingleFolder();

        if (path != string.Empty)
        {
            projectPath = path;
            projectPathField.text = path;
        }
    }

    public void ToggleProjectsWindow()
    {
        if (projectsPanel.activeSelf == false)
        {
            ProjectManager.LoadConfig();
            currentEditorName.text = ProjectManager.editorName;
            ShowProjectButtons();
        }
        else
        {
            ProjectManager.projects.Clear();
            foreach (GameObject button in GameObject.FindGameObjectsWithTag("ProjectButton"))
            {
                Destroy(button);
            }
        }

        projectsPanel.SetActive(!projectsPanel.activeSelf);
        buttonsPanel.SetActive(!buttonsPanel.activeSelf);

        selectedProject = null;
    }

    public void SaveConfig()
    {
        ProjectManager.editorName = currentEditorName.text;
        ProjectManager.SaveConfig();
    }

    private void Update()
    { 
        if (creationPanel.activeSelf == true)
        {
            projectName = projectNameField.text;
            editorName = editorNameField.text;
            projectPathField.text = projectPath;
        }

        if (selectedProject == null || selectedProject.projectName == string.Empty)
        {
            openOrCreateText.text = "Create New Project";
        }
        else if (selectedProject != null || selectedProject.projectName != string.Empty)
        {
            openOrCreateText.text = "Open " + selectedProject.projectName;

            openOrCreateButton.interactable = !(currentEditorName.text == string.Empty);
            ProjectManager.editorName = currentEditorName.text;
        }

        if(ProjectManager.projects.Count == 0)
        {
            createProjectText.text = "No projects found. Load or create a new project";
        }
        else
        {
            createProjectText.text = string.Empty;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}