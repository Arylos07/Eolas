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
using System;
using System.Linq;

[Serializable]
public class Project
{
    public string projectName;
    public string projectPath;
    public string editDate;
    public string editorName;   //This should be changed out for Github name or other known name later on. 
    public string eolasVersion;

    public List<Item> items = new List<Item>();

    public List<Stat> defaultStats = new List<Stat>();

    public Project()
    {
        projectName = string.Empty;
        projectPath = string.Empty;
        editDate = string.Empty;
        editorName = string.Empty;
    }

    public Project(string name, string path, string date, string editor, string programversion)
    {
        projectName = name;
        projectPath = path;
        editDate = date;
        editorName = editor;
        eolasVersion = programversion;
    }
}

[Serializable]
public class Item
{
    public string itemName;
    public string itemID = string.Empty;
    public List<string> categories = new List<string>();
    public byte[] imageData = null;
    public string description;
    public List<Stat> stats = new List<Stat>();
    public Recipe recipe;

}

[Serializable]
public class Stat
{
    public string name;
    public string value;

    public Stat(string statName, string statValue)
    {
        name = statName;
        value = statValue;
    }

    public Stat()
    {
        name = string.Empty;
        value = string.Empty;
    }

}

[Serializable]
public class Recipe
{
    public string skill;
    public string level;
    public string experience;
    public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
    public string notes;
}

[Serializable]
public class RecipeIngredient
{
    public string item;
    public string amount;
    public string notes;
}
