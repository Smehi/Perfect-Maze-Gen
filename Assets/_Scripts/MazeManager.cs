using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [Header("Maze properties")]
    [SerializeField] private int mazeRows;
    [SerializeField] private int mazeColumns;
    [SerializeField] private GameObject mazeCanvas;
    [Range(0, 1)] [SerializeField] private float delay;

    private Dictionary<GameObject, Cell> mazeCells = new Dictionary<GameObject, Cell>();
    private List<GameObject> previousCells = new List<GameObject>();

    [Header("Cell properties")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Color cellBackgroundColor;
    [SerializeField] private Color cellWallColor;
    [SerializeField] private Color cellHighlightColor;
    [SerializeField] private Color cellVisitedColor;
    private float cellWidth;
    private float cellHeight;

    [Header("Algorithm")]
    [SerializeField] private Behaviour[] algoritmScripts;
    [SerializeField] private Dictionary<ChosenAlgorithm, Behaviour> algorithms = new Dictionary<ChosenAlgorithm, Behaviour>();
    [SerializeField] private ChosenAlgorithm chosenAlgorithm;
    private enum ChosenAlgorithm
    {
        BackTracker,
        RandomizedPrims
    }

    [Header("UI Variables")]
    [SerializeField] private GameObject userInputCanvas;
    [SerializeField] private Text rowsText;
    [SerializeField] private Slider rowsInput;
    [SerializeField] private Text columnsText;
    [SerializeField] private Slider columnsInput;
    [SerializeField] private Text delayText;
    [SerializeField] private Slider delayInput;
    [SerializeField] private Dropdown dropdownInput;

    [Header("UI Colour")]
    [SerializeField] private Image backgroundTextImage;
    [SerializeField] private Slider backgroundRed;
    [SerializeField] private Slider backgroundGreen;
    [SerializeField] private Slider backgroundBlue;
    [SerializeField] private Image wallTextImage;
    [SerializeField] private Slider wallRed;
    [SerializeField] private Slider wallGreen;
    [SerializeField] private Slider wallBlue;
    [SerializeField] private Image highlightTextImage;
    [SerializeField] private Slider highlightRed;
    [SerializeField] private Slider highlightGreen;
    [SerializeField] private Slider highlightBlue;
    [SerializeField] private Image visitedTextImage;
    [SerializeField] private Slider visitedRed;
    [SerializeField] private Slider visitedGreen;
    [SerializeField] private Slider visitedBlue;

    // Use this for initialization
    public void Generate()
    {
        userInputCanvas.SetActive(false);
        InitVars();
        GenerateGrid();
        GenerateMaze(chosenAlgorithm);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            userInputCanvas.SetActive(!userInputCanvas.activeSelf);
        }
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
        cellWidth = (float)Screen.width / (float)mazeColumns;
        cellHeight = (float)Screen.height / (float)mazeRows;

        // Empty the cells list for repopulation
        previousCells.Clear();
        MazeCells.Clear();

        // Populate the algorithms dictionary
        for (int i = 0; i < algoritmScripts.Length; i++)
        {
            algorithms[(ChosenAlgorithm)i] = algoritmScripts[i];
        }
    }

    private void GenerateGrid()
    {
        // Nested for-loop to fill the screen with cells
        for (int y = 0; y < mazeRows; y++)
        {
            for (int x = 0; x < mazeColumns; x++)
            {
                // Make the cell and set the properties
                GameObject cell = Instantiate(cellPrefab);

                // We set the parent first because we want to set the position relative to the canvas
                cell.transform.SetParent(mazeCanvas.transform);

                // Set the position with a small offset because the pivot point of the cell is in the center
                cell.transform.position = new Vector2(x * cellWidth + (cellWidth / 2), y * cellHeight + (cellHeight / 2));
                cell.transform.localScale = new Vector2(cellWidth, cellHeight);
                cell.name = "Cell: " + (x + (y * mazeColumns));

                // Set the index of the cell
                Cell cellScript = cell.GetComponent<Cell>();
                cellScript.Position = new Vector2(x, y);
                cellScript.MazeRows = mazeRows;
                cellScript.MazeColumns = mazeColumns;

                // Set the cell colors
                cellScript.BackgroundColor = cellBackgroundColor;
                cellScript.WallColor = cellWallColor;
                cellScript.HighlightColor = cellHighlightColor;
                cellScript.VisitedColor = cellVisitedColor;

                previousCells.Add(cell);
                MazeCells.Add(cell, cellScript);
            }
        }
    }

    private void GenerateMaze(ChosenAlgorithm chosen)
    {
        // Disable all algorithms
        for (int i = 0; i < algoritmScripts.Length; i++)
        {
            algorithms[(ChosenAlgorithm)i].enabled = false;
        }

        // Enable the chosen algorithm
        algorithms[chosen].enabled = true;

        // We need to have a switch so we can get the correct script and execute the start again.
        switch (chosen)
        {
            case ChosenAlgorithm.BackTracker:
                GetComponent<RandomizedPrims>().StopAllCoroutines();
                GetComponent<BackTracker>().Init();
                break;
            case ChosenAlgorithm.RandomizedPrims:
                GetComponent<BackTracker>().StopAllCoroutines();
                GetComponent<RandomizedPrims>().Init();
                break;
            default:
                break;
        }
    }

    public void ShowMenu()
    {
        userInputCanvas.SetActive(true);
    }

    public void SetAlgorithm()
    {
        chosenAlgorithm = (ChosenAlgorithm)dropdownInput.value;
    }

    public void SetBackgroundColor()
    {
        var r = backgroundRed.value;
        var g = backgroundGreen.value;
        var b = backgroundBlue.value;

        Color color = new Color(r, g, b);
        backgroundTextImage.color = color;
        cellBackgroundColor = color;
    }

    public void SetWallColor()
    {
        var r = wallRed.value;
        var g = wallGreen.value;
        var b = wallBlue.value;

        Color color = new Color(r, g, b);
        wallTextImage.color = color;
        cellWallColor = color;
    }

    public void SetHighlightColor()
    {
        var r = highlightRed.value;
        var g = highlightGreen.value;
        var b = highlightBlue.value;

        Color color = new Color(r, g, b);
        highlightTextImage.color = color;
        cellHighlightColor = color;
    }

    public void SetVisitedColor()
    {
        var r = visitedRed.value;
        var g = visitedGreen.value;
        var b = visitedBlue.value;

        Color color = new Color(r, g, b);
        visitedTextImage.color = color;
        cellVisitedColor = color;
    }

    public void SetRows()
    {
        mazeRows = (int)rowsInput.value;

        rowsText.text = $"{mazeRows} rows";
    }

    public void SetColumns()
    {
        mazeColumns = (int)columnsInput.value;

        columnsText.text = $"{mazeColumns} columns";
    }

    public void SetDelay()
    {
        delay = delayInput.value;

        if (delay > 0)
        {
            delayText.text = $"Delay {delay}s";
        }
        else
        {
            delayText.text = $"Framerate";
        }
    }

    public Dictionary<GameObject, Cell> MazeCells
    {
        get
        {
            return mazeCells;
        }
        private set
        {
            mazeCells = value;
        }
    }

    public float Delay
    {
        get { return delay; }
    }
}
