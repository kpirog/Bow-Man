using Elympics;
using Medicine;
using UnityEngine;

namespace Platforms
{
    public class PlatformDurability : ElympicsMonoBehaviour
    {
        [SerializeField] private float durability;
        [SerializeField] private SpriteRenderer[] spriteRenderers;
        [SerializeField] private Color destroyedColor;
        [Inject] private Rigidbody2D Rb { get; }
        [Inject] private Collider2D Collider { get; }
        [Inject] private PlatformEffector2D PlatformEffector { get; }

        private LayerMask _untouchableLayer;

        private readonly ElympicsBool _destroyed = new();

        public void Start()
        {
            Rb.isKinematic = true;
            _untouchableLayer = LayerMask.NameToLayer("Untouchable");
            _destroyed.ValueChanged += SetPlatformDestroyed;
        }

        public void DecreaseDurability(float deltaTime)
        {
            if (durability > 0)
            {
                durability -= deltaTime;
                return;
            }

            _destroyed.Value = true;
        }

        private void SetPlatformDestroyed(bool oldValue, bool newValue)
        {
            gameObject.layer = _untouchableLayer;
            Rb.isKinematic = oldValue;
            Collider.usedByEffector = oldValue;
            PlatformEffector.enabled = oldValue;

            foreach (var sprite in spriteRenderers)
            {
                sprite.color = destroyedColor;
            }
        }
    }
}