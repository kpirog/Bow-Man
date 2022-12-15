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
        
        public void ExplosionPush(Vector2 hitPoint, float pushForce)
        {
            var characterPosition = (Vector2)transform.position;
            var distance = Vector2.Distance(hitPoint, characterPosition);
            
            Debug.Log($"Explosion distance = {distance}");
            
            var direction = hitPoint - characterPosition;
            direction = -direction.normalized;
            
            Rb.AddForce(direction * pushForce * distance * Elympics.TickDuration, ForceMode2D.Impulse);
            AnimationHandler.SetGetHitAnimation();
        }
    }
}
