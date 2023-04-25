using System;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application._Scripts.Core.Enemies
{
    [Serializable]
    public class AnimatorController<TAnimationState> where TAnimationState : Enum
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private MyDictionary<TAnimationState, string> _animationTriggers;

        public void PlayAnimation(TAnimationState state)
        {
            string trigger = _animationTriggers[ state];
            _animator.ResetTrigger(trigger);
            _animator.SetTrigger(trigger);
        }
    }
}