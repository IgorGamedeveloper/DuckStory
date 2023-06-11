using UnityEngine;

namespace IMG.Character
{
    public abstract class InputSystem : MonoBehaviour
    {
        public string MovementAxisName { get; private set; } = "Vertical";
        public string RotationAxisName { get; private set; } = "Horizontal";

        public string CameraRotateAxisName { get; private set; } = "Rotary";


        protected Vector3 _playerInput;
        public Vector3 PlayerInput { get { return _playerInput; } }

        protected float _cameraAngle;
        public float CameraAngle { get { return _cameraAngle; } }


        protected bool _holdForward = false;
        protected bool _holdBack = false;
        protected bool _holdLeft = false;
        protected bool _holdRight = false;

        public bool HoldForward { get { return _holdForward; } }
        public bool HoldBack { get { return _holdBack; } }
        public bool HoldLeft { get { return _holdLeft; } }
        public bool HoldRight { get { return _holdRight; } }



        private void Update()
        {
            GetInput();
        }

        public abstract void GetInput();
    }
}
