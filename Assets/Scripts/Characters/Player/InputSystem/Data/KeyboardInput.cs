using UnityEngine;

namespace IMG.Character
{
    public class KeyboardInput : InputSystem
    {
        public override void GetInput()
        {
            _playerInput.x = Input.GetAxisRaw(RotationAxisName);

            _playerInput.z = Input.GetAxisRaw(MovementAxisName);

            _cameraAngle = Input.GetAxis(CameraRotateAxisName);
        }
    }
}
