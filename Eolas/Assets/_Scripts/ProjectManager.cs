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
using LitJson;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour
{
    public static List<Project> projects = new List<Project>();
    public static string editorName;
    public static string projectName;
    public static string editDate;
    public static string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Eolas";
    public static List<string> projectPaths = new List<string>();
    public static List<string> projectsNotFound = new List<string>();

    public static void SaveConfig()
    {
        //Load binary coding and create a file to write to.
        BinaryFormatter binary = new BinaryFormatter();
        FileStream fStream = File.Create(documentsPath + "/config.conf");

        //Create a copy of the SaveManager to get variables to save.
        Config config = new Config();

        projectPaths.Clear();
        foreach (Project project in projects)
        {
            config.projectPaths.Add(project.projectPath);
            projectPaths.Add(project.projectPath);
        }

        config.editorName = editorName;

        //Add other variables if needed...

        //Encrypt information
        binary.Serialize(fStream, config);
        //Close file.
        fStream.Close();
    }

    public static void SaveProject(Project project, FileStream projectStream)
    {
        BinaryFormatter binary = new BinaryFormatter();

        project.editDate = DateTime.UtcNow.ToString();
        project.editorName = editorName;

        binary.Serialize(projectStream, project);
        LoadingManager.madeChanges = false;
    }

    public static void SaveNewProject(string projectName, string projectEditor, string projectPath)
    {
        //Load binary coding and create a file to write to.
        BinaryFormatter binary = new BinaryFormatter();
        string formattedPath = projectPath + "\\" + projectName + ".eolas";
        FileStream fStream = File.Create(formattedPath);

        //Create a copy of the SaveManager to get variables to save.
        Project project = new Project(projectName, formattedPath, DateTime.UtcNow.ToString(), projectEditor);

        projects.Add(project);
        projectPaths.Add(project.projectPath);

        editorName = projectEditor;

        //Add other variables if needed...

        //Encrypt information
        binary.Serialize(fStream, project);
        //Close file.
        fStream.Close();

        MainMenu.instance.ToggleCreationPanel();
        SaveConfig();
    }

    public static Project LoadProjectFile(string path)
    {
        if (File.Exists(path))
        {
            //Load binary formatter and open the file. Decrypt the file and close it.
            BinaryFormatter binary = new BinaryFormatter();
            FileStream fStream = File.Open(path, FileMode.Open);
            Project project = (Project)binary.Deserialize(fStream);
            fStream.Close();

            //Take decrypted variables and add them to the GameController so rules can be adjusted accordingly.
            project.projectPath = path;
            projects.Add(project);

            return project;
        }
        else
        {
            MainMenu.Exception("A project could not have been loaded. This file may have been moved or deleted.\nFile location: " + path);
            projectsNotFound.Add(path);
            return null;
        }
    }

    public static void LoadProjectFile(List<string> paths)
    {
        projectsNotFound.Clear();
        foreach (string path in paths)
        {
            LoadProjectFile(path);
        }

        if(projectsNotFound.Count != 0)
        {
            foreach(string path in projectsNotFound)
            {
                projectPaths.Remove(path);
            }

            SaveConfig();
        }
    }

    public static void InitDirectory()
    {
        if (Directory.Exists(documentsPath) == false)
        {
            Directory.CreateDirectory(documentsPath);
        }
    }

    public static void InitDirectory(string directory)
    {
        if(Directory.Exists(directory) == false)
        {
            Directory.CreateDirectory(directory);
        }
    }

    public static void LoadConfig()
    {
        //Only load if there is an existing file.
        if (File.Exists(documentsPath + "/config.conf"))
        {
            //Load binary formatter and open the file. Decrypt the file and close it.
            BinaryFormatter binary = new BinaryFormatter();
            FileStream fStream = File.Open(documentsPath + "/config.conf", FileMode.Open);
            Config config = (Config)binary.Deserialize(fStream);
            fStream.Close();

            //Take decrypted variables and add them to the GameController so rules can be adjusted accordingly.

            projectPaths = config.projectPaths;

            editorName = config.editorName;

            //Add other variables if needed...

            LoadProjectFile(projectPaths);
        }
    }
}

[Serializable]
public class Config
{
    public List<string> projectPaths = new List<string>();
    public string editorName;
}