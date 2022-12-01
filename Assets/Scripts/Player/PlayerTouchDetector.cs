using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    public class PlayerTouchDetector : ElympicsMonoBehaviour, IUpdatable
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float groundCheckHeight;
        [Inject] private Collider2D Collider { get; }

        private bool _isSlidingLeft, _isSlidingRight;
        public bool IsSliding => _isSlidingLeft || _isSlidingRight;
        public bool IsGrounded { get; private set; }

        public void ElympicsUpdate()
        {
            Vector2 position = transform.position;
            var bounds = Collider.bounds;
            var boxSize = new Vector2(bounds.size.x, groundCheckHeight);

            IsGrounded = Physics2D.OverlapBox(position + Vector2.down * bounds.extents.y, boxSize, 0f, groundLayer);

            _isSlidingLeft =
                Physics2D.OverlapCircle(position + (Vector2.left * Collider.bounds.extents.x), 0.01f, wallLayer);
            _isSlidingRight =
                Physics2D.OverlapCircle(position + (Vector2.right * Collider.bounds.extents.x), 0.01f, wallLayer);
        }
    }
}