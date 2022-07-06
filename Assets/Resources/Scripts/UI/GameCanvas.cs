using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private EscapeMenu _escapeMenu;
    [SerializeField] private CarSelectUI _carSelectMenu;

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
        if (_carSelectMenu.gameObject.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            _carSelectMenu.SetActive(false);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _escapeMenu.SetActive(!_escapeMenu.gameObject.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            _carSelectMenu.SetActive(!_carSelectMenu.gameObject.activeSelf);
        }
    }
}
