using Elympics;
using UnityEngine;
using Medicine;

namespace Projectiles
{
    public class Arrow : ElympicsMonoBehaviour, IUpdatable, IInitializable
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;

        [SerializeField] private LayerMask collisionLayer;
        [Inject] private Rigidbody2D Rb { get; }

        private readonly ElympicsBool _firstAction = new();
        private readonly ElympicsBool _collisionActive = new();

        private readonly ElympicsVector2 _direction = new();
        private readonly ElympicsVector2 _startPosition = new();

        private readonly ElympicsFloat _lifeTimer = new();
        private readonly ElympicsFloat _drawForce = new();

        private readonly ElympicsGameObject _owner = new();

        public void Initialize()
        {
            _startPosition.ValueChanged += OnStartPositionUpdated;
            _lifeTimer.Value = lifeTime;
        }

        public void Setup(Vector2 direction, Vector2 startPosition, float drawForce, ElympicsBehaviour owner)
        {
            _direction.Value = direction;
            _drawForce.Value = drawForce;
            _owner.Value = owner;
            _startPosition.Value = startPosition;
        }

        private void OnStartPositionUpdated(Vector2 oldPosition, Vector2 newPosition)
        {
            transform.position = newPosition;

            if (!_collisionActive.Value)
            {
                _firstAction.Value = true;
            }
        }

        private void Update()
        {
            if (Rb.velocity == Vector2.zero) return;
            transform.right = Rb.velocity;
        }

        public void ElympicsUpdate()
        {
            if (_firstAction)
            {
                Release();
                return;
            }

            _lifeTimer.Value -= Elympics.TickDuration;

            if (_lifeTimer.Value <= 0f)
            {
                ElympicsDestroy(gameObject);
            }

            if (_collisionActive)
            {
                CheckCollision();
            }
        }

        private void Release()
        {
            Rb.AddForce(_direction.Value * (_drawForce.Value * speed * Elympics.TickDuration), ForceMode2D.Impulse);
            _firstAction.Value = false;
            _collisionActive.Value = true;
        }

        private void CheckCollision()
        {
            var collision = Physics2D.Raycast(transform.position, transform.right,
                Rb.velocity.magnitude * Elympics.TickDuration, collisionLayer);

            if (!collision) return;

            var player = collision.transform.GetComponent<ElympicsBehaviour>();

            if (player)
            {
                if (_owner.Value != player)
                {
                    SetAfterCollision(player.transform, collision.point);
                }
            }
            else
            {
                SetAfterCollision(null, collision.point);
            }
        }

        private void SetAfterCollision(Transform parent, Vector2 hitPoint)
        {
            Rb.simulated = false;
            transform.localRotation = Quaternion.identity;
            transform.position = hitPoint;
            transform.SetParent(parent);
            _collisionActive.Value = false;
        }
    }
}