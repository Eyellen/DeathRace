using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class GameVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameVersionText;

#if UNITY_EDITOR
    void Update()
    {
        if (_gameVersionText.text != ("Version " + Application.version))
            _gameVersionText.text = "Version " + Application.version;
    }
#endif
}