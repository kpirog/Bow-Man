using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementHandler : ElympicsMonoBehaviour
    {
        [SerializeField] private float acceleration;
        [SerializeField] private float drag;
        [Inject] private Rigidbody2D Rb { get; set; }

        public void Move(float axis)
        {
            Rb.AddForce(Vector2.right * axis * acceleration * Elympics.TickDuration, ForceMode2D.Force);
        }

        public void SetDrag(float axis)
        {
            if (axis == 0f && Rb.velocity != Vector2.zero)
            {
                Rb.drag = drag;
            }
            else
            {
                Rb.drag = 0f;
            }
        }
    }
}
