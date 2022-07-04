using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;
    [SerializeField] private uint _carIndex;

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
        Debug.Log($"Selected {_carPrefab}");
        SpawnManager.Instance.SelectedCarIndex = _carIndex;
    }
}
