using System.Collections.Generic;
using UnityEngine;

namespace _Application.Scripts.Scriptables.Core.Enemies
{
    [CreateAssetMenu(fileName = "Base Unit Data", menuName = "Resources/Base Unit Data", order = 0)]
    public class BaseUnitData : ScriptableObject
    {
        [SerializeField] private float _health;
        [SerializeField] private float _motionsSpeed;
        [SerializeField] private float _attackSpeed;
        [SerializeField, NonReorderable] private List<DamageInfo> _attackInfo;
        [SerializeField, NonReorderable] private List<DamageInfo> _defenseInfo;

        public float Health => _health;
        public float MotionsSpeed => _motionsSpeed;
        public float AttackSpeed => _attackSpeed;
        public List<DamageInfo> AttackInfo => _attackInfo;
        public List<DamageInfo> DefenseInfo => _defenseInfo;
    }
}