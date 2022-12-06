using Elympics;
using Medicine;
using Projectiles;
using UnityEngine;

namespace Player
{
    public class PlayerShootingController : ElympicsMonoBehaviour
    {
        [Inject] private PlayerAnimationHandler AnimationHandler { get; }

        private float _drawingForce;
        private bool CanRelease => _drawingForce > 0f;

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
            var arrow = ElympicsInstantiate("Prefabs/Projectiles/Arrow", ElympicsPlayer.All).GetComponent<Arrow>();
            
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