using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    private MazeInput mazeInput;
    private MazeGridGenerator mazeGridGenerator;
    private MazeGenerator mazeGenerator;

    private void Awake()
    {
        mazeInput = GetComponent<MazeInput>();
        mazeGridGenerator = GetComponent<MazeGridGenerator>();
        mazeGenerator = GetComponent<MazeGenerator>();
    }

    public void Generate()
    {
        HideMenu();
        mazeGridGenerator.InitVars();
        mazeGridGenerator.GenerateGrid();
        mazeGenerator.GenerateMaze();
    }

    public void ShowMenu()
    {
        mazeInput.UserInputCanvas.SetActive(true);
    }

    public void HideMenu()
    {
        mazeInput.UserInputCanvas.SetActive(false);
    }
}
