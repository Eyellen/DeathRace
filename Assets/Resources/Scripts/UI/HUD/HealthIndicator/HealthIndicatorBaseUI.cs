using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthIndicatorBaseUI : MonoBehaviour
{
    [SerializeField] private HUD _hudScript;
    [SerializeField] private uint _carIndex;
    [SerializeField] private RectTransform _healthFillerTransform;
    private Image _healthFillerImage;
    protected IDamageable<int> _damageable;

    [Header("Health colors")]
    [SerializeField] private Color _maxHealthColor = new Color32(21, 255, 0, 255);
    [SerializeField] private Color _minHealthColor = new Color32(255, 0, 0, 255);

    [Header("Additional settings")]
    [SerializeField] private bool _useAxisX;
    [SerializeField] private bool _useAxisY;
    [SerializeField] private bool _isDisableOnMinHealth;

    private void Start()
    {
        _healthFillerImage = _healthFillerTransform.GetComponent<Image>();

        _hudScript.StartCoroutine(InitializeEvents());

        Disable();
    }

    private void Update()
    {
        if (_damageable != null)
        {
            float ratio = _damageable.HealthRatio;
            _healthFillerTransform.localScale = new Vector3(_useAxisX ? ratio : 1, _useAxisY ? ratio : 1, 1);
            _healthFillerImage.color = Color.Lerp(_minHealthColor, _maxHealthColor, ratio);

            if (ratio <= 0)
                gameObject.SetActive(!_isDisableOnMinHealth);
        }
        else
        {
            _healthFillerTransform.localScale = new Vector3(0, 1, 1);
            _healthFillerImage.color = _minHealthColor;
        }
    }

    private void Enable(uint carIndex)
    {
        if (_carIndex != carIndex) return;

        gameObject.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void AsignDamageable(uint carIndex)
    {
        if (carIndex != _carIndex) return;

        _hudScript.StartCoroutine(AsignDamageableCoroutine());
    }

    //private IEnumerator AsignCarCoroutine()
    //{
    //    while (Player.LocalPlayer.Car == null)
    //        yield return null;

    //    _damageable = Player.LocalPlayer.Car.GetComponent<CarDamageable>();
    //}
    protected abstract IEnumerator AsignDamageableCoroutine();

    private IEnumerator InitializeEvents()
    {
        while (SpawnManager.Instance == null)
            yield return null;

        SpawnManager.Instance.OnLocalCarSpawnedIndex += AsignDamageable;
        SpawnManager.Instance.OnLocalCarSpawnedIndex += Enable;

        SpawnManager.Instance.OnLocalCarDestroyed += Disable;
        SpawnManager.Instance.OnLocalCarRemoved += Disable;
    }
}
