using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTracker : MonoBehaviour, IAlgorithm
{
    private float delay;
    private MazeGridGenerator mazeGridGenerator;
    private PlayerSpawner playerSpawner;
    private GameObject currentCell;
    private Dictionary<GameObject, Cell> cells = new Dictionary<GameObject, Cell>();
    private List<GameObject> unVisited = new List<GameObject>();
    private List<GameObject> stack = new List<GameObject>();
    private IEnumerator currentSolve;

    public void Begin()
    {
        // Reset all the values
        delay = 0;
        mazeGridGenerator = null;
        playerSpawner = null;
        currentCell = null;
        cells = new Dictionary<GameObject, Cell>();
        unVisited = new List<GameObject>();
        stack = new List<GameObject>();

        mazeGridGenerator = GetComponent<MazeGridGenerator>();
        playerSpawner = GetComponent<PlayerSpawner>();

        delay = MazeInput.Instance.Delay;
        cells = mazeGridGenerator.MazeCells;
        
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

    public void End()
    {
        this.StopAllCoroutines();
    }

    private IEnumerator SolveMaze(float delay)
    {
        // Step 1
        System.Random rand = new System.Random();
        currentCell = cells.ElementAt(rand.Next(0, cells.Count)).Key;
        cells[currentCell].IsVisited = true;

        // Step 2
        while (unVisited.Count > 0)
        {
            cells[currentCell].StopHighlightCell();
         
            // Step 2.1
            var nextCell = cells[currentCell].GetRandomAvailableNeighbour();

            if (nextCell)
            {
                cells[nextCell].IsVisited = true;

                // Step 2.2
                stack.Add(currentCell);

                // Step 2.3
                RemoveWalls(cells[currentCell], cells[nextCell]);

                // Step 2.4
                currentCell = nextCell;
            }
            else if (stack.Count > 0)
            {
                currentCell = stack[stack.Count - 1];
                cells[currentCell].IsVisited = true;
                stack.RemoveAt(stack.Count - 1);
            }

            unVisited.Remove(currentCell);

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            else
            {
                yield return null;
            }
        }

        // We put this line here just to stop highlighting the last cell because we dont enter the while loop again
        cells[currentCell].StopHighlightCell();

        // Spawn the player when the algorithm is done with the maze
        playerSpawner.SpawnPlayer(mazeGridGenerator.CellWidth, mazeGridGenerator.CellHeight, mazeGridGenerator.MazeCells);
    }

    private void RemoveWalls(Cell cell1, Cell cell2)
    {
        // Check whether cell1 is on the left or right side and remove walls accordingly
        int x = (int)(cell1.Position.x - cell2.Position.x);
        switch (x)
        {
            case -1:
                cell1.RemoveWall(Cell.CellWalls.RightWall);
                cell2.RemoveWall(Cell.CellWalls.LeftWall);
                break;
            case 1:
                cell1.RemoveWall(Cell.CellWalls.LeftWall);
                cell2.RemoveWall(Cell.CellWalls.RightWall);
                break;
            default:
                break;
        }

        // Check whether cell1 is on the top or bottom and remove walls accordingly
        var y = (int)(cell1.Position.y - cell2.Position.y);
        switch (y)
        {
            case -1:
                cell1.RemoveWall(Cell.CellWalls.TopWall);
                cell2.RemoveWall(Cell.CellWalls.BottomWall);
                break;
            case 1:
                cell1.RemoveWall(Cell.CellWalls.BottomWall);
                cell2.RemoveWall(Cell.CellWalls.TopWall);
                break;
            default:
                break;
        }
    }
}
