using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Axel
{
    public AxelLocation AxelLocation;
    public bool IsDriveWheel;
    [Range(0, 1)] public float BrakesInfluence;
    public Wheel RightWheel;
    public Wheel LeftWheel;
}
