using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wilsons : MonoBehaviour
{
    private float delay;
    private MazeManager mazeManager;
    private GameObject currentCell;
    private GameObject newStartCell;
    private Dictionary<GameObject, Direction> directedCells = new Dictionary<GameObject, Direction>();
    private List<GameObject> remaining = new List<GameObject>();
    private IEnumerator currentSolve;
    private bool foundMaze;

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    // Use this for initialization
    public void Init()
    {
        // Reset all the values
        delay = 0;
        mazeManager = null;
        currentCell = null;
        newStartCell = null;
        directedCells = new Dictionary<GameObject, Direction>();
        remaining = new List<GameObject>();
        foundMaze = false;

        mazeManager = GetComponent<MazeManager>();

        delay = mazeManager.Delay;

        foreach (KeyValuePair<GameObject, Cell> pair in mazeManager.MazeCells)
        {
            // We need to add all the cells into the remaining list to loop through
            remaining.Add(mazeManager.MazeCells[pair.Key].gameObject);
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
        GameObject start = remaining.ElementAt(Random.Range(0, remaining.Count));
        start.GetComponent<Cell>().IsVisited = true;
        start.GetComponent<Cell>().StopHighlightCell();
        remaining.Remove(start);

        while (remaining.Count > 0)
        {
            newStartCell = remaining.ElementAt(Random.Range(0, remaining.Count));
            currentCell = newStartCell;

            do
            {
                foundMaze = RandomWalk(currentCell);

                if (delay > 0)
                {
                    yield return new WaitForSeconds(delay);
                }
                else
                {
                    yield return null;
                }
            }
            while (!foundMaze);

            EraseLoop(newStartCell);
        }
    }

    private bool RandomWalk(GameObject cell)
    {
        GameObject randomNeighbour = null;
        int pos;
        Cell cellScript = cell.GetComponent<Cell>();

        Direction randomDirection = (Direction)Random.Range(0, 4);

        // Directions are up, right, down, left
        switch (randomDirection)
        {
            case Direction.Up:
                pos = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y + 1);

                if (pos != -1)
                {
                    randomNeighbour = mazeManager.MazeCells.ElementAt(pos).Key;
                }

                break;
            case Direction.Right:
                pos = cellScript.GetIndex(cellScript.Position.x + 1, cellScript.Position.y);

                if (pos != -1)
                {
                    randomNeighbour = mazeManager.MazeCells.ElementAt(pos).Key;
                }

                break;
            case Direction.Down:
                pos = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y - 1);

                if (pos != -1)
                {
                    randomNeighbour = mazeManager.MazeCells.ElementAt(pos).Key;
                }

                break;
            case Direction.Left:
                pos = cellScript.GetIndex(cellScript.Position.x - 1, cellScript.Position.y);

                if (pos != -1)
                {
                    randomNeighbour = mazeManager.MazeCells.ElementAt(pos).Key;
                }

                break;
        }

        if (randomNeighbour)
        {
            directedCells[cell] = randomDirection;
            currentCell = randomNeighbour;
        }

        // If we made it back to a part of the maze return true, otherwise return false
        if (randomNeighbour && randomNeighbour.GetComponent<Cell>().IsVisited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void EraseLoop(GameObject cellObject)
    {
        if (!directedCells.ContainsKey(cellObject))
        {
            directedCells.Clear();
            return;
        }

        Cell cellScript;
        Direction cellDirection;

        cellScript = cellObject.GetComponent<Cell>();
        cellDirection = directedCells[cellObject];

        cellScript.IsVisited = true;
        cellScript.StopHighlightCell();
        remaining.Remove(cellObject);

        switch (cellDirection)
        {
            case Direction.Up:
                int indexT = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y + 1);

                cellScript.RemoveWall(Cell.CellWalls.TopWall);
                mazeManager.MazeCells.ElementAt(indexT).Value.RemoveWall(Cell.CellWalls.BottomWall);
                EraseLoop(mazeManager.MazeCells.ElementAt(indexT).Key);

                break;
            case Direction.Right:
                int indexR = cellScript.GetIndex(cellScript.Position.x + 1, cellScript.Position.y);

                cellScript.RemoveWall(Cell.CellWalls.RightWall);
                mazeManager.MazeCells.ElementAt(indexR).Value.RemoveWall(Cell.CellWalls.LeftWall);

                EraseLoop(mazeManager.MazeCells.ElementAt(indexR).Key);
                break;
            case Direction.Down:
                int indexD = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y - 1);

                cellScript.RemoveWall(Cell.CellWalls.BottomWall);
                mazeManager.MazeCells.ElementAt(indexD).Value.RemoveWall(Cell.CellWalls.TopWall);
                EraseLoop(mazeManager.MazeCells.ElementAt(indexD).Key);

                break;
            case Direction.Left:
                int indexL = cellScript.GetIndex(cellScript.Position.x - 1, cellScript.Position.y);

                cellScript.RemoveWall(Cell.CellWalls.LeftWall);
                mazeManager.MazeCells.ElementAt(indexL).Value.RemoveWall(Cell.CellWalls.RightWall);
                EraseLoop(mazeManager.MazeCells.ElementAt(indexL).Key);

                break;
        }
    }
}
