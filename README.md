# Eolas

A visual database for your data, from items in video games and inventory management, to recipes and account logs.

## What is Eolas?
Eolas, Gaelic for "knowledge", is a simple, but robust program that allows visual reading and storing of itemized lists. For example, a game developer can use Eolas to store item statistics in their games as well as requirements, descriptions, and crafting recipes. Eolas allows clean, and robust management of your data so everyone from artists and designers, to programmers can be on the same page. Goodbye spreadsheets!

## What can I do with Eolas?
Eolas is written to be simple for everyone to understand, but with enough versatility to be used in practically any scenario. We are using Eolas to keep track of our items in our in-development MMO, but we have seen Eolas used for item inventory management, HR people logs, and even recipe books. You can then share your Eolas project with other users so they can easily read and write data.

# Foreward before continuing
Eolas does not check for differences before reading and does not support merging. Because of this, if using Eolas in a development environment, we **strongly** suggest that you host your Eolas project on some flavour of Git, (Github, Atlassian, etc). This way, changes can be saved and merged easily, preventing file conflicts. If the project you are using Eolas for is on Git, we recommend putting the Eolas file in your project repository. 

At a later date, we plan to add file locking to Eolas, so only certain users can make changes, useful to prevent accidental changes to your project.

# Installation
The Eolas executable is found under [the releases page](http://https://github.com/Arylos07/Eolas/releases "the releases page") where you will find installers for Eolas. Select your OS and install. Make sure to run the installers as administrator to associate Eolas projects with Eolas. 

# Updating
Updating is simple. When a new version is released, Eolas will notify you. Simply download the new version from the releases page if you would like it and run the installer. The installer will then automatically update Eolas to the newest version.

## Downgrading
If the newest version isn't working for you for any reason, you are more than welcome to downgrade. Simply use the same process as updating to downgrade your version of Eolas.

# License
Eolas is developed for open source use, however it is protected by Creative Commons and an MIT License. Eolas is free to modify, but cannot be redistributed for commercial use (you cannot sell Eolas).

# Contribution
Eolas is released open source to allow developers, designers, and DevOps engineers tailor make Eolas to their needs. Instead of making Eolas for every possible scenario, we are releasing Eolas so you can make it work for your situation. 

Eolas is made using C# and uses the [Unity engine](https://unity3d.com/) as its base and graphical interface. 

To make changes to Eolas, you will need Unity 2019.1.1, although any version later should work. (if not, submit an issue). Versions later than 2018.1 may cause issues. 

1. [Download Unity 2019.1.1](https://unity3d.com/get-unity/download/archive "Download Unity 2019.1.1")
2. Create an issue so we can track your progress.
3. Fork the repository
4. Create a feature branch from master to work in. For example, if you want to add JSON data parsing, make a "json-data" branch from master.
5. Make your addition or fix.
6. If you want to keep your version private, you are more than welcome to do so. If you would like to submit your change to be part of Eolas, send a pull request to us.
