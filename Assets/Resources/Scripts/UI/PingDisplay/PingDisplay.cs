using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PingDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pingText;

    private void Update()
    {
        _pingText.text = (Math.Round(NetworkTime.rtt * 1000)).ToString();
    }
}
