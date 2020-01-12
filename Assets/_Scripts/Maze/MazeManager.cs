using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    private MazeGridGenerator mazeGridGenerator;
    private MazeGenerator mazeGenerator;

    private void Awake()
    {
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
        MazeInput.Instance.UserInputCanvas.SetActive(true);
    }

    public void HideMenu()
    {
        MazeInput.Instance.UserInputCanvas.SetActive(false);
    }
}
