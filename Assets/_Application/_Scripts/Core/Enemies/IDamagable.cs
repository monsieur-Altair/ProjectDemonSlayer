using System;
using System.Collections.Generic;
using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Core.Enemies
{
    public interface IDamagable
    {
        public event Action<IDamagable> Died;
        public event Action<IDamagable> Updated;
        public event Action<IDamagable> Damaged;

        public float CurrentHealth { get; }
        public float MaxHealth { get; }
        public Transform BarPoint { get; }
        List<DamageInfo> DefenceInfo { get; }
        Transform Transform { get; }
        public void TakeDamage(float damage);
        public void StartAttacking(IDamagable target);
    }
}