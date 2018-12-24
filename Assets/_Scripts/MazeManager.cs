using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [Header("Maze properties")]
    [SerializeField] private int mazeRows;
    [SerializeField] private int mazeColumns;
    [SerializeField] private GameObject mazeCanvas;

    [Header("Cell properties")]
    [SerializeField] private GameObject cellPrefab;
    private Dictionary<GameObject, Cell> mazeCells = new Dictionary<GameObject, Cell>();
    private float cellWidth;
    private float cellHeight;

    [Header("Algorithm")]
    [SerializeField] private Behaviour[] algoritmScripts;
    [SerializeField] private Dictionary<ChosenAlgorithm, Behaviour> algorithms = new Dictionary<ChosenAlgorithm, Behaviour>();
    [SerializeField] private ChosenAlgorithm chosenAlgorithm;
    private enum ChosenAlgorithm
    {
        BackTracker
    }

    // Use this for initialization
    void Start()
    {
        InitVars();
        GenerateGrid();
        GenerateMaze(chosenAlgorithm);
    }

    private void InitVars()
    {
        // Remove all the cells we currently have
        for (int i = 0; i < mazeCanvas.transform.childCount; i++)
        {
            Destroy(mazeCanvas.transform.GetChild(i));
        }

        // Recalculate the cell width and height
        cellWidth = Screen.width / mazeColumns;
        cellHeight = Screen.height / mazeRows;

        // Empty the cells list for repopulation
        mazeCells.Clear();

        // Populate the algorithms dictionary
        for (int i = 0; i < algoritmScripts.Length; i++)
        {
            algorithms[(ChosenAlgorithm)i] = algoritmScripts[i];
        }
    }

    private void GenerateGrid()
    {
        Vector2 spawnPos = Vector2.zero;

        // Nested for-loop to fill the screen with cells
        for (int y = 0; y < mazeRows; y++)
        {
            for (int x = 0; x < mazeColumns; x++)
            {
                // Make the cell and set the properties
                GameObject cell = Instantiate(cellPrefab);

                // We set the parent first because we want to set the position relative to the canvas
                cell.transform.SetParent(mazeCanvas.transform);

                // Set the position with a small offset because the pivot point of the cell is in the center
                cell.transform.position = new Vector2(x * cellWidth + (cellWidth / 2), y * cellHeight + (cellHeight / 2));
                cell.transform.localScale = new Vector2(cellWidth, cellHeight);
                cell.name = "Cell: " + ((x + (y * mazeRows)) + 1);

                mazeCells.Add(cell, cell.GetComponent<Cell>());
            }
        }
    }

    private void GenerateMaze(ChosenAlgorithm chosen)
    {
        // Disable all algorithms
        for (int i = 0; i < algoritmScripts.Length; i++)
        {
            algorithms[(ChosenAlgorithm)i].enabled = false;
        }

        // Enable the chosen algorithm
        algorithms[chosen].enabled = true;
    }

    public Dictionary<GameObject, Cell> MazeCells
    {
        get { return mazeCells; }
    }
}
