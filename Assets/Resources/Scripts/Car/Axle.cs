using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Axle
{
    public AxleLocation AxleLocation;
    public bool IsDriveAxle;
    [Range(0, 1)] public float BrakesInfluence;
    public Wheel RightWheel;
    public Wheel LeftWheel;
}
