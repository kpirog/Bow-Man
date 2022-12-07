using UnityEngine;

namespace Player.Effects
{
    public abstract class PlayerEffect : ScriptableObject
    {
        public abstract void ApplyTo(PlayerController player, Vector2? sourcePosition);
    }
}
