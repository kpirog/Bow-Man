using UnityEngine;

namespace Player.Effects
{
    [CreateAssetMenu(menuName = "Player Effects/Inversion", fileName = "Inversion Effect")]
    public class InversionEffect : PlayerEffect
    {
        [SerializeField] private float inversionTime;
        
        public override void ApplyTo(PlayerController player, Vector2? sourcePosition)
        {
            player.InputProvider.InvertControlForSeconds(inversionTime);
        }
    }
}