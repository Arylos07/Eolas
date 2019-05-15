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

public class StatType : MonoBehaviour
{
    public InputField statName;
    public InputField statValue;
    public Item item;
    public int statIndex;

    [Header("Buttons")]
    public Button upButton;
    public Button downButton;

    // Start is called before the first frame update
    void Start()
    {
        if(statIndex == 0)
        {
            upButton.interactable = false;
        }
        else if(statIndex == ItemCreation.instance.newStats.Count - 1)
        {
            downButton.interactable = false;
        }
    }

    public void Move(string value)
    {
        if(value == "up")
        {
            ItemCreation.instance.newStats.Move(statIndex, statIndex - 1);
        }
        else if(value == "down")
        {
            ItemCreation.instance.newStats.Move(statIndex, statIndex + 1);
        }
        else if(value == "-")
        {
            ItemCreation.instance.newStats.RemoveAt(statIndex);
        }

        ItemCreation.instance.RefreshStats(false);
    }
}
