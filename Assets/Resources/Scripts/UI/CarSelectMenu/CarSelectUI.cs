using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectUI : MonoBehaviour
{
    private void OnEnable()
    {
        CursorManager.ShowCursor();
        PlayerInput.IsBlocked = true;
    }

    private void OnDisable()
    {
        CursorManager.HideCursor();
        PlayerInput.IsBlocked = false;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Confirm()
    {
        //if (SpawnManager.Instance.SelectedCar == null) return;

        SpawnManager.Instance.Spawn();
        SetActive(false);
    }
}
