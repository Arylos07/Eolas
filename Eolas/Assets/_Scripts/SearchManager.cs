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

public class SearchManager : MonoBehaviour
{
    public List<Item> searchedItems = new List<Item>();
    public List<string> searchConditions = new List<string>();
    public List<string> searchOptions = new List<string>();
    private bool allowEnter;

    [Header("Managers")]
    public DataManager dataManager;
    public ItemCreation itemCreation;

    [Header("Searching UI")]
    public GameObject searchingPanel;
    public Text searchingText;
    public Dropdown searchMode;
    public InputField searchCondition;
    public Button resetButton;
    public Text conditionText;

    [Header("Prefabs")]
    public GameObject itemPrefab;

    [Space(10)]
    public Transform itemList;

    private void Start()
    {
        searchMode.options.Clear();
        searchMode.AddOptions(searchOptions);
    }

    public void UpdateDropdown()
    {
        searchMode.options.Clear();
        searchMode.AddOptions(searchOptions);
    }

    public void Search()
    {
        conditionText.text = string.Empty;
        conditionText.gameObject.SetActive(false);
        foreach (GameObject oldButton in GameObject.FindGameObjectsWithTag("ItemButton"))
        {
            Destroy(oldButton);
        }

        StartCoroutine(SearchItems());
    }

    private void Update()
    {
        if (allowEnter && (searchCondition.text.Length > 0) && (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit")))
        {
            Search();
            allowEnter = false;
        }
        else
            allowEnter = searchCondition.isFocused;
    }

    public void ResetItems()
    {
        StartCoroutine(_ResetItems());
    }

    IEnumerator _ResetItems()
    {
        searchingPanel.SetActive(true);
        conditionText.text = string.Empty;
        conditionText.gameObject.SetActive(false);
        searchingText.text = "Reseting...";

        foreach (GameObject oldButton in GameObject.FindGameObjectsWithTag("ItemButton"))
        {
            Destroy(oldButton);
        }


        foreach (Item item in LoadingManager.openProject.items)
        {
            ItemButton itemButton = Instantiate(itemPrefab, itemList).GetComponent<ItemButton>();

            itemButton.item = item;
        }

        searchCondition.text = string.Empty;

        searchingPanel.SetActive(false);
        yield return null;

        searchConditions.Clear();
        searchedItems.Clear();
    }

    IEnumerator SearchItems()
    {
        searchConditions.Clear();
        searchedItems.Clear();
        searchingText.text = "Starting search...";
        searchingPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        foreach (GameObject oldButton in GameObject.FindGameObjectsWithTag("ItemButton"))
        {
            Destroy(oldButton);
        }

        searchingText.text = "Searching Items...";
        if (searchMode.options[searchMode.value].text == "Item Name")
        {
            foreach (Item item in LoadingManager.openProject.items)
            {
                if (item.itemName.Contains(searchCondition.text.ToLower()))
                {
                    searchedItems.Add(item);
                }
            }
        }
        else if (searchMode.options[searchMode.value].text == "Item ID")
        {
            foreach (Item item in LoadingManager.openProject.items)
            {
                if (item.itemID == searchCondition.text)
                {
                    searchedItems.Add(item);
                    searchConditions.Add("Item ID = " + item.itemID);
                }
            }
        }
        else if (searchMode.options[searchMode.value].text == "Description")
        {
            foreach (Item item in LoadingManager.openProject.items)
            {
                if (item.description.Contains(searchCondition.text.ToLower()))
                {
                    searchedItems.Add(item);
                    searchConditions.Add("Description contains " + '"' + searchCondition.text + '"');
                }
            }
        }
        else if(searchMode.options[searchMode.value].text == "Category")
        {
            if (searchCondition.text.Contains(","))
            {
                string[] categories = searchCondition.text.Split(',');
                foreach (string cat in categories)
                {
                    cat.Trim();
                }

                foreach (Item item in LoadingManager.openProject.items)
                {
                    foreach (string category in categories)
                    {
                        if (item.categories.Contains(category))
                        {
                            searchedItems.Add(item);
                            searchConditions.Add("Catagories contains " + '"' + category + '"');
                            break;
                        }
                    }
                }
            }
            else if(searchCondition.text.Contains(",") != true)
            {
                foreach (Item item in LoadingManager.openProject.items)
                {
                    if (item.categories.Contains(searchCondition.text))
                    {
                        searchedItems.Add(item);
                        searchConditions.Add("Catagories contains " + '"' + searchCondition.text + '"');
                    }
                }
            }
        }
        else
        {
            string[] statCondition = null;
            int[] betweenComarisons = null;
            string comparison = string.Empty;
            int singleComparison = -1;
            if (searchCondition.text.Contains("-"))
            {
                searchCondition.text.Replace(" ", string.Empty);
                statCondition = searchCondition.text.Split('-');
                betweenComarisons = new int[] { int.Parse(statCondition[0]), int.Parse(statCondition[1]) };
                comparison = "-";
            }
            else if (searchCondition.text.Contains(">"))
            {
                searchCondition.text.Replace(" ", string.Empty);
                statCondition = searchCondition.text.Split('>');
                singleComparison = int.Parse(statCondition[1]);
                comparison = ">";
            }
            else if (searchCondition.text.Contains("<"))
            {
                searchCondition.text.Replace(" ", string.Empty);
                statCondition = searchCondition.text.Split('<');
                singleComparison = int.Parse(statCondition[1]);
                comparison = ">";
            }
            else
            {
                searchCondition.text.Replace(" ", string.Empty);
                singleComparison = int.Parse(searchCondition.text);
                comparison = "=";
            }

            foreach (Item item in LoadingManager.openProject.items)
            {
                foreach (Stat stat in item.stats)
                {
                    if (stat.name.ToLower() == searchMode.options[searchMode.value].text)
                    {
                        int value = int.Parse(stat.value);

                        //comparisons
                        if (comparison == "-")
                        {
                            if (value >= betweenComarisons[0] && value <= betweenComarisons[1])
                            {
                                searchedItems.Add(item);
                                searchConditions.Add(stat.name + " = " + stat.value);
                            }
                        }
                        else if (comparison == ">")
                        {
                            if (value >= singleComparison)
                            {
                                searchedItems.Add(item);
                                searchConditions.Add(stat.name + " = " + stat.value);
                            }
                        }
                        else if (comparison == "<")
                        {
                            if (value <= singleComparison)
                            {
                                searchedItems.Add(item);
                                searchConditions.Add(stat.name + " = " + stat.value);
                            }
                        }
                        else if (comparison == "=")
                        {
                            if (value == singleComparison)
                            {
                                searchedItems.Add(item);
                                searchConditions.Add(stat.name + " = " + stat.value);
                            }
                        }
                    }
                }
            }
        }

        int i = 0;
        searchingText.text = "Indexing results...";
        yield return new WaitForSeconds(1);

        foreach (Item item in searchedItems)
        {
            ItemButton button = Instantiate(itemPrefab, itemList).GetComponent<ItemButton>();

            button.item = item;
            if (searchConditions.Count != 0)
            {
                button.conditionText.text = searchConditions[i];
                button.searchConditionsPanel.SetActive(true);
            }
            i++;
        }

        searchingPanel.SetActive(false);
        if(searchedItems.Count == 0)
        {
            conditionText.text = "No items were found where " + searchMode.options[searchMode.value].text + " = " + searchCondition.text + 
                ".\nYou may need to check your spelling or make sure your items meet the criteria.";
            conditionText.gameObject.SetActive(true);
        }
        yield return null;
    }
}
