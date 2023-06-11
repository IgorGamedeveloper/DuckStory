using UnityEngine;

namespace IMG.Character
{
    public class CharacterParameters : MonoBehaviour
    {
        [Header("Параметры движения:")]
        [Tooltip("Расстояние передвижения.")]
        [SerializeField] private float _moveDistance = 6.4f;
        public float MoveDistance { get { return _moveDistance; } }

        [Space(5f)]
        [Tooltip("Время передвижения.")]
        [SerializeField] private float _moveTime = 0.4f;
        public float MoveTime { get { return _moveTime; } }

        [Space(5f)]
        [Tooltip("Временная задержка между ходом при инверсии движения.")]
        [SerializeField] private float _inverseMoveDelay = 0.8f;
        public float InverseMoveDelay { get { return _inverseMoveDelay; } }


        [Space(15f)]
        [Header("Параметры вращения:")]
        [Tooltip("Ось вращения.")]
        [SerializeField] private Vector3 _rotateAxis = Vector3.up;
        public Vector3 RotateAxis { get { return _rotateAxis; } }

        [Space(5f)]
        [Tooltip("Угол на который происходит вращение персонажа.")]
        [SerializeField] private float _rotateAngle = 90f;
        public float RotateAngle { get { return _rotateAngle; } }

        [Space(5f)]
        [Tooltip("Время вращения.")]
        [SerializeField] private float _rotateTime = 0.3f;
        public float RotateTime { get { return _rotateTime; } }
    }
}
