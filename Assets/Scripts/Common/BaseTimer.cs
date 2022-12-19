using Elympics;
using UnityEngine;

namespace Common
{
    public class BaseTimer : ElympicsMonoBehaviour, IInitializable
    {
        [SerializeField] private float time;

        protected readonly ElympicsFloat Timer = new();

        public bool Finished => Timer.Value <= 0f;

        public void Initialize()
        {
            Restart();
        }

        private void Restart()
        {
            Timer.Value = time;
        }

        public void Count()
        {
            if (Timer.Value <= 0f) return;
            Timer.Value -= Elympics.TickDuration;
        }
    }
}
