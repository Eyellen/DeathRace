using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarHealthIndicatorUI : MonoBehaviour
{
    [SerializeField] private HUD _hudScript;
    [SerializeField] private Color _green = new Color32(21, 255, 0, 255);
    [SerializeField] private Color _red = new Color32(255, 0, 0, 255);

    [SerializeField] private RectTransform _carHealthFiller;
    private Image _carHealthFillerImage;
    private CarDamageable _car;

    private void Start()
    {
        _carHealthFillerImage = _carHealthFiller.GetComponent<Image>();

        _hudScript.StartCoroutine(InitializeEvents());

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
        _hudScript.StartCoroutine(AsignCarCoroutine());
    }

    private IEnumerator AsignCarCoroutine()
    {
        while (Player.LocalPlayer.Car == null)
            yield return null;

        _car = Player.LocalPlayer.Car.GetComponent<CarDamageable>();
    }

    private IEnumerator InitializeEvents()
    {
        while (SpawnManager.Instance == null)
            yield return null;

        SpawnManager.Instance.OnLocalCarSpawned += AsignCar;
        SpawnManager.Instance.OnLocalCarSpawned += Enable;

        SpawnManager.Instance.OnLocalCarDestroyed += Disable;
    }
}
