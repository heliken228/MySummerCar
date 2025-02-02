using System;
using UnityEngine;

public class Wheels : MonoBehaviour
{
    public Transform WheelModel;
    public WheelCollider WheelCollider;

    public bool Steerable;
    public bool Motorized;
    
    private Vector3 _position;
    private Quaternion _rotation;

    private void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        WheelCollider.GetWorldPose(out _position, out _rotation);
        
        WheelModel.position = _position;
        WheelModel.rotation = _rotation;
    }
}
