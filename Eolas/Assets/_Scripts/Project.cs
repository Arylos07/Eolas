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
using System;

[Serializable]
public class Project
{
    public string projectName;
    public string projectPath;
    public string editDate;
    public string editorName;   //This should be changed out for Github name or other known name later on. 

    public List<Item> items = new List<Item>();

    public Project()
    {
        projectName = string.Empty;
        projectPath = string.Empty;
        editDate = string.Empty;
        editorName = string.Empty;
    }

    public Project(string name, string path, string date, string editor)
    {
        projectName = name;
        projectPath = path;
        editDate = date;
        editorName = editor;
    }
}

[Serializable]
public class Item
{
    public string itemName;
    public int itemID = -1;
    public List<string> categories = new List<string>();
    public byte[] imageData;
    public string description;
    public List<object> stats = new List<object>();
    public List<object> otherStats = new List<object>();
    public List<Recipes> recipes = new List<Recipes>();
}

[Serializable]
public class StringProperty
{
    public string name;
    public string property;
}

[Serializable]
public class IntProperty
{
    public string name;
    public int property;
}

[Serializable]
public class FloatProperty
{
    public string name;
    public float property;
}

[Serializable]
public class BoolProperty
{
    public string name;
    public bool property;
}

[Serializable]
public class Recipes
{
    public string skill;
    public List<RecipeIngredients> ingredients = new List<RecipeIngredients>();
    public string notes;
}

[Serializable]
public class RecipeIngredients
{
    public Item item;
    public int amount;
    public string notes;
}
