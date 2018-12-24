using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTracker : MonoBehaviour
{
    private MazeManager mazeManager;
    private GameObject currentCell;
    private Dictionary<GameObject, Cell> cells = new Dictionary<GameObject, Cell>();

    // Use this for initialization
    void Start()
    {
        mazeManager = GetComponent<MazeManager>();

        cells = mazeManager.MazeCells;

        Transform canvas = GameObject.Find("Canvas").transform;
        currentCell = canvas.GetChild(Random.Range(0, canvas.childCount)).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
