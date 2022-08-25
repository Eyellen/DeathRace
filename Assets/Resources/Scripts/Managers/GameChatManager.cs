using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class GameChatManager : NetworkBehaviour
{
    public static GameChatManager Instance { get; private set; }

    [Header("Window")]
    [SerializeField] private GameObject _chatWindow;
    [Header("Input Field")]
    [SerializeField] private TMP_InputField _chatInputField;
    public bool IsInputFieldActive { get; private set; }
    public bool IsInputFieldVisible { get => _chatInputField.gameObject.activeSelf; }

    [Header("ChatContent")]
    [SerializeField] private GameObject _chatMessagePrefab;
    [SerializeField] private GameObject _chatContent;
    private RectTransform _chatContentTransform;
    [SerializeField] private ScrollRect _chatScrollrect;

    [Header("Settings")]
    [SerializeField] private float _hideChatInSeconds = 5f;
    [SerializeField] private float _scrollbarSensitivity = 20;
    [SerializeField] private int _messageBuffer = 50;

    private IEnumerator _hideChatWindowCoroutine;

    private void Awake()
    {
        StartCoroutine(InitializeEvents());
        InitializeInstance();
    }

    private void Start()
    {
        _chatContentTransform = _chatContent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(IsInputFieldActive)
        {
            float scrollValue = Input.mouseScrollDelta.y;
            _chatScrollrect.verticalNormalizedPosition += scrollValue * _scrollbarSensitivity / _chatContentTransform.rect.height;
        }

        if (PlayerInput.IsChatPressed)
        {
            OpenChat();
        }
        if (PlayerInput.IsEscapePressed)
        {
            CloseChat();
        }
    }

    public void OpenChat()
    {
        // Stop hiding coroutine if chat has been opened
        if (_hideChatWindowCoroutine != null)
            StopCoroutine(_hideChatWindowCoroutine);

        SetActiveInputField(true);
        StartCoroutine(UpdateScrollValueCoroutine());
        IsInputFieldActive = true;
        _chatWindow.SetActive(true);
        _chatInputField.ActivateInputField();
        PlayerInput.IsButtonsBlocked = true;
    }

    public void CloseChat()
    {
        IsInputFieldActive = false;
        _chatInputField.DeactivateInputField();
        PlayerInput.IsButtonsBlocked = false;
        SetActiveInputField(false);

        // Hide chat coroutine
        if (_hideChatWindowCoroutine != null)
            StopCoroutine(_hideChatWindowCoroutine);
        _hideChatWindowCoroutine = HideChatWindowCoroutine(_hideChatInSeconds);
        StartCoroutine(_hideChatWindowCoroutine);
    }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private IEnumerator HideChatWindowCoroutine(float inSeconds)
    {
        yield return new WaitForSeconds(inSeconds);

        _chatWindow.SetActive(false);
    }

    public void SendChatMessage(string msg)
    {
        IsInputFieldActive = false;

        _chatInputField.DeactivateInputField();
        PlayerInput.IsButtonsBlocked = false;

        SetActiveInputField(false);

        if (Input.GetMouseButtonDown(1))
        {
            _chatInputField.DeactivateInputField();
            _chatInputField.text = string.Empty;
            return;
        }

        if (msg == string.Empty)
        {
            if (_hideChatWindowCoroutine != null)
                StopCoroutine(_hideChatWindowCoroutine);
            _hideChatWindowCoroutine = HideChatWindowCoroutine(_hideChatInSeconds);
            StartCoroutine(_hideChatWindowCoroutine);
            return;
        }

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
        messageText.text = $"<color=#4DD581>{from}</color>: {msg}";

        _chatWindow.SetActive(true);
        if(!IsInputFieldActive)
            StartCoroutine(UpdateScrollValueCoroutine());

        if (!IsInputFieldActive)
        {
            if (_hideChatWindowCoroutine != null)
                StopCoroutine(_hideChatWindowCoroutine);
            _hideChatWindowCoroutine = HideChatWindowCoroutine(_hideChatInSeconds);
            StartCoroutine(_hideChatWindowCoroutine);
        }

        CheckIfNeedToDeleteMessage();
    }

    private IEnumerator UpdateScrollValueCoroutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _chatScrollrect.verticalNormalizedPosition = 0;
    }

    private void CheckIfNeedToDeleteMessage()
    {
        if (_chatContent.transform.childCount > _messageBuffer)
        {
            Destroy(_chatContent.transform.GetChild(0).gameObject);
        }
    }

    private void SetActiveInputField(bool isActive)
    {
        _chatInputField.gameObject.SetActive(isActive);
    }

    [ServerCallback]
    private IEnumerator InitializeEvents()
    {
        while (PlayerListManager.Instance == null)
            yield return null;

        PlayerListManager.Instance.OnPlayerAddedToList += OnPlayerJoined;
        PlayerListManager.Instance.OnPlayerRemovedFromList += OnPlayerLeft;
    }

    private void OnPlayerJoined(Player player)
    {
        CmdSendChatMessage("<color=#C1C1C1>Server</color>", $"{player.Username} joined the server");
    }

    private void OnPlayerLeft(Player player)
    {
        CmdSendChatMessage("<color=#C1C1C1>Server</color>", $"{player.Username} left the server");
    }
}
