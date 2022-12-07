using System.Linq;
using Elympics;
using UnityEngine;

namespace Projectiles
{
    public class TrajectoryPrediction : ElympicsMonoBehaviour, IInitializable
    {
        [SerializeField] private float spaceBetweenPoints;
        [SerializeField] private int amountOfPoints;

        private ElympicsList<ElympicsGameObject> _predictionPoints;
        private bool Visible => _predictionPoints.All(x => x.Value.gameObject.activeSelf);

        public void Initialize()
        {
            _predictionPoints = new ElympicsList<ElympicsGameObject>(() => new ElympicsGameObject());

            for (var i = 0; i < amountOfPoints; i++)
            {
                var point = ElympicsInstantiate("Prefabs/Trajectory/Prediction Point", ElympicsPlayer.All);
                _predictionPoints.Add().Value = point.GetComponent<ElympicsBehaviour>();
            }

            SetVisible(false);
        }

        public void DisplayTrajectory(float drawingForce, float speed, Vector2 direction)
        {
            if (!Visible)
            {
                SetVisible(true);
            }

            for (var i = 0; i < _predictionPoints.Count; i++)
            {
                _predictionPoints[i].Value.transform.position = PointPosition(i * drawingForce * spaceBetweenPoints,
                    drawingForce, speed, direction);
            }
        }

        public void SetVisible(bool visible)
        {
            foreach (var point in _predictionPoints)
            {
                point.Value.gameObject.SetActive(visible);
            }
        }

        private Vector2 PointPosition(float time, float drawingForce, float speed, Vector2 direction)
        {
            var force = speed * drawingForce;
            return (Vector2)transform.position + (direction * force * time) + 0.5f * Physics2D.gravity * (time * time);
        }
    }
}