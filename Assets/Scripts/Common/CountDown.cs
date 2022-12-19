using Elympics;
using UnityEngine;

namespace Common
{
    public class CountDown : BaseTimer
    {
        public float RemainingTime => Mathf.FloorToInt(Timer.Value);
    }
}