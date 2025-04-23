using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public GameObject targetPrefab;
    public int rows = 5;
    public int cols = 5;
    public float spacing = 2f;
    public int activeTargets = 4;

    private List<Vector3> gridPositions = new List<Vector3>();
    private List<Target> currentTargets = new List<Target>();

    void Start()
    {
        GenerateGrid();
        SpawnInitialTargets();
    }

    void GenerateGrid()
    {
        Vector3 origin = transform.position;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // En lugar de generar sobre el plano XY, lo hacemos en XZ
                Vector3 pos = origin + new Vector3(x * spacing, y * spacing, 0); // sigue en XY


                gridPositions.Add(pos);
            }
        }
    }

    void SpawnInitialTargets()
    {
        for (int i = 0; i < activeTargets; i++)
        {
            SpawnTarget();
        }
    }

    public void SpawnTarget()
    {
        List<Vector3> availablePositions = new List<Vector3>(gridPositions);

        foreach (var t in currentTargets)
        {
            availablePositions.Remove(t.transform.position);
        }

        if (availablePositions.Count == 0) return;

        Vector3 spawnPos = availablePositions[Random.Range(0, availablePositions.Count)];

        // Rotación: mirando hacia atrás en Z (hacia el jugador que está en Z=0)
        Quaternion rotation = Quaternion.LookRotation(Vector3.back);

        GameObject newTarget = Instantiate(targetPrefab, spawnPos, rotation);
        Target targetScript = newTarget.GetComponent<Target>();
        targetScript.gridManager = this;
        currentTargets.Add(targetScript);
    }

    public void TargetHit(Target target)
    {
        currentTargets.Remove(target);
        SpawnTarget(); // Reemplaza el target destruido
    }
}
