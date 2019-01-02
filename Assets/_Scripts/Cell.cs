﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    // This enum is here so removing walls can be done from other scripts
    // without confusion on which wall is being removed
    public enum CellWalls
    {
        TopWall,
        RightWall,
        BottomWall,
        LeftWall
    }

    // This will keep a reference to the walls that have been set in the editor
    [SerializeField] private GameObject[] walls;
    private bool isVisited;
    public Color backgroundColor;
    public Color wallColor;
    public Color highlightColor;
    public Color visitedColor;

    // Removes a wall with the help of the enum
    public void RemoveWall(CellWalls wall)
    {
        switch (wall)
        {
            case CellWalls.TopWall:
                walls[0].SetActive(false);
                break;
            case CellWalls.RightWall:
                walls[1].SetActive(false);
                break;
            case CellWalls.BottomWall:
                walls[2].SetActive(false);
                break;
            case CellWalls.LeftWall:
                walls[3].SetActive(false);
                break;
        }
    }

    public bool GetWallStatus(CellWalls wall)
    {
        switch (wall)
        {
            case CellWalls.TopWall:
                if (walls[0].activeSelf)
                {
                    return true;
                }

                break;
            case CellWalls.RightWall:
                if (walls[1].activeSelf)
                {
                    return true;
                }

                break;
            case CellWalls.BottomWall:
                if (walls[2].activeSelf)
                {
                    return true;
                }

                break;
            case CellWalls.LeftWall:
                if (walls[3].activeSelf)
                {
                    return true;
                }

                break;
        }

        return false;
    }

    public List<GameObject> GetWalls()
    {
        List<GameObject> newWalls = new List<GameObject>();

        foreach (GameObject wall in walls)
        {
            if (wall.activeSelf)
            {
                newWalls.Add(wall);
            }
        }

        return newWalls;
    }

    public GameObject GetRandomAvailableNeighbour()
    {
        List<GameObject> neighbours = new List<GameObject>();

        // Check if the position is available first and then add it
        Cell top = GetIndex(Position.x, Position.y + 1) != -1 ? MazeCells.ElementAt(GetIndex(Position.x, Position.y + 1)).Value : null;
        Cell right = GetIndex(Position.x + 1, Position.y) != -1 ? MazeCells.ElementAt(GetIndex(Position.x + 1, Position.y)).Value : null;
        Cell bottom = GetIndex(Position.x, Position.y - 1) != -1 ? MazeCells.ElementAt(GetIndex(Position.x, Position.y - 1)).Value : null;
        Cell left = GetIndex(Position.x - 1, Position.y) != -1 ? MazeCells.ElementAt(GetIndex(Position.x - 1, Position.y)).Value : null;


        // If the neighbours are available and not visited add them to our neighbours list
        if (top && !top.IsVisited)
        {
            neighbours.Add(top.gameObject);
        }

        if (right && !right.IsVisited)
        {
            neighbours.Add(right.gameObject);
        }

        if (bottom && !bottom.IsVisited)
        {
            neighbours.Add(bottom.gameObject);
        }

        if (left && !left.IsVisited)
        {
            neighbours.Add(left.gameObject);
        }

        // Choose one at random if we have neighbours
        if (neighbours.Count > 0)
        {
            return neighbours[Random.Range(0, neighbours.Count)];
        }
        else
        {
            return null;
        }
    }

    // Get the index of a neighbour that is some position away from you
    public int GetIndex(float x, float y)
    {
        if (x < 0 || y < 0 || x > MazeColumns - 1 || y > MazeRows - 1)
        {
            return -1;
        }

        return (int)(x + (y * MazeColumns));
    }

    public void StopHighlightCell()
    {
        GetComponent<Image>().color = visitedColor;
    }

    public Color BackgroundColor
    {
        get
        {
            return backgroundColor;
        }
        set
        {
            backgroundColor = value;
            GetComponent<Image>().color = backgroundColor;
        }
    }

    public Color WallColor
    {
        get
        {
            return wallColor;
        }
        set
        {
            wallColor = value;

            foreach (GameObject wall in walls)
            {
                wall.GetComponent<Image>().color = wallColor; 
            }
        }
    }

    public Color HighlightColor
    {
        get
        {
            return highlightColor;
        }
        set
        {
            highlightColor = value;
        }
    }

    public Color VisitedColor
    {
        get
        {
            return visitedColor;
        }
        set
        {
            visitedColor = value;
        }
    }

    public bool IsVisited
    {
        get
        {
            return isVisited;
        }
        set
        {
            isVisited = value;
            GetComponent<Image>().color = highlightColor;
        }
    }

    public Vector2 Position
    {
        get;
        set;
    }

    public int MazeRows
    {
        get;
        set;
    }

    public int MazeColumns
    {
        get;
        set;
    }

    public Dictionary<GameObject, Cell> MazeCells
    {
        get;
        set;
    }
}
