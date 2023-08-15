# Welcome to CTFPV!
By Yunivers

| Table of Contents | Description |
|--|--|
| [What is CTFPV?](https://github.com/CTFAK/CTFPV#what-is-ctfpv) | A short description of what CTFPV is and what it's used for. |
| [Installation](https://github.com/CTFAK/CTFPV#installation) | How to install a precompiled version of CTFPV. |
| [Compilation](https://github.com/CTFAK/CTFPV#compilation) | How to compile CTFPV manually. |
| [Usage](https://github.com/CTFAK/CTFPV#usage) | How to use CTFPV. |
| [To-Do List](https://github.com/CTFAK/CTFPV#to-do-list) | What needs to be done to mark CTFPV as complete. |
| [Full Credits](https://github.com/CTFAK/CTFPV#full-credits) | Everyone who helped make CTFPV a reality. |

# What is CTFPV?
CTFPV (Standing for **C**lick**T**eam **F**usion **P**ointer **V**iewer) is a tool developed by Yunivers which can be used to view and modify the loaded properties of a game made in Clickteam Fusion 2.5 on runtime.

CTFPV currently allows you view values of almost everything currently loaded within the game, edit those values, and copy the pointers of those values for use in Cheat Engine, Autosplitting Language, Trainers/Mod Menus, etc.

# Installation
## Dependencies
CTFPV requires [.NET 6.0's Runtime, Core Runtime, and Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

After running the x64 installers for all 3 runtimes, you may proceed with the installation.
## Installing a precompiled release

To install an release, you should make your way over to [Releases](https://github.com/CTFAK/CTFPV/releases), and from there, if you scroll down you should find `CTFPV.zip`, from there just click on `CTFPV.zip` and it will start downloading.

Once it's downloaded, extract the .zip file to an empty folder.

# Compilation
## Dependencies
CTFPV requires [.NET 6.0's Runtime, Core Runtime, and Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

After running the x64 installers for all 3 runtimes, you may proceed with the compilation.

## Cloning the repo with Visual Studio

Make sure you have [Visual Studio](https://visualstudio.microsoft.com/) installed and open.

On the GitHub repository, click `Code` and copy the HTTPS URL.

In Visual Studio, under `Get started`, click `Clone a repository`, then paste the HTTPS URL from earlier. Input your desired path and press `Clone`.

## Compiling CTFPV

**Compiling CTFPV is not recommended.** Please go to [installation](https://github.com/CTFAK/CTFPV#installation) to download CTFPV precompiled.

If you'd like to compile CTFPV anyway, right click the solution on the right and press `Build Solution` or do it through the key bind `Control + Shift + B`, then right click the solution once again and press `Open Folder in File Explorer`.

In the File Explorer go to `bin\x64\Debug\net6.0-windows`, from there you should be able to run `CTFPV.exe` without problems!

# Usage
CTFPV may be difficult to navigate for those unfamiliar with the structure of Clickteam's Runtime.

To get started, open `CTFPV.exe` and press the drop down box, from there, find your Clickteam Fusion 2.5 game and press 'Load'.

You should now be able to see the game's name, the currently loaded frame, and all objects that are active in the scene. (Excluding Backdrops)

If you click on any of those, the properties panel on the bottom with fill with the values that item is related to. You may edit most of the values there, although some may not do what you expect, if anything at all. This is due to how the runtime is coded.

If you right click almost anything, there will be an option to copy the pointer that points to that item/value.

However, if you right click the game title, you will have a list of actions you may perform relating to the game's storyboard.

If you run into a ***crash***, you may [open an issue](https://github.com/CTFAK/CTFAK2.0/issues). If the issue is not a crash, please do not open an issue.

# To-Do List
|%| Task |Description
|--|--|--|
| 2% | This | Write a to-do list. |
| 0% | Run Header | Add a way to edit the Run Header. |

# Full Credits
|Name| Credit for... |
|--|--|
| [Yunivers](https://github.com/AITYunivers) | Developer of CTFPV. |
| [Clickteam](https://www.clickteam.com/) | For supplying the XNA Runtime and Extension SDK for reference. |

CTFPV is licensed under [AGPL-3.0](https://github.com/CTFAK/CTFPV/blob/master/LICENSE.txt).

Readme Last Updated August 15th, 2023.
