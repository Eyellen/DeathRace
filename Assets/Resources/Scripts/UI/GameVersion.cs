#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class GameVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameVersionText;

    void Update()
    {
        if (_gameVersionText.text != ("Version " + Application.version))
            _gameVersionText.text = "Version " + Application.version;
    }
}
#endif