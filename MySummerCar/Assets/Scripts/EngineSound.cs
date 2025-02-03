using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CarMovement))]
public class EngineSound : MonoBehaviour
{
   private AudioSource _engineSound;
   private CarMovement _carMovement;

   [SerializeField] private float _speedRatio; //чем больше значение - тем больше скоростной промежуток между передачами
   [SerializeField] private int _maxGear;

   private float _minSpeed = 0; // минимальная скорость, при которой будет переключение передачи
   private float _maxSpeed = 0;
   private float _currentGear = 0;
   
   private void Start()
   {
      _engineSound = GetComponent<AudioSource>();
      _carMovement = GetComponent<CarMovement>();

      if (_speedRatio <= 0)
         _speedRatio = 4;
      
      if (_maxGear <= 0)
         _maxGear = 5;
      
   }

   private void Update()
   {
      SwitchGear();
   }

   private void SwitchGear()
   {
      if (_carMovement.Speed >= _maxSpeed)
      {
         if (_currentGear < _maxGear)
         {
            _currentGear++;
            _minSpeed = _maxSpeed;
            
            _maxSpeed += GetMaxSpeedValue();
         }
      }
      else if (_carMovement.Speed < _minSpeed)
      {
         _maxSpeed -= GetMaxSpeedValue();
         _currentGear--;
         
         _minSpeed -= GetMaxSpeedValue();
      }
      
      _engineSound.pitch = 1 + _carMovement.Speed / _maxSpeed;
   }
   
   

   private float GetMaxSpeedValue() => _speedRatio * _currentGear;
}

