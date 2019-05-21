# Eolas

A visual database for your data, from items in video games and inventory management, to recipes and account logs.

## What is Eolas?
Eolas, Gaelic for "knowledge", is a simple, but robust program that allows visual reading and storing of itemized lists. For example, a game developer can use Eolas to store item statistics in their games as well as requirements, descriptions, and crafting recipes. Eolas allows clean, and robust management of your data so everyone from artists and designers, to programmers can be on the same page. Goodbye spreadsheets!

## What can I do with Eolas?
Eolas is written to be simple for everyone to understand, but with enough versatility to be used in practically any scenario. We are using Eolas to keep track of our items in our in-development MMO, but we have seen Eolas used for item inventory management, HR people logs, and even recipe books. You can then share your Eolas project with other users so they can easily read and write data.

# Foreward before continuing
Eolas does not check for differences before reading and does not support merging. Because of this, if using Eolas in a development environment, we **strongly** suggest that you host your Eolas project on some flavour of Git, (Github, Atlassian, etc). This way, changes can be saved and merged easily, preventing file conflicts. If the project you are using Eolas for is on Git, we recommend putting the Eolas file in your project repository. 

At a later date, we plan to add file locking to Eolas, so only certain users can make changes, useful to prevent accidental changes to your project.

# How to use item filtering.
Item filtering does not have a walkthrough or tooltips added just yet, but here's how you can take advantage of this complex system.

1. Select what you want to filter from the dropdown. Any stats you add to items will be automatically added to this dropdown. 
2. Enter what you want to search. You can enter names, parts of the descriptions, item ID's, and any stats.
3. Press "Search". Eolas will begin filtering items. Items will also display how they met your search criteria. Don't forget to press "Reset" to disable the filtering.

# Commands for filtering
When filtering stats, you have a cool feature when it comes to managing numerical stats. You can search for ranges, less thans, or greater thans. 

- Ranges: Ranges can be searched using "x-y". For example, to see an item with a stat between 1 and 50, enter "1-50" into the search bar when searching. Any items with a numerical value in that stat field will be searched and if they are within that range, they will be displayed as well as display how they met your criteria. 

- Less than: Searching for items who have a stat less than what you're searching for, simply enter ">x" in the search bar. For example, to find any items with a stat less than 50, enter ">50" in the search bar.

- Greater than: Like less than, you can search greater than using "<x", so items with a value greater than 1 can be searched using "<1".

# Installation
The Eolas executable is found under [the releases page](https://github.com/Arylos07/Eolas/releases "the releases page") where you will find installers for Eolas. Select your OS and install. Make sure to run the installers as administrator to associate Eolas projects with Eolas. You can also clone the installer from the repository.

# Updating
Updating is simple. When a new version is released, Eolas will notify you. Simply download the new version from the releases page if you would like it and run the installer. The installer will then automatically update Eolas to the newest version.

## Downgrading
If the newest version isn't working for you for any reason, you are more than welcome to downgrade. Simply use the same process as updating to downgrade your version of Eolas.

# License
Eolas is developed for open source use, however it is protected by Creative Commons and an MIT License. Eolas is free to modify, but cannot be redistributed for commercial use (you cannot sell Eolas).

# Contribution
Eolas is released open source to allow developers, designers, and DevOps engineers tailor make Eolas to their needs. Instead of making Eolas for every possible scenario, we are releasing Eolas so you can make it work for your situation. 

Eolas is made using C# and uses the [Unity engine](https://unity3d.com/) as its base and graphical interface. 

To make changes to Eolas, you will need Unity 2018.3.0f2, although any version later should work. (if not, submit an issue). Versions later than 2018.3 may cause issues. 2019.1+ has a critical bug preventing windowed builds, so until the bug is fully resolved (amongst other bugs), any 2019 version is not recommended.

1. [Download Unity 2018.3.0f2](https://unity3d.com/get-unity/download/archive "Download Unity 2019.3.0f2"). Do not use any 2019 version, as there is a critical bug preventing windowed building.
2. Create an issue so we can track your progress.
3. Fork the repository
4. Create a feature branch from master to work in. For example, if you want to add JSON data parsing, make a "json-data" branch from master.
5. Make your addition or fix.
6. If you want to keep your version private, you are more than welcome to do so. If you would like to submit your change to be part of Eolas, send a pull request to us.
