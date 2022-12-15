using UnityEngine;

namespace Player.Effects
{
    [CreateAssetMenu(menuName = "Player Effects/Explosion Push", fileName = "Explosion Push Effect")]
    public class ExplosionPushEffect : PlayerEffect
    {
        [SerializeField] private float force;
        public override void ApplyTo(PlayerController player, Vector2? sourcePosition)
        {
            player.CollisionHandler.ExplosionPush(sourcePosition ?? player.transform.position, force);
        }
    }
}
