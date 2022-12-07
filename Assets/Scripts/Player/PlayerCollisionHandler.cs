using Elympics;
using UnityEngine;
using Medicine;

namespace Player
{
    public class PlayerCollisionHandler : ElympicsMonoBehaviour
    {
        [Inject] private PlayerAnimationHandler AnimationHandler { get; }
        [Inject] private Rigidbody2D Rb { get; }
        
        
        public void Push(Vector2 hitPoint, float pushForce)
        {
            var direction = hitPoint - (Vector2)transform.position;
            direction = -direction.normalized;

            Rb.AddForce(direction * pushForce * Elympics.TickDuration, ForceMode2D.Impulse);
            AnimationHandler.SetGetHitAnimation();
        }
    }
}
