using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance { get; private set; }

    [SerializeField] private GameObject _content;
    [SerializeField] private TextMeshProUGUI _loadingText;
    private string _loadingDots = ".";

    private IEnumerator _handleLoadingEffectCoroutine;

    private void Start()
    {
        InitializeInstance();
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
        _content.SetActive(true);

        if (_handleLoadingEffectCoroutine != null)
            StopCoroutine(_handleLoadingEffectCoroutine);
        _handleLoadingEffectCoroutine = HandleLoadingEffectCoroutine();
        StartCoroutine(_handleLoadingEffectCoroutine);
    }

    public void Disable()
    {
        _content.SetActive(false);

        if (_handleLoadingEffectCoroutine != null)
            StopCoroutine(_handleLoadingEffectCoroutine);
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
