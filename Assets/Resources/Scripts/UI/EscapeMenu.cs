using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    private void OnEnable()
    {
        CursorManager.ShowCursor();
    }

    private void OnDisable()
    {
        CursorManager.HideCursor();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
