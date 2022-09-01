using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MessageManager : NetworkBehaviour
{
    public static MessageManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _topMessageBar;
    [SerializeField] private TextMeshProUGUI _bottomMessageBar;

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

    #region TopMessage
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
    #endregion


    #region BottomMessage
    public void ShowBottomMessage(string msg)
    {
        _bottomMessageBar.text = msg;
        _bottomMessageBar.gameObject.SetActive(true);
    }

    public void ShowBottomMessage(string msg, float seconds)
    {
        StartCoroutine(ShowBottomMessageCoroutine(msg, seconds));
    }

    private IEnumerator ShowBottomMessageCoroutine(string msg, float seconds)
    {
        ShowBottomMessage(msg);
        yield return new WaitForSeconds(seconds);
        //HideBottomMessage();
        HideBottomMessageSmoothly();
    }

    [ClientRpc]
    public void RpcShowBottomMessage(string msg)
    {
        ShowBottomMessage(msg);
    }

    [ClientRpc]
    public void RpcShowBottomMessage(string msg, float seconds)
    {
        ShowBottomMessage(msg, seconds);
    }

    public void HideBottomMessage()
    {
        _bottomMessageBar.gameObject.SetActive(false);
    }

    public void HideBottomMessageSmoothly()
    {
        StartCoroutine(HideBottomMessageCoroutine());
    }

    private IEnumerator HideBottomMessageCoroutine()
    {
        float initialAlpha = _bottomMessageBar.alpha;

        while (_bottomMessageBar.alpha > 0)
        {
            _bottomMessageBar.alpha -= Time.deltaTime;

            yield return null;
        }

        _bottomMessageBar.gameObject.SetActive(false);
        _bottomMessageBar.alpha = initialAlpha;
    }

    [ClientRpc]
    public void RpcHideBottomMessage()
    {
        HideBottomMessage();
    }

    [ClientRpc]
    public void RpcHideBottomMessageSmoothly()
    {
        HideBottomMessageSmoothly();
    }
    #endregion


    public void HideAllMessages()
    {
        _topMessageBar.gameObject.SetActive(false);
        _bottomMessageBar.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcHideAllMessages()
    {
        HideAllMessages();
    }
}
