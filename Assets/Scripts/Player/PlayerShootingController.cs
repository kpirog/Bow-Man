using Elympics;
using Medicine;
using Projectiles;
using UnityEngine;

namespace Player
{
    public class PlayerShootingController : ElympicsMonoBehaviour
    {
        [SerializeField] private ElympicsInt pushArrowsCount;
        [SerializeField] private ElympicsInt iceArrowsCount;
        [SerializeField] private ElympicsInt invertedArrowsCount;
        [Inject] private PlayerAnimationHandler AnimationHandler { get; }

        private ElympicsInt _arrowsCount = new();
        
        private ArrowType _currentArrowType;
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
                    _arrowsCount = pushArrowsCount;
                    break;
                case ArrowType.Ice:
                    _arrowPrefabPath = "Prefabs/Projectiles/Ice Arrow";
                    _arrowsCount = iceArrowsCount;
                    break;
                case ArrowType.Inverted:
                    _arrowPrefabPath = "Prefabs/Projectiles/Inverted Arrow";
                    _arrowsCount = invertedArrowsCount;
                    break;
            }

            _currentArrowType = type;
        }

        public void ProcessShoot(bool input, Vector2 mousePosition)
        {
            if (_arrowsCount.Value == 0) return;
            
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

            DecreaseArrowsCount();

            ResetBow();
        }

        private void DecreaseArrowsCount()
        {
            _arrowsCount.Value--;

            switch (_currentArrowType)
            {
                default:
                case ArrowType.Push:
                    pushArrowsCount.Value = _arrowsCount;
                    break;
                case ArrowType.Ice:
                    iceArrowsCount.Value = _arrowsCount;
                    break;
                case ArrowType.Inverted:
                    invertedArrowsCount.Value = _arrowsCount;
                    break;
            }
        }

        private void ResetBow()
        {
            AnimationHandler.StopShootAnimation();
            _drawingForce = 0f;
        }
    }
}