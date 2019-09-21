using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Wilsons : MonoBehaviour, IAlgorithm
{
    private float delay;
    private MazeGridGenerator mazeGridGenerator;
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

    public void Begin()
    {
        // Reset all the values
        delay = 0;
        mazeGridGenerator = null;
        currentCell = null;
        newStartCell = null;
        directedCells = new Dictionary<GameObject, Direction>();
        remaining = new List<GameObject>();
        foundMaze = false;

        mazeGridGenerator = GetComponent<MazeGridGenerator>();

        delay = MazeInput.Instance.Delay;

        foreach (KeyValuePair<GameObject, Cell> pair in mazeGridGenerator.MazeCells)
        {
            // We need to add all the cells into the remaining list to loop through
            remaining.Add(mazeGridGenerator.MazeCells[pair.Key].gameObject);
        }

        // Start generating the maze
        if (currentSolve != null)
        {
            StopCoroutine(currentSolve);
        }

        currentSolve = SolveMaze(delay);
        StartCoroutine(currentSolve);
    }
    
    public void End()
    {
        this.StopAllCoroutines();
    }

    private IEnumerator SolveMaze(float delay)
    {
        // We start by adding the first cell to the maze which will be our target
        GameObject start = remaining.ElementAt(Random.Range(0, remaining.Count));
        start.GetComponent<Cell>().IsVisited = true;
        start.GetComponent<Cell>().StopHighlightCell();
        remaining.Remove(start);

        while (remaining.Count > 0)
        {
            // We get a random new cell from the list that remains
            newStartCell = remaining.ElementAt(Random.Range(0, remaining.Count));
            currentCell = newStartCell;

            // We do this loop while we haven't found the maze yet
            do
            {
                // With the new cell we do a random walk which return a boolean: true if we found the maze, false if not
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

            // This is when we exit the second while which means we found the maze
            // Not it is time to erase the loops and make a path
            EraseLoop(newStartCell);
        }

        // Spawn the player when the algorithm is done with the maze
        mazeGridGenerator.SpawnPlayer();
    }

    private bool RandomWalk(GameObject cell)
    {
        GameObject randomNeighbour = null;
        int pos;
        Cell cellScript = cell.GetComponent<Cell>();

        // We choose a random direction to go in
        Direction randomDirection = (Direction)Random.Range(0, 4);

        // If we go into a direction which contains another cell we mark it as our neighbour
        switch (randomDirection)
        {
            case Direction.Up:
                pos = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y + 1);

                if (pos != -1)
                {
                    randomNeighbour = mazeGridGenerator.MazeCells.ElementAt(pos).Key;
                }

                break;
            case Direction.Right:
                pos = cellScript.GetIndex(cellScript.Position.x + 1, cellScript.Position.y);

                if (pos != -1)
                {
                    randomNeighbour = mazeGridGenerator.MazeCells.ElementAt(pos).Key;
                }

                break;
            case Direction.Down:
                pos = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y - 1);

                if (pos != -1)
                {
                    randomNeighbour = mazeGridGenerator.MazeCells.ElementAt(pos).Key;
                }

                break;
            case Direction.Left:
                pos = cellScript.GetIndex(cellScript.Position.x - 1, cellScript.Position.y);

                if (pos != -1)
                {
                    randomNeighbour = mazeGridGenerator.MazeCells.ElementAt(pos).Key;
                }

                break;
        }

        // If we went into a direction which had a neigbour we highlight the current cell
        // We also add it to a dictionary which hold the object as the key and the latest direction it went in as the value
        // At last we set the new cell to the neighbour we got so the process can repeat
        if (randomNeighbour)
        {
            cellScript.HighlightCell();
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
        // We check at the start is the object we put in is part of the dictionary
        // If it isn't it means we have done all the cells that were needed and got to the maze so we are done
        if (!directedCells.ContainsKey(cellObject))
        {
            // Reset all the cells that got highlighted but not added to the maze
            foreach (KeyValuePair<GameObject, Direction> pair in directedCells)
            {
                if (!pair.Key.GetComponent<Cell>().IsVisited)
                {
                    pair.Key.GetComponent<Image>().color = pair.Key.GetComponent<Cell>().BackgroundColor;
                }
            }

            // Clear the list and return for the next cycle 
            directedCells.Clear();
            return;
        }

        // Initialization values for this cell
        Cell cellScript;
        Direction cellDirection;

        cellScript = cellObject.GetComponent<Cell>();
        cellDirection = directedCells[cellObject];

        // Here we set the cell to be visited which means it is now part of the maze
        cellScript.IsVisited = true;
        cellScript.StopHighlightCell();

        // Because it is part of the maze now we have to remove it from the remaining cells list
        remaining.Remove(cellObject);

        // Here we check what direction this cell has
        // We remove the wall from this cell towards the direction and remove the opposite wall from the adjacent cell in that direction
        // After removing the walls we call this method again with the adjacent cell in that direction so the process can repeat
        switch (cellDirection)
        {
            case Direction.Up:
                int indexT = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y + 1);

                cellScript.RemoveWall(Cell.CellWalls.TopWall);
                mazeGridGenerator.MazeCells.ElementAt(indexT).Value.RemoveWall(Cell.CellWalls.BottomWall);
                EraseLoop(mazeGridGenerator.MazeCells.ElementAt(indexT).Key);

                break;
            case Direction.Right:
                int indexR = cellScript.GetIndex(cellScript.Position.x + 1, cellScript.Position.y);

                cellScript.RemoveWall(Cell.CellWalls.RightWall);
                mazeGridGenerator.MazeCells.ElementAt(indexR).Value.RemoveWall(Cell.CellWalls.LeftWall);

                EraseLoop(mazeGridGenerator.MazeCells.ElementAt(indexR).Key);
                break;
            case Direction.Down:
                int indexD = cellScript.GetIndex(cellScript.Position.x, cellScript.Position.y - 1);

                cellScript.RemoveWall(Cell.CellWalls.BottomWall);
                mazeGridGenerator.MazeCells.ElementAt(indexD).Value.RemoveWall(Cell.CellWalls.TopWall);
                EraseLoop(mazeGridGenerator.MazeCells.ElementAt(indexD).Key);

                break;
            case Direction.Left:
                int indexL = cellScript.GetIndex(cellScript.Position.x - 1, cellScript.Position.y);

                cellScript.RemoveWall(Cell.CellWalls.LeftWall);
                mazeGridGenerator.MazeCells.ElementAt(indexL).Value.RemoveWall(Cell.CellWalls.RightWall);
                EraseLoop(mazeGridGenerator.MazeCells.ElementAt(indexL).Key);

                break;
        }
    }
}
