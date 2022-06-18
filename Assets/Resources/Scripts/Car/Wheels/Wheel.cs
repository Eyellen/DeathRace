using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wheel : MonoBehaviour
{
    public Transform MeshTransform;
    public WheelCollider Collider;
    public Transform TireMarkPoint;

    #region Properties
    public bool ToggleUpdateWheels { get; set; } = true;

    public float ForwardSlip
    {
        get
        {
            WheelHit hitInfo;
            if (!Collider.GetGroundHit(out hitInfo)) return 0;
            return hitInfo.forwardSlip;
        }
    }

    public float SidewaysSlip
    {
        get
        {
            WheelHit hitInfo;
            if (!Collider.GetGroundHit(out hitInfo)) return 0;
            return hitInfo.sidewaysSlip;
        }
    }
    #endregion

    private void Update()
    {
        if (ToggleUpdateWheels) UpdateWheel();
        UpdateTrailMarkPoint();
    }

    private void UpdateWheel()
    {
        Vector3 position;
        Quaternion rotation;
        Collider.GetWorldPose(out position, out rotation);
        MeshTransform.position = position;
        MeshTransform.rotation = rotation;
    }

    private void UpdateTrailMarkPoint()
    {
        Vector3 position;
        Quaternion rotation;
        Collider.GetWorldPose(out position, out rotation);
        TireMarkPoint.position = position - (Vector3.up * (Collider.radius - 0.01f));
    }
}
