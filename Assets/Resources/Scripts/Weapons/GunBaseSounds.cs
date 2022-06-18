using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBaseSounds : MonoBehaviour
{
    [SerializeField] private GunBase _gunScript;

    [Header("Shooting sound")]
    [SerializeField] private AudioSource _shootSource;

    void Update()
    {
        ShootingSound(_gunScript.IsShooting);
    }

    private void ShootingSound(bool isShooting)
    {
        _shootSource.mute = !isShooting;
    }
}
