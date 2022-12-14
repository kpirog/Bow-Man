using System;
using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementHandler : ElympicsMonoBehaviour, IUpdatable
    {
        [SerializeField] private float acceleration;
        [SerializeField] private float jumpForce;
        [SerializeField] private float drag;
        [SerializeField] private float maxVelocity;

        [SerializeField] private float wallSlidingMultiplier;
        [SerializeField] private float fallMultiplier;
        [SerializeField] private float lowJumpMultiplier;

        [SerializeField] private Vector2 horizontalVelocityReduction;
        [SerializeField] private Vector2 verticalVelocityReduction;

        [Inject] private Rigidbody2D Rb { get; }
        [Inject] private PlayerTouchDetector TouchDetector { get; }
        [Inject] private PlayerAnimationHandler AnimationHandler { get; }

        private readonly ElympicsFloat _slowTimer = new();

        private bool IsGrounded => TouchDetector.IsGrounded || TouchDetector.IsSliding;
        private bool CanJump => (IsGrounded || _doubleJumpAvailable) && !_jumpLocked;

        private bool _doubleJumpAvailable;
        private bool _doubleJumpUsed;
        private bool _jumpLocked;

        private float _currentAcceleration;
        private float _jetpackMovementSpeed;
        private int _slowMultiplier;

        private readonly ElympicsBool _jetpackMovementAllowed = new();

        public Action<bool, float> OnJetpackEquipped;

        private void OnEnable()
        {
            OnJetpackEquipped += EnableJetpackMovement;
        }

        private void OnDisable()
        {
            OnJetpackEquipped -= EnableJetpackMovement;
        }

        public void ElympicsUpdate()
        {
            if (_slowTimer.Value > 0f)
            {
                _slowTimer.Value -= Elympics.TickDuration;
            }
            else
            {
                _currentAcceleration = acceleration;
            }
        }

        public void Move(float axis)
        {
            Rb.AddForce(Vector2.right * axis * _currentAcceleration * Elympics.TickDuration, ForceMode2D.Force);
            AnimationHandler.SetMovementAnimation(axis);
        }

        public void JetpackMove(bool input)
        {
            if (!_jetpackMovementAllowed.Value || !input) return;
            Rb.AddForce(Vector2.up * _jetpackMovementSpeed * Elympics.TickDuration, ForceMode2D.Impulse);
        }

        public void SetDrag(float axis)
        {
            if (TouchDetector.IsGrounded)
            {
                Rb.drag = axis == 0f && Rb.velocity != Vector2.zero ? drag : 0f;
                return;
            }

            Rb.drag = 0f;
        }

        #region Jump

        public void ProcessJump(bool jump)
        {
            if (jump)
            {
                if (CanJump)
                {
                    ApplyJump();
                }
            }
            else
            {
                switch (IsGrounded)
                {
                    case true:
                        _jumpLocked = false;
                        break;
                    case false when !_doubleJumpUsed:
                        _doubleJumpAvailable = true;
                        _jumpLocked = false;
                        break;
                }
            }

            BetterJumpLogic(jump);
        }

        private void ApplyJump()
        {
            if (_doubleJumpAvailable)
            {
                _doubleJumpAvailable = false;
                _doubleJumpUsed = true;
            }
            else
            {
                _doubleJumpUsed = false;
            }

            Rb.AddForce(Vector2.up * jumpForce * Elympics.TickDuration, ForceMode2D.Impulse);
            _jumpLocked = true;

            AnimationHandler.SetJumpAnimation();
        }

        private void BetterJumpLogic(bool jump)
        {
            if (TouchDetector.IsGrounded) return;

            if (IsFallingDown())
            {
                if (TouchDetector.IsSliding && jump)
                {
                    Rb.velocity += Vector2.up * Physics2D.gravity.y * (wallSlidingMultiplier - 1) *
                                   Elympics.TickDuration;
                }
                else
                {
                    Rb.velocity +=
                        Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Elympics.TickDuration;
                }
            }
            else if (Rb.velocity.y > 0f && !jump)
            {
                Rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Elympics.TickDuration;
            }
        }

        #endregion

        public void Slide()
        {
            if (TouchDetector.IsSliding)
            {
                Rb.velocity = new Vector2(Rb.velocity.x,
                    Mathf.Clamp(Rb.velocity.y, -wallSlidingMultiplier, float.MaxValue));
            }
        }

        public bool IsFallingDown()
        {
            return Rb.velocity.y < 0f;
        }

        public void LimitSpeed()
        {
            if (Mathf.Abs(Rb.velocity.x) > maxVelocity)
            {
                Rb.velocity *= horizontalVelocityReduction;
            }

            if (Mathf.Abs(Rb.velocity.y) > maxVelocity)
            {
                Rb.velocity *= verticalVelocityReduction;
            }
        }

        public void SetSlow(float slowTime, float slowMultiplier)
        {
            _slowTimer.Value = slowTime;
            _currentAcceleration = acceleration * slowMultiplier;
        }

        private void EnableJetpackMovement(bool enable, float speed)
        {
            _jetpackMovementSpeed = speed;
            _jetpackMovementAllowed.Value = enable;
        }
    }
}