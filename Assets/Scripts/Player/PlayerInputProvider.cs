using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerInputProvider : MonoBehaviour
    {
        public float HorizontalAxis { get; private set; }
        public bool Jump { get; private set; }
        public bool Fire { get; private set; }
        public bool Released { get; private set; }

        public float MousePositionX { get; private set; }
        public float MousePositionY { get; private set; }

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
            Jump = Input.GetKey(KeyCode.Space);
            Fire = Input.GetMouseButton(0);
            Released = Input.GetMouseButtonUp(0);

            if (!Fire) return;

            MousePositionX = GetMousePosition().x;
            MousePositionY = GetMousePosition().y;
        }

        public Vector3 GetMousePosition()
        {
            var screenPosition = Input.mousePosition;
            var worldPosition =
                _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y,
                    _mainCamera.nearClipPlane));

            return new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
        }
    }
}