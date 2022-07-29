using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManagerUI : MonoBehaviour
{
    public static MessageManagerUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _bottonMessageBar;

    private void Start()
    {
        InitializeInstance();
    }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
#if UNITY_EDITOR
            Debug.Log($"Trying to create duplicate of {nameof(MessageManagerUI)} when it's not allowed. " +
                $"The duplicate will be destroyed.");
#endif
            Destroy(gameObject);
        }
    }

    public void ShowBottonMessage(string msg)
    {
        _bottonMessageBar.text = msg;
        _bottonMessageBar.gameObject.SetActive(true);
    }

    public void HideBottonMessage()
    {
        _bottonMessageBar.gameObject.SetActive(false);
    }
}
