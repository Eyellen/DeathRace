using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class GameChatUI : NetworkBehaviour
{
    [Header("Window")]
    [SerializeField] private GameObject _chatWindow;
    [Header("Input Field")]
    [SerializeField] private TMP_InputField _chatInputField;
    private bool _isInputFieldActive;

    [Header("ChatContent")]
    [SerializeField] private GameObject _chatMessagePrefab;
    [SerializeField] private GameObject _chatContent;
    private RectTransform _chatContentTransform;
    [SerializeField] private ScrollRect _chatContentScrollrect;

    [Header("Settings")]
    [SerializeField] private float _hideChatInSeconds = 5f;
    [SerializeField] private float _scrollbarSensitivity = 20;
    [SerializeField] private int _messageBuffer = 50;

    private IEnumerator _hideChatWindowCoroutine;

    private void Start()
    {
        _chatContentTransform = _chatContent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(_isInputFieldActive)
        {
            float scrollValue = Input.mouseScrollDelta.y;
            _chatContentScrollrect.verticalNormalizedPosition += scrollValue * _scrollbarSensitivity / _chatContentTransform.rect.height;
        }

        if (PlayerInput.IsChatPressed)
        {
            // Stop hiding coroutine if chat has been opened
            if (_hideChatWindowCoroutine != null)
                StopCoroutine(_hideChatWindowCoroutine);

            _isInputFieldActive = true;
            _chatWindow.SetActive(true);
            _chatInputField.ActivateInputField();
            PlayerInput.IsButtonsBlocked = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _isInputFieldActive = false;
            _chatInputField.DeactivateInputField();
            PlayerInput.IsButtonsBlocked = false;

            // Hide chat coroutine
            if (_hideChatWindowCoroutine != null)
                StopCoroutine(_hideChatWindowCoroutine);
            _hideChatWindowCoroutine = HideChatWindowCoroutine(_hideChatInSeconds);
            StartCoroutine(_hideChatWindowCoroutine);
        }
    }

    private IEnumerator HideChatWindowCoroutine(float inSeconds)
    {
        yield return new WaitForSeconds(inSeconds);

        _chatWindow.SetActive(false);
    }

    public void SendChatMessage(string msg)
    {
        if (msg == string.Empty) return;

        _isInputFieldActive = false;

        _chatInputField.DeactivateInputField();
        PlayerInput.IsButtonsBlocked = false;

        CmdSendChatMessage(Player.LocalPlayer.Username, msg);
        _chatInputField.text = string.Empty;
    }

    [Command(requiresAuthority = false)]
    private void CmdSendChatMessage(string from, string msg)
    {
        RpcSendChatMessage(from, msg);
    }

    [ClientRpc]
    private void RpcSendChatMessage(string from, string msg)
    {
        GameObject message = Instantiate(_chatMessagePrefab, _chatContent.transform);

        TextMeshProUGUI messageText = message.GetComponent<TextMeshProUGUI>();
        messageText.text = $"<color=\"red\">{from}</color>: {msg}";

        _chatWindow.SetActive(true);
        if(!_isInputFieldActive)
            StartCoroutine(UpdateScrollValue());

        if (!_isInputFieldActive)
        {
            if (_hideChatWindowCoroutine != null)
                StopCoroutine(_hideChatWindowCoroutine);
            _hideChatWindowCoroutine = HideChatWindowCoroutine(_hideChatInSeconds);
            StartCoroutine(_hideChatWindowCoroutine);
        }

        CheckIfNeedToDeleteMessage();
    }

    private IEnumerator UpdateScrollValue()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _chatContentScrollrect.verticalNormalizedPosition = 0;
    }

    private void CheckIfNeedToDeleteMessage()
    {
        if (_chatContent.transform.childCount > _messageBuffer)
        {
            Destroy(_chatContent.transform.GetChild(0).gameObject);
        }
    }
}
