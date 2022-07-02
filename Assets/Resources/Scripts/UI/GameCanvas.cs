using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _escapeMenu;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _escapeMenu.SetActive(!_escapeMenu.activeSelf);
        }
    }
}
