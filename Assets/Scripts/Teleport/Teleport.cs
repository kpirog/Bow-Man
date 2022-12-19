using Elympics;
using Medicine;
using UnityEngine;

namespace Teleport
{
    public class Teleport : ElympicsMonoBehaviour, IUpdatable
    {
        [SerializeField] private Transform exit;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private float timeToTeleport;
        [SerializeField] private Color teleportColor;

        private bool _started;
        private float _startTime;
        [Inject] private Collider2D Collider { get; }
        [Inject] private SpriteRenderer SpriteRenderer { get; }
        
        private readonly ElympicsFloat _teleportTimer = new();
        private bool Teleported =>_teleportTimer.Value <= 0f && _started;
        
        public void ElympicsUpdate()
        {
            RunTimer();
            
            var playerInTrigger = Physics2D.OverlapBox(Collider.bounds.center, Collider.bounds.size, 0f, layerMask);

            if (playerInTrigger)
            {
                var player = playerInTrigger.transform;
                
                if (!_started)
                {
                    StartTeleportingProcess();
                }
                else
                {
                    SetTeleportStateColor();
                }

                if (Teleported)
                {
                    TeleportPlayer(player);
                }
            }
            else
            {
                if (!_started) return;
                ResetValues();
            }
        }

        private void SetTeleportStateColor()
        {
            SpriteRenderer.color = Color.Lerp(SpriteRenderer.color, teleportColor,
                ((Time.time - _startTime) / timeToTeleport) * Elympics.TickDuration);
        }

        private void StartTeleportingProcess()
        {
            _started = true;
            _teleportTimer.Value = timeToTeleport;
            _startTime = Time.time;
        }

        private void ResetValues()
        {
            _teleportTimer.Value = 0f;
            _started = false;
            SpriteRenderer.color = Color.white;
        }

        private void TeleportPlayer(Transform player)
        {
            player.position = exit.position;
            ResetValues();
        }

        private void RunTimer()
        {
            if(_teleportTimer.Value <= 0f) return;
            _teleportTimer.Value -= Elympics.TickDuration;
        }
    }
}