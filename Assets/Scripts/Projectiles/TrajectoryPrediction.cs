using System.Linq;
using UnityEngine;

namespace Projectiles
{
    public class TrajectoryPrediction : MonoBehaviour
    {
        [SerializeField] private GameObject predictionPointPrefab;
        [SerializeField] private float spaceBetweenPoints;
        [SerializeField] private int amountOfPoints;

        private GameObject[] _predictionPoints;
        private bool Visible => _predictionPoints.All(x => x.activeSelf);

        private void Awake()
        {
            _predictionPoints = new GameObject[amountOfPoints];

            for (var i = 0; i < amountOfPoints; i++)
            {
                var point = Instantiate(predictionPointPrefab, transform.position, Quaternion.identity);
                point.transform.SetParent(transform);
                _predictionPoints[i] = point;
            }

            SetVisible(false);
        }
        
        public void DisplayTrajectory(float drawingForce, float speed, Vector2 direction)
        {
            if (!Visible)
            {
                SetVisible(true);
            }

            for (var i = 0; i < _predictionPoints.Length; i++)
            {
                _predictionPoints[i].transform.position = PointPosition(i * drawingForce * spaceBetweenPoints,
                    drawingForce, speed, direction);
            }
        }

        public void SetVisible(bool visible)
        {
            foreach (var point in _predictionPoints)
            {
                point.gameObject.SetActive(visible);
                point.transform.position = transform.position;
            }
        }

        private Vector2 PointPosition(float time, float drawingForce, float speed, Vector2 direction)
        {
            var force = speed * drawingForce;
            return (Vector2)transform.position + (direction * force * time) + 0.5f * Physics2D.gravity * (time * time);
        }
    }
}