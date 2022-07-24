using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceModeManager : MonoBehaviour
{
    public static RaceModeManager Instance { get; private set; }

    [field: SerializeField] 
    public CheckPoint[] CheckPoints { get; private set; }

    private int _lapsCompleted = 0;

    private void Start()
    {
        InitializeInstance();
        InitializeCheckPoints();
    }

    private void OnEnable()
    {
        //foreach (var checkPoint in CheckPoints)
        //{
        //    checkPoint.gameObject.SetActive(true);
        //}
        CheckPoints[0].transform.root.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        CheckPoints[0].transform.root.gameObject.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
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
            // Initializing index
            CheckPoints[i].CheckPointIndex = i;

            // Initializing event
            CheckPoints[i].OnCheckPointPassed += OnCheckPointPassed;
        }
    }

    private void OnCheckPointPassed(int checkPointIndex)
    {
        // When passing last point we are resetting initial point to be able to pass it
        if (checkPointIndex == CheckPoints.Length - 1)
        {
            CheckPoints[0].ResetPoint();
        }

        if (checkPointIndex == 0)
        {
            if(CheckPoints[CheckPoints.Length - 1].IsPassed)
            {
                _lapsCompleted++;
                ResetAllCheckPoints();
            }

            CheckPoints[checkPointIndex].MarkAsPassed();

            return;
        }

        // Mark current point as passed only if previous point was passed
        if (CheckPoints[checkPointIndex - 1].IsPassed)
        {
            CheckPoints[checkPointIndex].MarkAsPassed();
        }
    }

    private void ResetAllCheckPoints()
    {
        foreach (var checkPoint in CheckPoints)
        {
            checkPoint.ResetPoint();
        }
    }
}
