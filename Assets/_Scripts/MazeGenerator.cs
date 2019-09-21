using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    private IAlgorithm[] algorithms; 
    private IAlgorithm currentAlgorithm;

    public void Awake()
    {
        algorithms = GetComponents<IAlgorithm>();
    }

	public void GenerateMaze()
    {
        if (currentAlgorithm != null)
            currentAlgorithm.End();

        currentAlgorithm = algorithms[MazeInput.Instance.ChosenAlgorithm];
        currentAlgorithm.Begin();
    }
}
