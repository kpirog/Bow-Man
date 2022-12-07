using UnityEngine;

namespace Player.Effects
{
    [CreateAssetMenu(menuName = "Player Effects/Freeze", fileName = "Freeze Effect")]
    public class FreezeEffect : PlayerEffect
    {
        [SerializeField] private float freezeTime;
        
        public override void ApplyTo(PlayerController player, Vector2? sourcePosition)
        {
            player.InputProvider.FreezeInputForSeconds(freezeTime);    
        }
    }
}
