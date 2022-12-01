using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementHandler : ElympicsMonoBehaviour
    {
        [SerializeField] private float acceleration;
        [SerializeField] private float jumpForce;
        [SerializeField] private float drag;
        [Inject] private Rigidbody2D Rb { get; }

        [Inject] private PlayerTouchDetector TouchDetector { get; }

        public void Move(float axis)
        {
            Rb.AddForce(Vector2.right * axis * acceleration * Elympics.TickDuration, ForceMode2D.Force);
        }

        public void SetDrag(float axis)
        {
            Rb.drag = axis == 0f && Rb.velocity != Vector2.zero ? drag : 0f;
        }

        public void Jump()
        {
            if (!TouchDetector.IsGrounded) return;
            Rb.AddForce(Vector2.up * jumpForce * Elympics.TickDuration, ForceMode2D.Impulse);
        }
    }
}