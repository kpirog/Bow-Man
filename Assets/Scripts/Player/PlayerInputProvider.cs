using Elympics;
using UnityEngine;

namespace Player
{
    public class PlayerInputProvider : ElympicsMonoBehaviour, IUpdatable
    {
        public float HorizontalAxis { get; private set; }
        public float MousePositionX { get; private set; }
        public float MousePositionY { get; private set; }
        public bool Jump { get; private set; }
        public bool Fire { get; private set; }
        public bool StandardArrow { get; private set; }
        public bool IceArrow { get; private set; }
        public bool InvertedArrow { get; private set; }

        private readonly ElympicsFloat _freezeTimer = new();
        private readonly ElympicsFloat _inversionTimer = new();

        private bool _inputFrozen;
        private bool _inputInverted;
        
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_inputFrozen) return;

            HorizontalAxis = !_inputInverted ? Input.GetAxis("Horizontal") : -Input.GetAxis("Horizontal");
            Jump = Input.GetKey(KeyCode.Space);
            Fire = Input.GetMouseButton(0);

            StandardArrow = Input.GetKey(KeyCode.Alpha1);
            IceArrow = Input.GetKey(KeyCode.Alpha2);
            InvertedArrow = Input.GetKey(KeyCode.Alpha3);

            if (!Fire) return;
            MousePositionX = GetMousePosition().x;
            MousePositionY = GetMousePosition().y;
        }

        public void ElympicsUpdate()
        {
            if (_inputFrozen)
            {
                _freezeTimer.Value -= Elympics.TickDuration;
                CheckTimer(_freezeTimer.Value, ref _inputFrozen);
            }
            
            if (_inputInverted)
            {
                _inversionTimer.Value -= Elympics.TickDuration;
                CheckTimer(_inversionTimer.Value, ref _inputInverted);
            }
        }

        private void CheckTimer(float timerValue, ref bool running)
        {
            if (timerValue <= 0f)
            {
                running = false;
            }
        }
        
        private Vector3 GetMousePosition()
        {
            var screenPosition = Input.mousePosition;
            var worldPosition =
                _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y,
                    _mainCamera.nearClipPlane));

            return new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
        }

        public void FreezeInputForSeconds(float time)
        {
            _freezeTimer.Value = time;
            _inputFrozen = true;
        }

        public void InvertControlForSeconds(float time)
        {
            _inversionTimer.Value = time;
            _inputInverted = true;
        }
    }
}