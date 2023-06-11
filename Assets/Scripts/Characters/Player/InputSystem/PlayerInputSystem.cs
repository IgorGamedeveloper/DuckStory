using Unity.VisualScripting;
using UnityEngine;

namespace IMG.Character
{
    public class PlayerInputSystem : MonoBehaviour
    {
        public InputSystem TargetInputSystem { get; private set; }


        public enum AllInputSystem
        {
            Keyboard,
            Handheld,
            Joystick
        }

        public AllInputSystem CurrentInputSystem { get; private set; } = AllInputSystem.Keyboard;

        [SerializeField] private bool _autodetectedInputSystem = true;



        private void Awake()
        {
            if (_autodetectedInputSystem == true)
            {
                CheckDeviceType();
            }

            ChangeInputSystemType(CurrentInputSystem);
        }

        private void CheckDeviceType()
        {
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                CurrentInputSystem = AllInputSystem.Keyboard;
            }
            else if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                CurrentInputSystem = AllInputSystem.Handheld;
            }
            else if (SystemInfo.deviceType == DeviceType.Console)
            {
                CurrentInputSystem = AllInputSystem.Joystick;
            }
            
            Debug.Log($"Текущий контроллер {CurrentInputSystem}");
        }

        public void ChangeInputSystemType(AllInputSystem currentInputSystem)
        {
            switch (currentInputSystem)
            {
                case AllInputSystem.Keyboard:
                case AllInputSystem.Handheld:
                case AllInputSystem.Joystick:
                default:
                    if (gameObject.TryGetComponent(out KeyboardInput keyboardInput) == false)
                    {
                        TargetInputSystem = gameObject.AddComponent<KeyboardInput>();
                    }
                    else
                    {
                        TargetInputSystem = keyboardInput;
                    }

                    break;
            }
        }
    }
}
