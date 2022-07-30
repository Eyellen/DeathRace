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

    public void Spawn()
    {
        //if (SpawnManager.Instance.SelectedCar == null) return;
        if (Player.LocalPlayer.Car != null)
        {
            SpawnManager.Instance.DestroyCurrentCar();
        }

        SpawnManager.Instance.Spawn();
        SetActive(false);
    }

    public void Spectate()
    {
        Player.LocalPlayer.Car?.GetComponent<CarDamageable>().DestroySelf();
    }
}
