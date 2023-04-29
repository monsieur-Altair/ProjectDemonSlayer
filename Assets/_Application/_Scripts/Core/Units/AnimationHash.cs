using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public static class AnimationHash
    {
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Run = Animator.StringToHash("Run");
        public static readonly int Idle = Animator.StringToHash("Idle");        
        public static readonly int Die = Animator.StringToHash("Die");
        public static readonly int Revive = Animator.StringToHash("Revive");
        public static readonly int Ultimate = Animator.StringToHash("Ultimate");
    }
}