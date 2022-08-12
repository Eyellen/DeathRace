using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarHealthIndicatorUI : MonoBehaviour
{
    [SerializeField] private Color _green;
    [SerializeField] private Color _red;

    [SerializeField] private RectTransform _carHealthFiller;
    private Image _carHealthFillerImage;
    private CarDamageable _car;

    private void Start()
    {
        _carHealthFillerImage = _carHealthFiller.GetComponent<Image>();

        SpawnManager.Instance.OnLocalCarSpawned += AsignCar;
        SpawnManager.Instance.OnLocalCarSpawned += Enable;

        SpawnManager.Instance.OnLocalCarDestroyed += Disable;

        Disable();
    }

    private void Update()
    {
        if (_car != null)
        {
            float ratio = _car.HealthRemainingRatio;
            _carHealthFiller.localScale = new Vector3(ratio, 1, 1);
            _carHealthFillerImage.color = Color.Lerp(_red, _green, ratio);
        }
        else
        {
            _carHealthFiller.localScale = new Vector3(0, 1, 1);
            _carHealthFillerImage.color = _red;
        }
    }

    private void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void AsignCar()
    {
        _car = Player.LocalPlayer.Car.GetComponent<CarDamageable>();
    }
}
