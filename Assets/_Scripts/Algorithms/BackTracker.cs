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

    // Use this for initialization
    void Start()
    {
        mazeManager = GetComponent<MazeManager>();

        delay = mazeManager.Delay;
        cells = mazeManager.MazeCells;

        canvas = GameObject.Find("Canvas").transform;

        foreach (KeyValuePair<GameObject, Cell> pair in cells)
        {
            unVisited.Add(cells[pair.Key].gameObject);

            Dictionary<int, Cell> newDic = new Dictionary<int, Cell>();

            foreach (KeyValuePair<GameObject, Cell> pair2 in cells)
            {
                newDic.Add(cells[pair2.Key].GetIndex(cells[pair2.Key].Position.x, cells[pair2.Key].Position.y), cells[pair2.Key]);
            }

            cells[pair.Key].MazeCells = newDic;
        }

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
            // Step 2.1
            var nextCell = cells[currentCell].GetNeighbour();

            if (nextCell)
            {
                cells[nextCell].IsVisited = true;

                // Step 2.2

                // Step 2.3
                RemoveWalls(cells[currentCell], cells[nextCell]);

                // Step 2.4
                currentCell = nextCell;
            }

            //unVisited.Remove(currentCell);

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
