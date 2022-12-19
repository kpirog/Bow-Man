using Elympics;
using Medicine;
using Player;
using Player.Effects;
using UnityEngine;

namespace Obstacles
{
    public class Barrel : ElympicsMonoBehaviour, IUpdatable, IDamageable 
    {
        [SerializeField] private float explosionRange;
        [SerializeField] private LayerMask explosionLayer;
        
        [SerializeField] private PlayerEffect playerEffect;
        [SerializeField] private ParticleSystem particles;

        private readonly ElympicsBool _exploded = new();
        private readonly ElympicsFloat _destroyTime = new();
        
        [Inject] private Collider2D Collider { get; }
        [Inject] private SpriteRenderer SpriteRenderer { get; }
        [Inject] private Rigidbody2D Rb { get; }
        [Inject] private DestroyHandler DestroyHandler { get; }

        private void Awake()
        {
            _exploded.ValueChanged += OnExploded;
        }
        
        public void ElympicsUpdate()
        {
            if (_destroyTime.Value > 0f)
            {
                _destroyTime.Value -= Elympics.TickDuration;
                
                if (_destroyTime.Value <= 0f)
                {
                    DestroyHandler.destroyed.Value = true;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            Explode();
        }
        
        private void Explode()
        {
            var collider = Physics2D.OverlapCircle(transform.position, explosionRange, explosionLayer);

            if (collider != null)
            {
                var player = collider.transform.GetComponent<PlayerController>();

                if (player)
                {
                    playerEffect.ApplyTo(player, transform.position);
                }
            }

            _destroyTime.Value = particles.main.duration;
            _exploded.Value = true;
        }

        private void OnExploded(bool oldValue, bool newValue)
        {
            DisableComponents();
            particles.Play();
        }
        
        private void DisableComponents()
        {
            Collider.enabled = false;
            SpriteRenderer.enabled = false;
            Rb.simulated = false;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }
    }
}
