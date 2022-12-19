using Elympics;
using Medicine;
using Player;
using UnityEngine;

namespace Jetpack
{
    public class Jetpack : ElympicsMonoBehaviour, IUpdatable
    {
        [SerializeField] private float force;
        [SerializeField] private float fuel;
        [SerializeField] private LayerMask layerMask;

        [Inject] private Collider2D Collider { get; }
        [Inject.FromChildren] private ParticleSystem Particles { get; }

        private readonly ElympicsBool _equipped = new();
        private readonly ElympicsBool _noFuel = new();
        
        private PlayerMovementHandler _connectedPlayer;

        private void OnEnable()
        {
            _equipped.ValueChanged += OnEquipped;
            _noFuel.ValueChanged += OnFuelEmpty;
        }

        private void OnDisable()
        {
            _equipped.ValueChanged -= OnEquipped;
            _noFuel.ValueChanged -= OnFuelEmpty;
        }

        public void ElympicsUpdate()
        {
            if (_noFuel.Value) return;

            if (!_equipped.Value)
            {
                var collider = Physics2D.OverlapBox(Collider.bounds.center, Collider.bounds.size, 0f, layerMask);
                
                if (!collider) return;
                _connectedPlayer = collider.gameObject.GetComponent<PlayerMovementHandler>();
                
                Equip();
            }
            else
            {
                if (fuel <= 0f)
                {
                    UnEquip(_connectedPlayer);
                }
                else
                {
                    fuel -= Elympics.TickDuration;
                    Particles.Play();
                }
            }
        }

        private void UnEquip(PlayerMovementHandler player)
        {
            player.OnJetpackEquipped?.Invoke(false, force);
            _noFuel.Value = true;
        }

        private void Equip()
        {
            _equipped.Value = true;
            _connectedPlayer.OnJetpackEquipped?.Invoke(true, force);
        }

        private void OnEquipped(bool oldValue, bool newValue)
        {
            transform.SetParent(_connectedPlayer.transform);
            transform.localPosition = Vector3.zero;
        }

        private void OnFuelEmpty(bool oldValue, bool newValue)
        {
            Particles.Stop();
            Destroy(gameObject);
        }
    }
}