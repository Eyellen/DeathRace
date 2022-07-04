using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        CursorManager.ShowCursor();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is closed");
    }
}
