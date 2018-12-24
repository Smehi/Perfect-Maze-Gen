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
            GetComponent<Image>().color = Color.blue;
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
