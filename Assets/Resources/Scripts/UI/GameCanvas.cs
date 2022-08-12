using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas Instance { get; private set; }

    [SerializeField] private EscapeMenu _escapeMenu;
    [SerializeField] private CarSelectUI _carSelectMenu;

    [SerializeField] private GameObject[] _objectsToHide;

    private void Start()
    {
        InitializeInstance();
        //CursorManager.HideCursor();
    }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

        if(Input.GetKeyDown(KeyCode.F1))
        {
            ToggleHUD();
        }
    }

    private void ToggleHUD()
    {
        foreach (var objectToHide in _objectsToHide)
        {
            objectToHide.SetActive(!objectToHide.activeSelf);
        }
    }

    public void SetActiveHUD(bool isActive)
    {
        foreach (var objectToHide in _objectsToHide)
        {
            objectToHide.SetActive(isActive);
        }
    }
}
