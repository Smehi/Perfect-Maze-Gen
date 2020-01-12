using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private MazeManager mazeManager;
    private GameObject player;

    private void Awake()
    {
        mazeManager = GetComponent<MazeManager>();
    }

    public void SpawnPlayer(float cellWidth, float cellHeight, Dictionary<GameObject, Cell> mazeCells)
    {
        // We want to delete the previous player if there is one
        if (player)
        {
            Destroy(player);
        }

        // Make the player
        player = Instantiate(MazeInput.Instance.PlayerPrefab);

        // Set the parent for the same reasons as the cell
        player.transform.SetParent(MazeInput.Instance.MazeCanvas.transform);

        // Put the player in the top left cell
        player.transform.position = new Vector2(0 * cellWidth + (cellWidth / 2), (MazeInput.Instance.MazeRows - 1) * cellHeight + (cellHeight / 2));
        player.transform.localScale = new Vector2(cellWidth, cellHeight);
        player.name = "Player";

        // Set all the needed information 
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.MazeManager = mazeManager;
        pc.Position = new Vector2(0, MazeInput.Instance.MazeRows - 1);
        pc.Rows = MazeInput.Instance.MazeRows;
        pc.Columns = MazeInput.Instance.MazeColumns;
        pc.Width = cellWidth;
        pc.Height = cellHeight;
        pc.MazeCells = mazeCells;
    }
}
