using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;

    private void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
    }

    private void OnMouseExit()
    {
        Debug.Log("OnMouseExit");
    }

    public void SelectCar()
    {
        SpawnManager.Instance.SelectedCar = _carPrefab;
    }
}
