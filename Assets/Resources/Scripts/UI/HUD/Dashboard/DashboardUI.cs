using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardUI : MonoBehaviour
{
    private GameObject _localCar;
    [SerializeField]  private HUD _hudScript;

    [Header("Weapon bar")]
    [SerializeField] private Image _bullet;
    [SerializeField] private Color _bulletEnabled;
    [SerializeField] private Color _bulletDisabled;
    [SerializeField] private Color _bulletNotAvailable;
    private GunBase _gun;

    [Space(10)]
    [SerializeField] private Image _rocket;
    [SerializeField] private Color _rocketEnabled;
    [SerializeField] private Color _rocketDisabled;
    [SerializeField] private Color _rocketNotAvailable;
    private RocketLauncher _rocketLauncher;

    [Header("Protection bar")]
    [SerializeField] private Image _smoke;
    [SerializeField] private Color _smokeEnabled;
    [SerializeField] private Color _smokeDisabled;
    [SerializeField] private Color _smokeNotAvailable;
    private CarProtectSystems _protectSystems;

    [Header("Other")]
    [SerializeField] private Image _headlights;
    [SerializeField] private Color _headlightsDisabled;
    [SerializeField] private Color _headlightsNearLight;
    [SerializeField] private Color _headlightsFarLight;
    private CarLights _lights;

    private IEnumerator _updateCoroutine;

    private void Start()
    {
        _hudScript.StartCoroutine(InitializeEvents());

        Disable();
    }

    private void OnEnable()
    {
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);

        _updateCoroutine = UpdateCoroutine();
        StartCoroutine(_updateCoroutine);
    }

    private void OnDisable()
    {
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            HandleGunPanel();
            HandleRocketPanel();
            HandleSmokePanel();
            HandleLightsPanel();

            yield return null;
        }
    }

    private void InitializeVariabled()
    {
        _localCar = Player.LocalPlayer.Car;

        if (_localCar.TryGetComponent(out GunBase gunBase))
            _gun = gunBase;

        if (_localCar.TryGetComponent(out RocketLauncher rocketLauncher))
            _rocketLauncher = rocketLauncher;

        if (_localCar.TryGetComponent(out CarProtectSystems carProtectSystems))
            _protectSystems = carProtectSystems;

        if (_localCar.TryGetComponent(out CarLights carLights))
            _lights = carLights;
    }

    private void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void HandleGunPanel()
    {
        if (_gun == null ||
            _gun.IsAmmoRunOut)
        {
            _bullet.color = _bulletNotAvailable;
            return;
        }

        if (_gun.IsActivated)
        {
            _bullet.color = _bulletEnabled;
            return;
        }
        else
        {
            _bullet.color = _bulletDisabled;
            return;
        }
    }

    private void HandleRocketPanel()
    {
        if (_rocketLauncher == null ||
            _rocketLauncher.IsRocketsRanOut)
        {
            _rocket.color = _rocketNotAvailable;
            return;
        }

        if (_rocketLauncher.IsActivated)
        {
            _rocket.color = _rocketEnabled;
            return;
        }
        else
        {
            _rocket.color = _rocketDisabled;
            return;
        }
    }

    private void HandleSmokePanel()
    {
        if (_protectSystems == null ||
            _protectSystems.IsSmokeRanOut)
        {
            _smoke.color = _smokeNotAvailable;
            return;
        }

        if (_protectSystems.IsActivated)
        {
            _smoke.color = _smokeEnabled;
            return;
        }
        else
        {
            _smoke.color = _smokeDisabled;
            return;
        }
    }

    private void HandleLightsPanel()
    {
        switch ((LightMode)_lights.CurrentLightModeIndex)
        {
            case LightMode.None:
                _headlights.color = _headlightsDisabled;
                break;

            case LightMode.Near:
                _headlights.color = _headlightsNearLight;
                break;

            case LightMode.Far:
                _headlights.color = _headlightsFarLight;
                break;

            default:
                break;
        }
    }

    private void AsignCar()
    {
        _hudScript.StartCoroutine(AsignCarCoroutine());
    }

    private IEnumerator AsignCarCoroutine()
    {
        while (Player.LocalPlayer.Car == null)
            yield return null;

        InitializeVariabled();
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
