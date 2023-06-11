using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IMG.Grid.Pathfinding;
using TMPro;

namespace IMG.Character
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(CharacterParameters))]
    public abstract class CharacterMovement : MonoBehaviour
    {
        //  _________________________________________________    ŒÃœŒÕ≈Õ“€ œ≈–—ŒÕ¿∆¿:

        protected Rigidbody _rigidbody;
        protected CapsuleCollider _collider;
        protected CharacterParameters _parameters;

        //  _________________________________________________   —“¿“”— ƒ¬»∆≈Õ»ﬂ œ≈–—ŒÕ¿∆¿:

        protected bool _inMove;

        //  _________________________________________________   œ¿–¿Ã≈“–€  ŒÀ¿…ƒ≈–¿:

        protected Vector3 _colliderCenter = new Vector3(0f, 1.1f, 0f);
        protected float _colliderRadius = 1.1f;
        protected float _colliderHeight = 5f;

        //  _________________________________________________   œ¿–¿Ã≈“–€ œ–Œ¬≈– » œ–Œ’Œƒ»ÃŒ—“»:

        protected Vector3 _boxCastSize = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("ÕÂÔÓıÓ‰ËÏ˚Â ÒÎÓË:")]
        [SerializeField] private LayerMask _obstacleLayer;

        //  _________________________________________________   Œ¡ÕŒ¬Àﬂ≈Ã€≈ œ¿–¿Ã≈“–€ ƒ¬»∆≈Õ»ﬂ:

        protected Vector3 _startPosition;
        protected Vector3 _targetPosition;
        protected Vector3 _localDirection;
        protected Vector3 _moveDirection;

        protected float _timeInMove;




        #region »Õ»÷»¿À»«¿÷»ﬂ

        //  #################################################   »Õ»÷»¿À»«¿÷»ﬂ

        //  _________________________________________________   »Õ»÷»¿À»«¿÷»ﬂ  ŒÃœŒÕ≈Õ“Œ¬ » Õ¿—“–Œ… ¿  ŒÀ¿…ƒ≈–¿:

        protected virtual void Awake()
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.freezeRotation = true;

            _collider = GetComponent<CapsuleCollider>();
            _collider.center = _colliderCenter;
            _collider.radius = _colliderRadius;
            _collider.height = _colliderHeight; 

            _parameters = GetComponent<CharacterParameters>();
        }

        protected virtual void Start()
        {
            _startPosition = _rigidbody.position;
            _targetPosition = _rigidbody.position;
        }

        #endregion

        protected virtual void SelectDirection() { }

        public bool CheckWalkable(Vector3 targetPosition)
        {
            return !Physics.CheckBox(targetPosition, _boxCastSize, Quaternion.identity, _obstacleLayer);
        }

        public void MoveInDirection(Vector3 direction)
        {
            if (_inMove == false)
            {
                StopAllCoroutines();
                StartCoroutine(Movement(direction));
            }
        }

        public void RotateToTheSide(bool isRight)
        {
            if (_inMove == false)
            {
                StopAllCoroutines();
                StartCoroutine(Rotation(isRight));
            }
        }

        protected void UpdateMovementData()
        {
            _startPosition = _rigidbody.position;
            _moveDirection = transform.TransformDirection(_localDirection * _parameters.MoveDistance);
            _targetPosition = _startPosition + _moveDirection;
        }

        protected abstract void StartMove();
        protected abstract void EndMove();

        protected abstract IEnumerator Movement(Vector3 direction);
        protected abstract IEnumerator Rotation(bool rightSide);

        protected void FastEndMove()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.position = _targetPosition;
        }
    }
}
