using UnityEngine;
using IMG.Character;

public class CameraMovementBehindTarget : MonoBehaviour
{
    private InputSystem _inputSystem;
    private Transform _currentTarget;

    [SerializeField] private float _rotationSpeed = 8f;
    [SerializeField] private float _upperAngle = 40f;
    [SerializeField] private float _distance = 25f;

    private float _horizontalRotation;



    private void Awake()
    {
        _currentTarget = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Start()
    {
        _inputSystem = FindObjectOfType<PlayerInputSystem>().TargetInputSystem;
    }

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
