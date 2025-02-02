using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float _motorTorque = 500f;
    [SerializeField] private float _breakForce = 300f;
    [SerializeField] private float _maxSteeringAngle = 45f;
    [SerializeField] private Transform _flTransform, _frTransform, _blTransform, _brTransform;

    [SerializeField] private WheelCollider _flWheel, _frWheel;
    [SerializeField] private WheelCollider _blWheel, _brWheel;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float motor = _motorTorque * Input.GetAxis("Vertical");
        float steering = _maxSteeringAngle * Input.GetAxis("Horizontal");
        
        _flWheel.steerAngle = steering;
        _frWheel.steerAngle = steering;
        
        _blWheel.motorTorque = motor;
        _brWheel.motorTorque = motor;

        if (Input.GetKey(KeyCode.Space))
        {
            _blWheel.brakeTorque = _breakForce;
            _brWheel.brakeTorque = _breakForce;
        }
        else
        {
            _blWheel.brakeTorque = 0f;
            _brWheel.brakeTorque = 0f;
        }
        UpdateWheelsVisualize(_flWheel, _flTransform);
        UpdateWheelsVisualize(_frWheel, _frTransform);
        UpdateWheelsVisualize(_blWheel, _blTransform);
        UpdateWheelsVisualize(_brWheel, _brTransform);
    }

    void UpdateWheelsVisualize(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }
}
