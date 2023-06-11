using UnityEngine;
using System.Collections;
using IMG.Character;
using IMG.Grid;

public class InverseEnemyMovement : EnemyMovement
{
    private bool _playerStartMove = false;
    private bool _playerInMove;

    private float _moveTimeDelay;



    protected override void Subscribe()
    {
        MovementEventHandler.startMove += PlayerMoveStatus;
        MovementEventHandler.endMove += PlayerMoveStatus;
    }

    protected override void Unsubscribe()
    {
        MovementEventHandler.startMove -= PlayerMoveStatus;
        MovementEventHandler.endMove -= PlayerMoveStatus;
    }

    private void Update()
    {
        if (_playerStartMove == true)
        {
            InverseMove();
        }

        if (_moveTimeDelay > 0)
        {
            _moveTimeDelay -= Time.deltaTime;
        }
    }

    private void PlayerMoveStatus(bool inMove)
    {
        if (inMove == false)
        {
            _moveTimeDelay = _parameters.InverseMoveDelay;
        }

        _playerStartMove = true;
        _playerInMove = inMove;
    }

    private void InverseMove()
    {
        if (_playerInMove == false)
        {
            if (_inMove == false)
            {
                if (_moveTimeDelay <= 0)
                {
                    _moveTimeDelay = _parameters.InverseMoveDelay;
                    SelectDirection();
                }
            }
        }
    }

    protected override void StartMove()
    {
        _inMove = true;
    }

    protected override void EndMove()
    {
        _inMove = false;

    }

    protected override IEnumerator Movement(Vector3 direction)
    {
        UpdateMovementData();

        if (CheckWalkable(_targetPosition) == false)
        {
            yield break;
        }

        MovementGridHandler.UpdatePosition(SceneGrid.Instance, _startPosition, _targetPosition, _pathToTarget[0]);

        StartMove();

        _timeInMove = 0;

        while (_timeInMove < _parameters.MoveTime)
        {
            _rigidbody.MovePosition(Vector3.Lerp(_startPosition, _targetPosition, _timeInMove / _parameters.MoveTime));
            _timeInMove += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        FastEndMove();
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

        _targetRotation = _rigidbody.rotation * Quaternion.AngleAxis(currentRotateAngle, _parameters.RotateAxis);

        float timeLeft = 0;

        while (timeLeft < _parameters.RotateTime)
        {
            timeLeft += Time.deltaTime;

            _rigidbody.rotation = Quaternion.Slerp(startRotation, _targetRotation, timeLeft / _parameters.RotateTime);

            yield return new WaitForFixedUpdate();
        }

        EndMove();
    }
}
