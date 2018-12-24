using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTracker : MonoBehaviour
{
    private float delay;
    private MazeManager mazeManager;
    private Transform canvas;
    private GameObject currentCell;
    private Dictionary<GameObject, Cell> cells = new Dictionary<GameObject, Cell>();
    private List<GameObject> unVisited = new List<GameObject>();
    private List<GameObject> stack = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        mazeManager = GetComponent<MazeManager>();

        delay = mazeManager.Delay;
        cells = mazeManager.MazeCells;

        canvas = GameObject.Find("Canvas").transform;

        // Add all the cells to the unvisited list to loop through when making the maze
        foreach (KeyValuePair<GameObject, Cell> pair in cells)
        {
            unVisited.Add(cells[pair.Key].gameObject);

            // Make a new dictionary that stores a int and Cell script.
            // The int will represent the ID of the cell that is stored in the script so checking for neighbours is easier.
            // Your fps will tank because of this, but I couldn't figure out a more efficient way at the time.
            Dictionary<int, Cell> newDic = new Dictionary<int, Cell>();

            // Populate the new dictionary
            foreach (KeyValuePair<GameObject, Cell> pair2 in cells)
            {
                newDic.Add(cells[pair2.Key].GetIndex(cells[pair2.Key].Position.x, cells[pair2.Key].Position.y), cells[pair2.Key]);
            }

            // Add the dictionary to the cell
            cells[pair.Key].MazeCells = newDic;
        }

        // Start generating the maze
        StartCoroutine(SolveMaze(delay));
    }

    private IEnumerator SolveMaze(float delay)
    {
        // Step 1
        currentCell = canvas.GetChild(Random.Range(0, canvas.childCount)).gameObject;
        cells[currentCell].IsVisited = true;

        // Step 2
        while (unVisited.Count > 0)
        {
            cells[currentCell].StopHighlightCell();

            // Step 2.1
            var nextCell = cells[currentCell].GetNeighbour();

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
