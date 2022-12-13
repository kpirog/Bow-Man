using Elympics;
using Medicine;
using UnityEngine;

namespace Platforms
{
    public class PlatformTrigger : ElympicsMonoBehaviour, IUpdatable
    {
        [SerializeField] private LayerMask collisionLayer;
        [Inject] private Collider2D Collider { get; }
        [Inject] private PlatformDurability PlatformDurability { get; }
        
        public void ElympicsUpdate()
        {
            var characterCollider = Physics2D.OverlapBox(Collider.bounds.center, Collider.bounds.size, 0f, collisionLayer);

            if (characterCollider)
            {
                PlatformDurability.DecreaseDurability(Elympics.TickDuration);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube((Vector2)Collider.bounds.center, Collider.bounds.size);
        }
    }
}
