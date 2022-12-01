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
        
        [SerializeField] private float wallSlidingMultiplier;
        [SerializeField] private float fallMultiplier;
        [SerializeField] private float lowJumpMultiplier;
        [Inject] private Rigidbody2D Rb { get; }
        [Inject] private PlayerTouchDetector TouchDetector { get; }

        private bool _canJump;

        public void Move(float axis)
        {
            Rb.AddForce(Vector2.right * axis * acceleration * Elympics.TickDuration, ForceMode2D.Force);
        }

        public void SetDrag(float axis)
        {
            if (TouchDetector.IsGrounded)
            {
                Rb.drag = axis == 0f && Rb.velocity != Vector2.zero ? drag : 0f;
            }
            else
            {
                Rb.drag = 0f;
            }
        }

        public void Jump(bool input)
        {
            if (input && (TouchDetector.IsGrounded || TouchDetector.IsSliding))
            {
                if (_canJump)
                {
                    Rb.AddForce(Vector2.up * jumpForce * Elympics.TickDuration, ForceMode2D.Impulse);
                    _canJump = false;
                }
            }

            if (!input)
            {
                _canJump = true;
            }
        }
        
        public void BetterJumpLogic(bool input)
        {
            if (TouchDetector.IsGrounded) return;

            if (IsFallingDown())
            {
                if (TouchDetector.IsSliding && input)
                {
                    Rb.velocity += Vector2.up * Physics2D.gravity.y * (wallSlidingMultiplier - 1) *
                                   Elympics.TickDuration;
                }
                else
                {
                    Rb.velocity +=
                        Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Elympics.TickDuration;
                }
            }
            else if (Rb.velocity.y > 0f && !input)
            {
                Rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Elympics.TickDuration;
            }
        }

        public void Slide()
        {
            if (TouchDetector.IsSliding)
            {
                Rb.velocity = new Vector2(Rb.velocity.x,
                    Mathf.Clamp(Rb.velocity.y, -wallSlidingMultiplier, float.MaxValue));
            }
        }
        private bool IsFallingDown()
        {
            return Rb.velocity.y < 0f;
        }
    }
}