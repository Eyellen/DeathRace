using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceModeManager : MonoBehaviour
{
    public static RaceModeManager Instance { get; private set; }

    [field: SerializeField] 
    public CheckPoint[] CheckPoints { get; private set; }

    private void Start()
    {
        InitializeInstance();
        InitializeCheckPoints();
    }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void InitializeCheckPoints()
    {
        for (int i = 0; i < CheckPoints.Length; i++)
        {
            CheckPoints[i].CheckPointIndex = i;
        }
    }
}
