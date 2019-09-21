using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    private MazeInput mazeInput;
    private IAlgorithm[] algorithms; 
    private IAlgorithm currentAlgorithm;

    public void Awake()
    {
        mazeInput = GetComponent<MazeInput>();
        algorithms = GetComponents<IAlgorithm>();
    }

	public void GenerateMaze()
    {
        if (currentAlgorithm != null)
            currentAlgorithm.End();

        currentAlgorithm = algorithms[mazeInput.ChosenAlgorithm];
        currentAlgorithm.Begin();
    }
}
