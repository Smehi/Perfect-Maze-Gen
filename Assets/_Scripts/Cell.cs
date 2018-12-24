using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
