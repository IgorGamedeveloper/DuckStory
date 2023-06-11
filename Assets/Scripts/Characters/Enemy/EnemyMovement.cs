using System.Collections;
using UnityEngine;
using IMG.Grid.Pathfinding;
using System.Collections.Generic;
using IMG.Grid;

namespace IMG.Character
{
    [RequireComponent(typeof(Pathfinder))]
    public class EnemyMovement : CharacterMovement
    {
        protected Pathfinder _pathfinding;

        protected Quaternion _targetRotation;

        //  _________________________________________________   œ¿–¿Ã≈“–€ œŒ»— ¿ œ”“»:

        protected GameObject _player;
        protected List<Vector3> _pathToTarget = new List<Vector3>();




        //  _________________________________________________________________   »Õ»÷»¿À»«¿÷»ﬂ » Œœ–≈ƒ≈À≈Õ»≈ ¬–≈Ã≈Õ» œŒƒœ»— » Õ¿ —Œ¡€“»ﬂ » Œ“œ»— » Œ“ —Œ¡€“»…:

        #region »Õ»÷»¿À»«¿÷»ﬂ, œŒƒœ»— ¿, Œ“œ»— ¿

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        protected virtual void Subscribe()
        {
            Unsubscribe();

            MovementEventHandler.startStep += SelectDirection;
            MovementEventHandler.endStep += EndMove;
        }

        protected virtual void Unsubscribe()
        {
            MovementEventHandler.startStep -= SelectDirection;
            MovementEventHandler.endStep -= EndMove;
        }

        private new void Awake()
        {
            base.Awake();

            _pathfinding = GetComponent<Pathfinder>();
            _player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        protected override void Start()
        {
            base.Start();
            Invoke(nameof(LateStart), 0.1f);
        }

        protected IEnumerator LateStart()
        {
            _startPosition = _rigidbody.position;
            MovementGridHandler.UpdatePosition(SceneGrid.Instance, _startPosition);
            yield break;
        }

        #endregion

        //  _________________________________________________________________   Õ¿’Œ∆ƒ≈Õ»≈ œ”“», ¬€¡Œ– ƒ≈…—“¬»ﬂ ¬–¿Ÿ≈Õ»ﬂ »À» ƒ¬»∆≈Õ»ﬂ:

        #region ¬€¡Œ– Õ¿œ–¿¬À≈Õ»ﬂ, ƒ¬»∆≈Õ»≈ » ¬–¿Ÿ≈Õ»≈

        //  _________________________________________________________________   Õ¿’Œ∆ƒ≈Õ»≈ œ”“» ƒŒ ÷≈À» » ¬€¡Œ– —À≈ƒ”ﬁŸ≈√Œ ƒ≈…—“¬»ﬂ:

        protected override void SelectDirection() 
        {
            _pathToTarget = _pathfinding.FindPath(transform.position, _player.transform.position);

            if (_pathToTarget != null && _pathToTarget.Count > 0)
            {
                _localDirection = transform.InverseTransformPoint(_pathToTarget[0]).normalized;
                _localDirection = new Vector3(Mathf.RoundToInt(_localDirection.x), Mathf.RoundToInt(_localDirection.y), Mathf.RoundToInt(_localDirection.z));
                Vector3 currentRotationNormilize = transform.InverseTransformDirection(transform.forward);

                if (currentRotationNormilize == _localDirection)
                {
                    MoveInDirection(_localDirection);
                }
                else if (_localDirection == Vector3.right)
                {
                    RotateToTheSide(true);
                }
                else if (_localDirection == Vector3.left)
                {
                    RotateToTheSide(false);
                }
                else if (_localDirection == Vector3.back)
                {
                    int randomSide = Random.Range(0, 2);

                    if (randomSide == 0)
                    {
                        RotateToTheSide(false);
                    }
                    else
                    {
                        RotateToTheSide(true);
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

        //  _________________________________________________________________   ƒ¬»∆≈Õ»≈:

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
        }

        //  _________________________________________________________________   ¬–¿Ÿ≈Õ»≈:

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
        }

        #endregion

        //  _________________________________________________________________   Œ“À¿ƒ ¿:

        private void OnDrawGizmosSelected()
        {
            if (_pathToTarget != null && _pathToTarget.Count > 0)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < _pathToTarget.Count; i++)
                {
                    Gizmos.DrawSphere(_pathToTarget[i], 2f);
                }

                Gizmos.color = Color.black;
                Gizmos.DrawCube(_pathToTarget[0], new Vector3(2f, 2f, 2f));
            }


            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_localDirection, 2f);
        }
    }
}
