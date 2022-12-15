using UnityEngine;

namespace Player.Effects
{
    [CreateAssetMenu(menuName = "Player Effects/Slow", fileName = "Slow Effect")]
    public class SlowEffect : PlayerEffect
    {
        [SerializeField] private float slowTime;
        [SerializeField][Range(0.01f, 1f)] private float slowMultiplier;
        
        public override void ApplyTo(PlayerController player, Vector2? sourcePosition)
        {
            player.MovementHandler.SetSlow(slowTime, slowMultiplier);
        }
    }
}
