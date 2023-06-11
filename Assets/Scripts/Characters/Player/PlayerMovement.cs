using System.Collections;
using UnityEngine;

namespace IMG.Character
{
    [RequireComponent(typeof(PlayerInputSystem))]
    public class PlayerMovement : CharacterMovement
    {
        private PlayerInputSystem _inputSystem;
        private InputSystem _input;


        protected override void Awake()
        {
            base.Awake();
            _inputSystem = GetComponent<PlayerInputSystem>();
        }

        protected override void Start()
        {
            base.Start();
            _input = _inputSystem.TargetInputSystem;
        }

        private void Update()
        {
            if (_inputSystem.CurrentInputSystem == PlayerInputSystem.AllInputSystem.Keyboard)
            {
                MovementInput();
                RotateInput();
            }
            else if (_inputSystem.CurrentInputSystem == PlayerInputSystem.AllInputSystem.Handheld)
            {
                HandheldMovement();
            }
        }

        #region Ввод с клавиатуры 
        private void MovementInput()
        {
            if (_input.PlayerInput.z != 0 && _inMove == false)
            {
                _localDirection = new Vector3(0, 0, _input.PlayerInput.z);
                MoveInDirection(_localDirection);
            }
        }

        private void RotateInput()
        {
            if (_input.PlayerInput.x != 0 && _inMove == false)
            {
                if (_input.PlayerInput.x > 0)
                {
                    RotateToTheSide(true);
                }
                else if (_input.PlayerInput.x < 0)
                {
                    RotateToTheSide(false);
                }
            }
        }
        #endregion

        #region Сенсорный ввод
        private void HandheldMovement()
        {
            if (_input.HoldForward == true)
            {
                MoveInDirection(Vector3.forward);
            }

            if (_input.HoldBack == true)
            {
                MoveInDirection(Vector3.back);
            }

            if (_input.HoldLeft == true)
            {
                RotateToTheSide(false);
            }

            if (_input.HoldRight == true)
            {
                RotateToTheSide(true);
            }
        }
        #endregion


        protected override void StartMove()
        {
            MovementEventHandler.InvokeStartMove();
            _inMove = true;
        }

        protected override void EndMove()
        {
            MovementEventHandler.InvokeEndMove();
            _inMove = false;
        }

        protected override IEnumerator Movement(Vector3 direction)
        {
            UpdateMovementData();

            if (CheckWalkable(_targetPosition) == false)
            {
                EndMove();
                yield break;
            }

            StartMove();

            float timeLeft = 0;

            while (timeLeft < _parameters.MoveTime)
            {
                _rigidbody.MovePosition(Vector3.Lerp(_startPosition, _targetPosition, timeLeft / _parameters.MoveTime));
                timeLeft += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.position = _targetPosition;
            EndMove();
        }

        protected override IEnumerator Rotation(bool isRight)
        {
            StartMove();

            Quaternion startRotation = _rigidbody.rotation;
            float currentRotateAngle = _parameters.RotateAngle;

            if (isRight == false)
            {
                currentRotateAngle *= -1f;
            }

            Quaternion targetRotation = _rigidbody.rotation * Quaternion.AngleAxis(currentRotateAngle, _parameters.RotateAxis);


            _timeInMove = 0;

            while (_timeInMove < _parameters.RotateTime)
            {
                _timeInMove += Time.deltaTime;

                _rigidbody.rotation = Quaternion.Slerp(startRotation, targetRotation, _timeInMove / _parameters.RotateTime);

                yield return new WaitForFixedUpdate();
            }

            EndMove();
        }
    }
}
