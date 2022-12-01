using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    public class PlayerTouchDetector : ElympicsMonoBehaviour, IUpdatable
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckHeight;
        [Inject] private Collider2D Collider { get; }

        public bool IsGrounded { get; private set; }


        public void ElympicsUpdate()
        {
            Vector2 position = transform.position;
            var bounds = Collider.bounds;
            var boxSize = new Vector2(bounds.size.x, groundCheckHeight);

            IsGrounded =
                Physics2D.OverlapBox(position + Vector2.down * bounds.extents.y, boxSize, 0f, groundLayer);
        }
    }
}