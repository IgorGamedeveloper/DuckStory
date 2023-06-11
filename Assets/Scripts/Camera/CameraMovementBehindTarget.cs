using UnityEngine;
using IMG.Character;

public class CameraMovementBehindTarget : MonoBehaviour
{
    //  _________________________________________________________________   КОМПОНЕНТЫ КАМЕРЫ:
    private InputSystem _inputSystem;
    private Transform _currentTarget;


    //  _________________________________________________________________   ПАРАМЕТРЫ КАМЕРЫ:

    [Header("Параметры камеры:")]
    
    [Space(15f)]
    [Header("Скорость вращения камеры:")]
    [SerializeField] private float _rotationSpeed = 8f;
    
    [Space(10f)]
    [Header("Угол наклона камеры.")]
    [SerializeField] private float _upperAngle = 40f;
    
    [Space(10f)]
    [Header("Растояние от камеры до цели.")]
    [SerializeField] private float _distance = 25f;

    private float _horizontalRotation;




    //  ###################################################################   ИНИЦИАЛИЗАЦИЯ:

    private void Awake()
    {
        _currentTarget = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Start()
    {
        _inputSystem = FindObjectOfType<PlayerInputSystem>().TargetInputSystem;
    }

    //  _________________________________________________________________   ПЕРЕМЕЩЕНИЕ КАМЕРЫ:

    void LateUpdate()
    {
        if (_currentTarget != null)
        {
            _horizontalRotation += -_inputSystem.CameraAngle * _rotationSpeed * _distance * Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(_upperAngle, _horizontalRotation, 0f);

            Vector3 negativeDistance = new Vector3(0f, 0f, -_distance);

            Vector3 targetPosition = targetRotation * negativeDistance + _currentTarget.position;

            transform.rotation = targetRotation;
            transform.position = targetPosition;

            transform.position = _currentTarget.position - transform.forward * _distance;
        }
    }
}
