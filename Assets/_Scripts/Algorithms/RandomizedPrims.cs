﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomizedPrims : MonoBehaviour
{
    private float delay;
    private MazeManager mazeManager;
    private GameObject currentCell;
    private Dictionary<GameObject, Cell> cells = new Dictionary<GameObject, Cell>();
    private List<GameObject> unVisited = new List<GameObject>();
    private List<GameObject> walls = new List<GameObject>();
    private IEnumerator currentSolve;

    // Use this for initialization
    public void Init()
    {
        // Reset all the values
        delay = 0;
        mazeManager = null;
        currentCell = null;
        cells = new Dictionary<GameObject, Cell>();
        unVisited = new List<GameObject>();
        walls = new List<GameObject>();

        mazeManager = GetComponent<MazeManager>();

        delay = mazeManager.Delay;
        cells = mazeManager.MazeCells;

        foreach (KeyValuePair<GameObject, Cell> pair in cells)
        {
            // We need to add all the cells into the unvisted list to loop through
            unVisited.Add(cells[pair.Key].gameObject);

            // We also need to inject the dictionary with all the cells into every cell to find neighbours
            cells[pair.Key].MazeCells = cells;
        }

        // Start generating the maze
        if (currentSolve != null)
        {
            StopCoroutine(currentSolve);
        }

        currentSolve = SolveMaze(delay);
        StartCoroutine(currentSolve);
    }

    private IEnumerator SolveMaze(float delay)
    {
        // Step 2
        System.Random rand = new System.Random();
        currentCell = cells.ElementAt(rand.Next(0, cells.Count)).Key;
        cells[currentCell].IsVisited = true;

        walls.AddRange(cells[currentCell].GetWalls());

        // Step 3
        while (walls.Count > 0)
        {
            // Step 3.1
            var currentWall = walls[Random.Range(0, walls.Count)];
            currentCell = cells[currentWall.transform.parent.gameObject].gameObject;

            // Step 3.1.1
            switch (currentWall.name)
            {
                case "Top Wall":
                    int indexT = cells[currentCell].GetIndex(cells[currentCell].Position.x, cells[currentCell].Position.y + 1);

                    if (indexT != -1)
                    {
                        GameObject topNeighbour = cells.ElementAt(indexT).Key;

                        if (cells[currentCell].IsVisited && !cells[topNeighbour].IsVisited)
                        {
                            cells[currentCell].RemoveWall(Cell.CellWalls.TopWall);
                            cells[topNeighbour].RemoveWall(Cell.CellWalls.BottomWall);
                            cells[topNeighbour].IsVisited = true;

                            // Step 3.1.2
                            walls.AddRange(cells[topNeighbour].GetWalls());
                        }
                    }

                    break;
                case "Right Wall":
                    int indexR = cells[currentCell].GetIndex(cells[currentCell].Position.x + 1, cells[currentCell].Position.y);

                    if (indexR != -1)
                    {
                        GameObject rightNeighbour = cells.ElementAt(indexR).Key;

                        if (cells[currentCell].IsVisited && !cells[rightNeighbour].IsVisited)
                        {
                            cells[currentCell].RemoveWall(Cell.CellWalls.RightWall);
                            cells[rightNeighbour].RemoveWall(Cell.CellWalls.LeftWall);
                            cells[rightNeighbour].IsVisited = true;

                            // Step 3.1.2
                            walls.AddRange(cells[rightNeighbour].GetWalls());
                        }
                    }

                    break;
                case "Bottom Wall":
                    int indexB = cells[currentCell].GetIndex(cells[currentCell].Position.x, cells[currentCell].Position.y - 1);

                    if (indexB != -1)
                    {
                        GameObject bottomNeighbour = cells.ElementAt(indexB).Key;

                        if (cells[currentCell].IsVisited && !cells[bottomNeighbour].IsVisited)
                        {
                            cells[currentCell].RemoveWall(Cell.CellWalls.BottomWall);
                            cells[bottomNeighbour].RemoveWall(Cell.CellWalls.TopWall);
                            cells[bottomNeighbour].IsVisited = true;

                            // Step 3.1.2
                            walls.AddRange(cells[bottomNeighbour].GetWalls());
                        }
                    }

                    break;
                case "Left Wall":
                    int indexL = cells[currentCell].GetIndex(cells[currentCell].Position.x - 1, cells[currentCell].Position.y);

                    if (indexL != -1)
                    {
                        GameObject leftNeighbour = cells.ElementAt(indexL).Key;

                        if (cells[currentCell].IsVisited && !cells[leftNeighbour].IsVisited)
                        {
                            cells[currentCell].RemoveWall(Cell.CellWalls.LeftWall);
                            cells[leftNeighbour].RemoveWall(Cell.CellWalls.RightWall);
                            cells[leftNeighbour].IsVisited = true;

                            // Step 3.1.2
                            walls.AddRange(cells[leftNeighbour].GetWalls());
                        }
                    }

                    break;
            }

            cells[currentCell].StopHighlightCell();

            // Step 3.2
            walls.Remove(currentWall);

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            else
            {
                yield return null;
            }
        }
    }
}
