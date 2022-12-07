using UnityEngine;

namespace Player.Effects
{
    [CreateAssetMenu(menuName = "Player Effects/Push", fileName = "Push Effect")]
    public class PushEffect : PlayerEffect
    {
        [SerializeField] private float force;
        
        public override void ApplyTo(PlayerController player, Vector2? sourcePosition)
        {
            player.CollisionHandler.Push(sourcePosition ?? player.transform.position, force);
        }
    }
}
