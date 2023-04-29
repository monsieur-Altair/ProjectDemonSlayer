using System;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application._Scripts.Core.Enemies
{
    [Serializable]
    public class AnimatorController
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _useAnim = false;
        
        // [SerializeField] private MyDictionary<TAnimationState, string> _animationTriggers;
        //
        // public void PlayAnimation(TAnimationState state)
        // {
        //     string trigger = _animationTriggers[ state];
        //     _animator.ResetTrigger(trigger);
        //     _animator.SetTrigger(trigger);
        // }
        
        public void PlayAnimation(int hash)
        {
            if(_useAnim == false)
                return;
            
            _animator.ResetTrigger(hash);
            _animator.SetTrigger(hash);
        }
    }
}