using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunSounds : MonoBehaviour
{
    private enum ClipStates
    {
        None,
        Start,
        Loop,
        End
    }

    [SerializeField] private Minigun _minigun;

    [Header("Barrels spinning sounds")]
    [SerializeField] private AudioSource _barrelSpinSource;
    private ClipStates _spinState;
    [SerializeField] private AudioClip _barrelsStartSpin;
    [SerializeField] private AudioClip _barrelsSpinLoop;
    [SerializeField] private AudioClip _barrelsStopSpin;

    [Header("Shooting sounds")]
    [SerializeField] private AudioSource _minigunShootSource;
    private ClipStates _shootState;
    [SerializeField] private AudioClip _startShoot;
    [SerializeField] private AudioClip _shootLoop;
    [SerializeField] private AudioClip _stopShoot;

    void Start()
    {
        _spinState = ClipStates.None;
        _shootState = ClipStates.None;
    }


    void Update()
    {
        BarrelSpinningSound(_minigun.IsSpinning);
        ShootingSound(_minigun.IsShooting);
    }

    private void BarrelSpinningSound(bool isSpinning)
    {
        switch (_spinState)
        {
            case ClipStates.None:
                {
                    if (isSpinning)
                    {
                        _spinState = ClipStates.Start;
                    }
                    break;
                }
            case ClipStates.Start:
                {
                    if (_barrelSpinSource.clip != _barrelsStartSpin)
                    {
                        _barrelSpinSource.loop = false;
                        _barrelSpinSource.clip = _barrelsStartSpin;
                        _barrelSpinSource.Play();
                    }
                    if (!_barrelSpinSource.isPlaying)
                    {
                        _spinState = ClipStates.Loop;
                    }
                    if(!isSpinning)
                    {
                        _spinState = ClipStates.End;
                    }
                    break;
                }
            case ClipStates.Loop:
                {
                    if (_barrelSpinSource.clip != _barrelsSpinLoop)
                    {
                        _barrelSpinSource.loop = true;
                        _barrelSpinSource.clip = _barrelsSpinLoop;
                        _barrelSpinSource.Play();
                    }
                    if(!isSpinning)
                    {
                        _spinState = ClipStates.End;
                    }
                    break;
                }
            case ClipStates.End:
                {
                    if (_barrelSpinSource.clip != _barrelsStopSpin)
                    {
                        _barrelSpinSource.loop = false;
                        _barrelSpinSource.clip = _barrelsStopSpin;
                        _barrelSpinSource.Play();
                    }
                    if(!_barrelSpinSource.isPlaying)
                    {
                        _spinState = ClipStates.None;
                    }
                    if (isSpinning)
                    {
                        _spinState = ClipStates.Start;
                    }
                    break;
                }
        }
    }

    private void ShootingSound(bool isShooting)
    {
        _minigunShootSource.mute = !isShooting;
    }
}
