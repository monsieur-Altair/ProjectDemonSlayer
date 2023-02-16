using System.Collections.Generic;
using UnityEngine;

namespace _Application.Scripts.Scriptables.Core.Enemies
{
    [CreateAssetMenu(fileName = "Base Data", menuName = "Resources/Base Data", order = 0)]
    public class BaseData : ScriptableObject
    {
        [SerializeField] private float _health = 20;
        [SerializeField] private float _motionsSpeed = 1;
        [SerializeField] private float _attackCooldown = 1;
        [SerializeField, NonReorderable] private List<DamageInfo> _attackInfo;
        [SerializeField, NonReorderable] private List<DamageInfo> _defenseInfo;

        public float Health => _health;
        public float MotionsSpeed => _motionsSpeed;
        public float AttackCooldown => _attackCooldown;
        public List<DamageInfo> AttackInfo => _attackInfo;
        public List<DamageInfo> DefenseInfo => _defenseInfo;
    }
}