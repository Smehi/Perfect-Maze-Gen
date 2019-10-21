# Perfect-Maze-Gen

## General
A perfect maze generator made in Unity/C#.

The project is setup so it runs from the moment you download/clone it. There are some settings that are set in the inspector
which act as the default values. These settings are also present in the UI. There are also a couple variables which need to be set in the inspector such as the cell prefab and cell parent. There are also loads of UI elements that are set in the inspector. If you don't want to add your own UI elements you don't need to touch these.

## Adding your own algorithm
If you want to use this project to add your own maze algorithms you can do so by:
1. Make sure your algorithm script uses the IAlgorithm interface.
2. Add the algorithm script to the GameObject that has all the other scripts.
3. Add a value to the dropdown enum under the Canvas object.
4. Make sure the algorithm script on the object match the order of the dropdown values.

## Controls
ESC - hide/show settings menu  
Mouse - change settings in menu  
WASD - move player after algorithm is done generating  
↑←↓→ - move player after algorithm is done generating
