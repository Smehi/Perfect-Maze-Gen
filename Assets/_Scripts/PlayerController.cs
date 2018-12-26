using System.Collections;
using System.Collections.Generic;
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

        // TODO: check if the player is not trying to pass through a wall.

        Position = newPosition;
        transform.position = new Vector2(newPosition.x * Width + (Width / 2), newPosition.y * Height + (Height / 2));
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

    // TODO: Get a list of all the cells
}
