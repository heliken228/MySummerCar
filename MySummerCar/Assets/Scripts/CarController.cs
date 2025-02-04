using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Transform _centerOfMass;
    
    [SerializeField] private Wheel[] _wheels;

    [SerializeField] private float _motorTorque = 500f;
    [SerializeField] private float _brakeForce = 50000f;
    [SerializeField] private float _handbrakeForce = 100000f;
    [SerializeField] private float _brakeInput;
    private bool _handbrake;
    
    private float _verticalInput;
    private float _horizontalInput;

    public float Speed;
    [SerializeField] private AnimationCurve _steeringCurve;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _handbrake = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _handbrake = false;
        }

        if (_handbrake)
        {
            Handbrake();
        }
        Brake();
        Move();
        Steering();
        CheckInput();
    }

    private void Move()
    {
        Speed = _rb.linearVelocity.magnitude;

    foreach (Wheel wheel in _wheels)
    {
        // Применяем крутящий момент только к задним колёсам
        if (!wheel.IsForwardWheel)
        {
            wheel.WheelCollider.motorTorque = _motorTorque * _verticalInput;
        }
        else
        {
            // Сбрасываем крутящий момент на передних колёсах
            wheel.WheelCollider.motorTorque = 0;
        }

        // Обновляем позицию и вращение колеса
        wheel.UpdateMeshPosition();
        }
    }

    private void CheckInput() //условия торможения
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
        float movingDirectional = Vector3.Dot(transform.forward, _rb.linearVelocity); //узнаём в какую сторону едет машина
        if ((movingDirectional < -0.5f && _verticalInput > 0) || (movingDirectional > 0.5f && _verticalInput < 0))
            _brakeInput = Mathf.Abs(_verticalInput);
        else
            _brakeInput = 0;
    }

    private void Brake()
    {
        if (_handbrake) return; // Не применяем обычный тормоз, если включён ручной тормоз

        foreach (Wheel wheel in _wheels)
        {
            if (wheel.IsForwardWheel)
                wheel.WheelCollider.brakeTorque = _brakeInput * _brakeForce * 0.7f;
            else
                wheel.WheelCollider.brakeTorque = _brakeInput * _brakeForce * 0.3f;
        }
    
    }
    
    private void Handbrake()
    {
            foreach (Wheel wheel in _wheels)
            {
                if (!wheel.IsForwardWheel)
                {
                    wheel.WheelCollider.brakeTorque = _handbrakeForce;
                    Debug.Log($"Applying handbrake to rear wheel: {wheel.WheelCollider.name}");
                }
                else
                    wheel.WheelCollider.brakeTorque = 0;
            }
            
    }

    private void Steering()
    {
        float steeringAngle = _horizontalInput * _steeringCurve.Evaluate(Speed);
        float slipAngle = Vector3.Angle(transform.forward, _rb.linearVelocity - transform.forward);

        if (slipAngle < 120)
            steeringAngle += Vector3.SignedAngle(transform.forward, _rb.linearVelocity, Vector3.up);
        
        steeringAngle = Mathf.Clamp(steeringAngle, -48, 48);
        
        foreach (Wheel wheel in _wheels)
        {
            if(wheel.IsForwardWheel)
                wheel.WheelCollider.steerAngle = steeringAngle;
        }
    }
    
}

[System.Serializable]
public struct Wheel
{
    public Transform WheelMesh;
    public WheelCollider WheelCollider;
    public bool IsForwardWheel;

    public void UpdateMeshPosition()
    {
        Vector3 pos;
        Quaternion rot;
        
        WheelCollider.GetWorldPose(out pos, out rot);
        
        WheelMesh.position = pos;
        WheelMesh.rotation = rot;
    }
}
