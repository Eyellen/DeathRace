using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class GameChatUI : NetworkBehaviour
{
    [SerializeField] private GameObject _chatWindow;
    [SerializeField] private TMP_InputField _chatInputField;

    [SerializeField] private GameObject _chatContent;
    [SerializeField] private GameObject _chatMessagePrefab;

    [SerializeField] private float _hideChatInSeconds = 5f;

    private IEnumerator _hideChatWindowCoroutine;

    private void Update()
    {
        if(PlayerInput.IsChatPressed)
        {
            // Stop hiding coroutine if chat has been opened
            if (_hideChatWindowCoroutine != null)
                StopCoroutine(_hideChatWindowCoroutine);

            _chatWindow.SetActive(true);
            _chatInputField.ActivateInputField();
            PlayerInput.IsButtonsBlocked = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
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

        //
        _chatInputField.DeactivateInputField();
        PlayerInput.IsButtonsBlocked = false;
        //

        CmdSendChatMessage(Player.LocalPlayer.Username, msg);
        _chatInputField.text = string.Empty;
    }

    [Command(requiresAuthority = false)]
    private void CmdSendChatMessage(string fromPlayer, string msg)
    {
        RpcSendChatMessage(fromPlayer, msg);
    }

    [ClientRpc]
    private void RpcSendChatMessage(string fromPlayer, string msg)
    {
        GameObject message = Instantiate(_chatMessagePrefab, _chatContent.transform);

        TextMeshProUGUI messageText = message.GetComponent<TextMeshProUGUI>();
        messageText.text = $"<color=\"red\">{fromPlayer}</color>: {msg}";

        _chatWindow.SetActive(true);

        if (_hideChatWindowCoroutine != null)
            StopCoroutine(_hideChatWindowCoroutine);
        _hideChatWindowCoroutine = HideChatWindowCoroutine(_hideChatInSeconds);
        StartCoroutine(_hideChatWindowCoroutine);
    }
}
