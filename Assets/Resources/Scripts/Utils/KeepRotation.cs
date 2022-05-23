using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour
{
    [SerializeField] private Quaternion _rotation;
    private Transform _thisTransform;

    void Start()
    {
        _thisTransform = GetComponent<Transform>();
    }


    void Update()
    {
        _thisTransform.rotation = _rotation;
    }
}
