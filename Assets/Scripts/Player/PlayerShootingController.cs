using System;
using Elympics;
using Medicine;
using Projectiles;
using UnityEngine;

namespace Player
{
    public class PlayerShootingController : ElympicsMonoBehaviour
    {
        [Inject] private PlayerAnimationHandler AnimationHandler { get; }

        private string _arrowPrefabPath;
        private float _drawingForce;
        private bool CanRelease => _drawingForce > 0f;

        private void Awake()
        {
            SetCurrentArrow(ArrowType.Push);
        }

        public void SetCurrentArrow(ArrowType type)
        {
            switch (type)
            {
                default:
                case ArrowType.Push:
                    _arrowPrefabPath = "Prefabs/Projectiles/Push Arrow";
                    break;
                case ArrowType.Ice:
                    _arrowPrefabPath = "Prefabs/Projectiles/Ice Arrow";
                    break;
                case ArrowType.Inverted:
                    _arrowPrefabPath = "Prefabs/Projectiles/Inverted Arrow";
                    break;
            }
        }

        public void ProcessShoot(bool input, Vector2 mousePosition)
        {
            if (input)
            {
                DrawBow();
            }
            else
            {
                if (!CanRelease) return;
                CreateArrow(mousePosition);
            }
        }

        private void DrawBow()
        {
            _drawingForce += Elympics.TickDuration;

            if (_drawingForce >= 1f)
            {
                _drawingForce = 1f;
            }

            AnimationHandler.SetShootAnimation();
        }

        private void CreateArrow(Vector2 mousePosition)
        {
            var arrow = ElympicsInstantiate(_arrowPrefabPath, ElympicsPlayer.All).GetComponent<Arrow>();

            arrow.Setup((mousePosition - (Vector2)transform.position).normalized, transform.position, _drawingForce,
                ElympicsBehaviour);

            ResetBow();
        }

        private void ResetBow()
        {
            AnimationHandler.StopShootAnimation();
            _drawingForce = 0f;
        }
    }
}