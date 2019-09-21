using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0.15f, 0.3f)] [SerializeField] private float moveInterval;
    private float nextMove = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextMove)
        {
            // Get input to move the player
            // We also return after a keystroke because we don't wan't players to cross diagonally
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                SetNewPosition(new Vector2(Position.x, Position.y + 1));
                nextMove = Time.time + moveInterval;
                return;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                SetNewPosition(new Vector2(Position.x - 1, Position.y));
                nextMove = Time.time + moveInterval;
                return;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                SetNewPosition(new Vector2(Position.x, Position.y - 1));
                nextMove = Time.time + moveInterval;
                return;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                SetNewPosition(new Vector2(Position.x + 1, Position.y));
                nextMove = Time.time + moveInterval;
                return;
            }
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

    public MazeGridGenerator MazeManager
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
