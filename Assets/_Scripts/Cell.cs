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
    private bool[] wallState = new bool[] { true, true, true, true };
    private bool isVisited;

    // Removes a wall with the help of the enum
    public void RemoveWall(CellWalls wall)
    {
        switch (wall)
        {
            case CellWalls.TopWall:
                if (GetWallState(wall))
                {
                    walls[0].SetActive(false);
                    wallState[0] = false;
                }
                break;
            case CellWalls.RightWall:
                if (GetWallState(wall))
                {
                    walls[1].SetActive(false);
                    wallState[1] = false;
                }
                break;
            case CellWalls.BottomWall:
                if (GetWallState(wall))
                {
                    walls[2].SetActive(false);
                    wallState[2] = false;
                }
                break;
            case CellWalls.LeftWall:
                if (GetWallState(wall))
                {
                    walls[3].SetActive(false);
                    wallState[3] = false;
                }
                break;
        }
    }

    public bool GetWallState(CellWalls wall)
    {
        switch (wall)
        {
            case CellWalls.TopWall:
                return wallState[0];
            case CellWalls.RightWall:
                return wallState[1];
            case CellWalls.BottomWall:
                return wallState[2];
            case CellWalls.LeftWall:
                return wallState[3];
        }

        return false;
    }

    public GameObject GetNeighbour()
    {
        List<GameObject> neighbours = new List<GameObject>();
        
        Cell top = GetIndex(Position.x, Position.y + 1) != -1 ? MazeCells[GetIndex(Position.x, Position.y + 1)] : null;
        Cell right = GetIndex(Position.x + 1, Position.y) != -1 ? MazeCells[GetIndex(Position.x + 1, Position.y)] : null;
        Cell bottom = GetIndex(Position.x, Position.y - 1) != -1 ? MazeCells[GetIndex(Position.x, Position.y - 1)] : null;
        Cell left = GetIndex(Position.x - 1, Position.y) != -1 ? MazeCells[GetIndex(Position.x - 1, Position.y)] : null;

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

        if (neighbours.Count > 0)
        {
            return neighbours[Random.Range(0, neighbours.Count)];
        }
        else
        {
            return null;
        }
    }

    public int GetIndex(float x, float y)
    {
        if (x < 0 || y < 0 || x > MazeColumns - 1 || y > MazeRows - 1)
        {
            return -1;
        }

        return (int)(x + (y * MazeRows));
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
            GetComponent<Image>().color = Color.cyan;
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

    public Dictionary<int, Cell> MazeCells
    {
        get;
        set;
    }
}
