using System;
using Common;
using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationHandler : ElympicsMonoBehaviour
    {
        [Inject] private Animator Animator { get; }
        [Inject] private SpriteRenderer SpriteRenderer { get; }

        private static readonly int MovementKey = Animator.StringToHash("IsMoving");
        private static readonly int JumpKey = Animator.StringToHash("Jump");
        private static readonly int FallDownKey = Animator.StringToHash("IsFallingDown");
        private static readonly int ShootKey = Animator.StringToHash("IsShooting");
        private static readonly int GetHitKey = Animator.StringToHash("GetHit");

        private ElympicsFloat SpriteDirection { get; } = new();

        private void Start()
        {
            SpriteDirection.ValueChanged += OnSpriteDirectionChanged;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGameStateChanged += SetAnimatorState;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameStateChanged -= SetAnimatorState;       
        }

        private void OnSpriteDirectionChanged(float oldValue, float newValue)
        {
            SpriteRenderer.flipX = newValue < 0f;
        }

        public void SetMovementAnimation(float axis)
        {
            var isMoving = axis != 0f;
            Animator.SetBool(MovementKey, isMoving);

            if (isMoving)
            {
                SpriteDirection.Value = axis;
            }
        }

        public void SetJumpAnimation()
        {
            Animator.SetTrigger(JumpKey);
        }

        public void SetFallDownAnimation(bool isFallingDown)
        {
            Animator.ResetTrigger(JumpKey);
            Animator.SetBool(FallDownKey, isFallingDown);
        }

        public void SetShootAnimation()
        {
            Animator.SetBool(ShootKey, true);
        }

        public void StopShootAnimation()
        {
            Animator.SetBool(ShootKey, false);
        }

        public void SetGetHitAnimation()
        {
            Animator.SetTrigger(GetHitKey);
        }

        private void SetAnimatorState(GameState state)
        {
            Animator.enabled = state != GameState.Finished;
        }
    }
}