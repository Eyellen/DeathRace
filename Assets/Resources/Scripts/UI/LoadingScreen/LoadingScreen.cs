using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _loadingText;
    private string _loadingDots = ".";

    private IEnumerator _handleLoadingEffectCoroutine;

    private void Start()
    {
        InitializeInstance();
        Disable();
    }

    private void OnEnable()
    {
        if (_handleLoadingEffectCoroutine != null)
            StopCoroutine(_handleLoadingEffectCoroutine);
        _handleLoadingEffectCoroutine = HandleLoadingEffectCoroutine();
        StartCoroutine(_handleLoadingEffectCoroutine);
    }

    private void OnDisable()
    {
        if (_handleLoadingEffectCoroutine != null)
            StopCoroutine(_handleLoadingEffectCoroutine);
    }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator HandleLoadingEffectCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.4f);

            if (_loadingDots.Length < 3)
                _loadingDots += ".";
            else
                _loadingDots = ".";

            _loadingText.text = "Loading" + _loadingDots;

            yield return null;
        }
    }
}
