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
        currentCell = canvas.GetChild(Random.Range(0, canvas.childCount)).gameObject;

        while (unVisited.Count > 0)
        {
            cells[currentCell].IsVisited = true;

            var next = cells[currentCell].GetNeighbour();
            if (next)
            {
                cells[next].IsVisited = true;
                currentCell = next;
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
}
