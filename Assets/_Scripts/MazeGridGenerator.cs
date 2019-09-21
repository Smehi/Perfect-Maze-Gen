using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MazeGridGenerator : MonoBehaviour
{
    [Header("Maze properties")]
    private GameObject player;
    private List<GameObject> previousCells = new List<GameObject>();

    [Header("Cell properties")]
    private float cellWidth;
    private float cellHeight;

    public void Generate()
    {
        MazeInput.Instance.UserInputCanvas.SetActive(false);
        InitVars();
        GenerateGrid();
        GetComponent<MazeGenerator>().GenerateMaze();
    }

    private void InitVars()
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
        cellWidth = (float)Screen.width / (float)MazeInput.Instance.MazeColumns;
        cellHeight = (float)Screen.height / (float)MazeInput.Instance.MazeRows;

        // Empty the cells list for repopulation
        previousCells.Clear();
        MazeCells.Clear();
    }

    private void GenerateGrid()
    {
        // Nested for-loop to fill the screen with cells
        for (int y = 0; y < MazeInput.Instance.MazeRows; y++)
        {
            for (int x = 0; x < MazeInput.Instance.MazeColumns; x++)
            {
                // Make the cell and set the properties
                GameObject cell = Instantiate(MazeInput.Instance.CellPrefab);

                // We set the parent first because we want to set the position relative to the canvas
                cell.transform.SetParent(MazeInput.Instance.MazeCanvas.transform);

                // Set the position with a small offset because the pivot point of the cell is in the center
                cell.transform.position = new Vector2(x * cellWidth + (cellWidth / 2), y * cellHeight + (cellHeight / 2));
                cell.transform.localScale = new Vector2(cellWidth, cellHeight);
                cell.name = "Cell: " + (x + (y * MazeInput.Instance.MazeColumns));

                // Set the index of the cell
                Cell cellScript = cell.GetComponent<Cell>();
                cellScript.Position = new Vector2(x, y);
                cellScript.MazeRows = MazeInput.Instance.MazeRows;
                cellScript.MazeColumns = MazeInput.Instance.MazeColumns;

                // Set the cell colors
                cellScript.BackgroundColor = MazeInput.Instance.CellBackgroundColor;
                cellScript.WallColor = MazeInput.Instance.CellWallColor;
                cellScript.HighlightColor = MazeInput.Instance.CellHighlightColor;
                cellScript.VisitedColor = MazeInput.Instance.CellVisitedColor;

                previousCells.Add(cell);
                MazeCells.Add(cell, cellScript);
            }
        }

        // We do this at the end of generating the maze just so we have a start and end from the top left cell and bottom right cell
        MazeCells.ElementAt((MazeInput.Instance.MazeRows - 1) * MazeInput.Instance.MazeColumns).Value.RemoveWall(Cell.CellWalls.LeftWall);
        MazeCells.ElementAt(MazeInput.Instance.MazeColumns - 1).Value.RemoveWall(Cell.CellWalls.RightWall);
    }

    public void SpawnPlayer()
    {
        // We want to delete the previous player if there is one
        if (player)
        {
            Destroy(player);
        }
         
        // Make the player
        player = Instantiate(MazeInput.Instance.PlayerPrefab);

        // Set the parent for the same reasons as the cell
        player.transform.SetParent(MazeInput.Instance.MazeCanvas.transform);

        // Put the player in the top left cell
        player.transform.position = new Vector2(0 * cellWidth + (cellWidth / 2), (MazeInput.Instance.MazeRows - 1) * cellHeight + (cellHeight / 2));
        player.transform.localScale = new Vector2(cellWidth, cellHeight);
        player.name = "Player";

        // Set all the needed information 
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.MazeManager = this;
        pc.Position = new Vector2(0, MazeInput.Instance.MazeRows - 1);
        pc.Rows = MazeInput.Instance.MazeRows;
        pc.Columns = MazeInput.Instance.MazeColumns;
        pc.Width = cellWidth;
        pc.Height = cellHeight;
        pc.MazeCells = MazeCells;
    }

    public void ShowMenu()
    {
        MazeInput.Instance.UserInputCanvas.SetActive(true);
    }

    public Dictionary<GameObject, Cell> MazeCells { get; private set; } = new Dictionary<GameObject, Cell>();
}
