using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class MazeGridGenerator : MonoBehaviour
{
    private List<GameObject> previousCells = new List<GameObject>();
    private MazeInput mazeInput;

    private void Awake()
    {
        mazeInput = GetComponent<MazeInput>();
    }

    public void InitVars()
    {
        // Remove all the cells we currently have
        if (previousCells.Count > 0)
        {
            foreach (GameObject cell in previousCells)
            {
                Destroy(cell);
            }
        }

        // Recalculate the cell width and height
        CellWidth = (float)Screen.width / (float)mazeInput.MazeColumns;
        CellHeight = (float)Screen.height / (float)mazeInput.MazeRows;

        // Empty the cells list for repopulation
        previousCells.Clear();
        MazeCells.Clear();
    }

    public void GenerateGrid()
    {
        // Nested for-loop to fill the screen with cells
        for (int y = 0; y < mazeInput.MazeRows; y++)
        {
            for (int x = 0; x < mazeInput.MazeColumns; x++)
            {
                // Make the cell and set the properties
                GameObject cell = Instantiate(mazeInput.CellPrefab);

                // We set the parent first because we want to set the position relative to the canvas
                cell.transform.SetParent(mazeInput.MazeCanvas.transform);

                // Set the position with a small offset because the pivot point of the cell is in the center
                cell.transform.position = new Vector2(x * CellWidth + (CellWidth / 2), y * CellHeight + (CellHeight / 2));
                cell.transform.localScale = new Vector2(CellWidth, CellHeight);
                cell.name = "Cell: " + (x + (y * mazeInput.MazeColumns));

                // Set the index of the cell
                Cell cellScript = cell.GetComponent<Cell>();
                cellScript.Position = new Vector2(x, y);
                cellScript.MazeRows = mazeInput.MazeRows;
                cellScript.MazeColumns = mazeInput.MazeColumns;

                // Set the cell colors
                cellScript.BackgroundColor = mazeInput.CellBackgroundColor;
                cellScript.WallColor = mazeInput.CellWallColor;
                cellScript.HighlightColor = mazeInput.CellHighlightColor;
                cellScript.VisitedColor = mazeInput.CellVisitedColor;

                previousCells.Add(cell);
                MazeCells.Add(cell, cellScript);
            }
        }

        // We do this at the end of generating the maze just so we have a start and end from the top left cell and bottom right cell
        MazeCells.ElementAt((mazeInput.MazeRows - 1) * mazeInput.MazeColumns).Value.RemoveWall(Cell.CellWalls.LeftWall);
        MazeCells.ElementAt(mazeInput.MazeColumns - 1).Value.RemoveWall(Cell.CellWalls.RightWall);
    }

    public Dictionary<GameObject, Cell> MazeCells { get; private set; } = new Dictionary<GameObject, Cell>();
    public float CellWidth { get; private set; }
    public float CellHeight { get; private set; }
}
