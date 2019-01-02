# Perfect-Maze-Gen

## General
A perfect maze generator made in Unity/C#.

The project is setup so it runs from the moment you download/clone it. There are some settings that are set in the inspector
which act as the default values. These settings are also present in the UI. There are also a couple variables which need to be set in the inspector such as the cell prefab and cell parent. There are also loads of UI elements that are set in the inspector. If you don't want to add your own UI elements you don't need to touch these.

## Adding your own algorithm
If you want to use this project to add your own maze algorithms you can do so by:
1. Add it to the ChosenAlgorithm enum in MazeManager.cs.
2. Call the script in the switch in the GenerateMaze method in MazeManager.cs.
3. Add the script to the Maze Generator GameObject.
4. Increase the size of Algorithm Scripts in the inspector on the Maze Generator GameObject.
5. Add the script that is on the Maze Generator GameObject in the array that you just increased.
6. Find the dropdown component in the canvas and add a new option (name can be whatever you want, it is linked to the position of the enum in step 1).

## Controls
ESC - hide/show settings menu  
Mouse - change settings in menu  
WASD - move player after algorithm is done generating  
↑←↓→ - move player after algorithm is done generating
