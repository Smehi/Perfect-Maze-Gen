using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class MazeInput : MonoBehaviour
{
    #region Singleton
    private static MazeInput instance;

    public static MazeInput Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("No instance of MazeInput has been found in the scene!");
            
            return instance;
        }
    }
    #endregion

    [Header("Maze properties")]
    [SerializeField] private int mazeRows;
    [SerializeField] private int mazeColumns;
    [SerializeField] private GameObject mazeCanvas;
    [SerializeField] private GameObject playerPrefab;
    [Range(0, 1)] [SerializeField] private float delay;

    [Header("Cell properties")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Color cellBackgroundColor;
    [SerializeField] private Color cellWallColor;
    [SerializeField] private Color cellHighlightColor;
    [SerializeField] private Color cellVisitedColor;

    [Header("Algorithm")]
    private int chosenAlgorithm;

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

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            userInputCanvas.SetActive(!userInputCanvas.activeSelf);
        }
    }

    public void SetAlgorithm()
    {
        chosenAlgorithm = dropdownInput.value;
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

    #region Properties
    public int MazeRows
    {
        get { return mazeRows; }
    }

    public int MazeColumns
    {
        get { return mazeColumns; }
    }

    public GameObject MazeCanvas
    {
        get { return mazeCanvas; }
    }

    public GameObject PlayerPrefab
    {
        get { return playerPrefab; }
    }

    public float Delay
    {
        get { return delay; }
    }

    public GameObject CellPrefab
    {
        get { return cellPrefab; }
    }

    public Color CellBackgroundColor
    {
        get { return cellBackgroundColor; }
    }

    public Color CellWallColor
    {
        get { return cellWallColor; }
    }

    public Color CellHighlightColor
    {
        get { return cellHighlightColor; }
    }

    public Color CellVisitedColor
    {
        get { return cellVisitedColor; }
    }

    public int ChosenAlgorithm
    {
        get { return chosenAlgorithm; }
    }

    public GameObject UserInputCanvas
    {
        get { return userInputCanvas; }
    }
    #endregion
}
