using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLine : MonoBehaviour
{
    public MainMenu mainMenu;
    public GameObject initPanel;
    public static CommandLine instance;
    private static bool loadFromFile = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (loadFromFile == true)
        {
            bool contains = false;
            string[] args = System.Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (arg.Contains(".eolas"))
                {
                    contains = true;
                    MainMenu.instance.LoadProject(arg);
                    break;
                }
            }

            if (contains == false)
            {
                initPanel.SetActive(false);
            }

            loadFromFile = false;
        }
        else if(loadFromFile == false)
        {
            initPanel.SetActive(false);
        }

    }
}
