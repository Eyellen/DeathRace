using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MessageManager : NetworkBehaviour
{
    public static MessageManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _topMessageBar;
    [SerializeField] private TextMeshProUGUI _bottonMessageBar;

    private void Awake()
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
            Debug.Log($"Trying to create duplicate of {nameof(MessageManager)} when it's not allowed. " +
                $"The duplicate will be destroyed.");
#endif
            Destroy(gameObject);
        }
    }

    public void ShowTopMessage(string msg)
    {
        _topMessageBar.text = msg;
        _topMessageBar.gameObject.SetActive(true);
    }

    public void ShowTopMessage(string msg, float seconds)
    {
        StartCoroutine(ShowTopMessageCoroutine(msg, seconds));
    }

    private IEnumerator ShowTopMessageCoroutine(string msg, float seconds)
    {
        ShowTopMessage(msg);
        yield return new WaitForSeconds(seconds);
        //HideTopMessage();
        HideTopMessageSmoothly();
    }

    [ClientRpc]
    public void RpcShowTopMessage(string msg)
    {
        ShowTopMessage(msg);
    }

    [ClientRpc]
    public void RpcShowTopMessage(string msg, float seconds)
    {
        ShowTopMessage(msg, seconds);
    }

    public void HideTopMessage()
    {
        _topMessageBar.gameObject.SetActive(false);
    }

    public void HideTopMessageSmoothly()
    {
        StartCoroutine(HideTopMessageCoroutine());
    }

    private IEnumerator HideTopMessageCoroutine()
    {
        float initialAlpha = _topMessageBar.alpha;

        while (_topMessageBar.alpha > 0)
        {
            _topMessageBar.alpha -= Time.deltaTime;

            yield return null;
        }

        _topMessageBar.gameObject.SetActive(false);
        _topMessageBar.alpha = initialAlpha;
    }

    [ClientRpc]
    public void RpcHideTopMessage()
    {
        HideTopMessage();
    }

    [ClientRpc]
    public void RpcHideTopMessageSmoothly()
    {
        HideTopMessageSmoothly();
    }

    public void ShowBottomMessage(string msg)
    {
        _bottonMessageBar.text = msg;
        _bottonMessageBar.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcShowBottomMessage(string msg)
    {
        ShowBottomMessage(msg);
    }

    public void HideBottomMessage()
    {
        _bottonMessageBar.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcHideBottomMessage()
    {
        HideBottomMessage();
    }

    public void HideAllMessages()
    {
        _topMessageBar.gameObject.SetActive(false);
        _bottonMessageBar.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcHideAllMessages()
    {
        HideAllMessages();
    }
}
