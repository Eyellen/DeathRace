using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private EscapeMenu _escapeMenu;

    private void Start()
    {
        //CursorManager.HideCursor();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _escapeMenu.SetActive(!_escapeMenu.gameObject.activeSelf);
        }
    }
}
