using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wheel : MonoBehaviour
{
    public Transform Transform;
    public WheelCollider Collider;

    private void Update()
    {
        UpdateWheel();
    }

    public void UpdateWheel()
    {
        Vector3 position;
        Quaternion rotation;
        Collider.GetWorldPose(out position, out rotation);
        Transform.position = position;
        Transform.rotation = rotation;
    }
}
