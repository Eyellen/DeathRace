using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectPreviewManager : MonoBehaviour
{
    public static CarSelectPreviewManager Instance { get; set; }

    public uint SelectedCarIndex { get; set; }
    [field: SerializeField] public GameObject[] PreviewCars { get; set; }

    private void Start()
    {
        InitializeInstance();
        PreviewCars[SelectedCarIndex].SetActive(true);
    }

    private void InitializeInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Trying to create another one {nameof(CarSelectPreviewManager)} when it's Singleton." +
                $"The duplicate of {nameof(CarSelectPreviewManager)} will be destroyed");
#endif
            Destroy(gameObject);
        }
    }

    public void ShowCar(uint carIndex)
    {
        PreviewCars[SelectedCarIndex].SetActive(false);
        PreviewCars[carIndex].SetActive(true);
    }

    public void HideCar(uint carIndex)
    {
        PreviewCars[carIndex].SetActive(false);
        PreviewCars[SelectedCarIndex].SetActive(true);
    }
}
