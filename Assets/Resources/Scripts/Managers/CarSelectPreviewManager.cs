using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectPreviewManager : MonoBehaviour
{
    public static CarSelectPreviewManager Instance { get; set; }

    public uint SelectedCarIndex { get; set; } = 0;
    [field: SerializeField] public GameObject[] PreviewCars { get; set; }
    [field: SerializeField] private GameObject[] CarsInfo { get; set; }

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
#if UNITY_EDITOR || DEBUG_BUILD
            Debug.LogWarning($"Trying to create another one {nameof(CarSelectPreviewManager)} when it's Singleton." +
                $"The duplicate of {nameof(CarSelectPreviewManager)} will be destroyed");
#endif
            Destroy(gameObject);
        }
    }

    public void ShowCar(uint carIndex)
    {
        PreviewCars[SelectedCarIndex].SetActive(false);
        CarsInfo[SelectedCarIndex].SetActive(false);

        PreviewCars[carIndex].transform.position = transform.position + Vector3.up * 0.5f;
        PreviewCars[carIndex].transform.rotation = transform.rotation;

        PreviewCars[carIndex].SetActive(true);
        CarsInfo[carIndex].SetActive(true);
    }

    public void HideCar(uint carIndex)
    {
        PreviewCars[carIndex].SetActive(false);
        CarsInfo[carIndex].SetActive(false);

        PreviewCars[SelectedCarIndex].SetActive(true);
        CarsInfo[SelectedCarIndex].SetActive(true);
    }
}
