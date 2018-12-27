using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Get input to move the player
        // We also return after a keystroke because we don't wan't players to cross diagonally.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetNewPosition(new Vector2(Position.x, Position.y + 1));
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetNewPosition(new Vector2(Position.x - 1, Position.y));
            return;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetNewPosition(new Vector2(Position.x, Position.y - 1));
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetNewPosition(new Vector2(Position.x + 1, Position.y));
            return;
        }
    }

    private void SetNewPosition(Vector2 newPosition)
    {
        // Check if we are not going out the map fist
        if (newPosition.x < 0 || newPosition.y < 0 || newPosition.x > Columns - 1 || newPosition.y > Rows - 1)
        {
            return;
        }
        
        // Check for the right or left wall
        switch ((int)Position.x - (int)newPosition.x)
        {
            case -1:
                if (MazeCells.ElementAt((int)(Position.x + Position.y * Columns)).Value.GetWallStatus(Cell.CellWalls.RightWall))
                {
                    return;
                }

                break;
            case 1:
                if (MazeCells.ElementAt((int)(Position.x + Position.y * Columns)).Value.GetWallStatus(Cell.CellWalls.LeftWall))
                {
                    return;
                }

                break;
        }

        // Check for the top or bottom wall
        switch ((int)Position.y - (int)newPosition.y)
        {
            case -1:
                if (MazeCells.ElementAt((int)(Position.x + Position.y * Columns)).Value.GetWallStatus(Cell.CellWalls.TopWall))
                {
                    return;
                }

                break;
            case 1:
                if (MazeCells.ElementAt((int)(Position.x + Position.y * Columns)).Value.GetWallStatus(Cell.CellWalls.BottomWall))
                {
                    return;
                }

                break;
        }

        Position = newPosition;
        transform.position = new Vector2(newPosition.x * Width + (Width / 2), newPosition.y * Height + (Height / 2));

        // Show the menu when we reach the end of the maze
        if (Position == new Vector2(Columns - 1, 0))
        {
            MazeManager.ShowMenu();
        }
    }

    public MazeManager MazeManager
    {
        get;
        set;
    }

    public Vector2 Position
    {
        get;
        set;
    }

    public int Rows
    {
        get;
        set;
    }

    public int Columns
    {
        get;
        set;
    }

    public float Width
    {
        get;
        set;
    }

    public float Height
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
