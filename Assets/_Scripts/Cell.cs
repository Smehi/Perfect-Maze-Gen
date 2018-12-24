using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // This will keep a reference to the walls that have been set in the editor
    [SerializeField] private GameObject[] walls;

    // This enum is here so removing walls can be done from other scripts
    // without confusion on which wall is being removed
    public enum CellWalls
    {
        TopWall,
        RightWall,
        BottomWall,
        LeftWall
    }

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
}
