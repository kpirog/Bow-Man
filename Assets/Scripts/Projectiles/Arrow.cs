using System;
using Elympics;
using UnityEngine;
using Medicine;
using Player;
using Player.Effects;

namespace Projectiles
{
    public class Arrow : ElympicsMonoBehaviour, IUpdatable, IInitializable
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        [SerializeField] private PlayerEffect playerEffect;

        [SerializeField] private LayerMask collisionLayer;
        [Inject] private Rigidbody2D Rb { get; }
        [Inject] private SpriteRenderer SpriteRenderer { get; }

        private readonly ElympicsBool _firstAction = new();
        private readonly ElympicsBool _collisionActive = new();

        private readonly ElympicsVector2 _direction = new();
        private readonly ElympicsVector2 _startPosition = new();

        private readonly ElympicsFloat _lifeTimer = new();
        private readonly ElympicsFloat _drawForce = new();

        private readonly ElympicsGameObject _owner = new();

        private Color _startColor;

        private void Awake()
        {
            _startColor = SpriteRenderer.color;
        }

        public void Initialize()
        {
            _startPosition.ValueChanged += OnStartPositionUpdated;
            _lifeTimer.Value = lifeTime;

            SpriteRenderer.color = new Color(1f, 1f, 1f, 0f);
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
                SpriteRenderer.color = _startColor;
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

            var player = collision.transform.GetComponent<PlayerController>();

            if (player)
            {
                if (_owner.Value != player.ElympicsBehaviour)
                {
                    SetAfterCollision(player.transform, collision.point);
                    playerEffect.ApplyTo(player, collision.point);
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

    public enum ArrowType
    {
        Push, Ice, Inverted
    }
}