using UnityEngine;

namespace Player
{
    public class PlayerInputProvider : MonoBehaviour
    {
        public float HorizontalAxis { get; private set; }

        private void Update()
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
        }
    }
}