using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float _motorTorque = 3000f;
    [SerializeField] private float _breakForce = 50000f;
    [SerializeField] private float _maxSteeringAngle = 45f;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Transform _flTransform, _frTransform, _blTransform, _brTransform;

    [SerializeField] private WheelCollider _flWheel, _frWheel;
    [SerializeField] private WheelCollider _blWheel, _brWheel;
    private Rigidbody _rigidbody;

    public float Speed { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _centerOfMass.localPosition;
    }

    private void Update()
    {
        Speed = _rigidbody.linearVelocity.magnitude;
        
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
            _flWheel.brakeTorque = 0f;
            _frWheel.brakeTorque = 0f;
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
