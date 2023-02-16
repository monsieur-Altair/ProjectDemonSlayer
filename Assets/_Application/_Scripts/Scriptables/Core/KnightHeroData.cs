using System.Collections.Generic;
using UnityEngine;

namespace _Application.Scripts.Scriptables.Core.Enemies
{
    [CreateAssetMenu(fileName = "Knight Hero Data", menuName = "Resources/Knight Hero Data", order = 0)]
    public class KnightHeroData : BaseUnitData
    {
        [Header("Skill info")]
        [SerializeField, NonReorderable] private List<DamageInfo> _skillAttackInfo;
        [SerializeField] private float _skillAttackRadius;
        [SerializeField] private float _skillCooldown;

        public float SkillCooldown => _skillCooldown;
        public List<DamageInfo> SkillAttackInfo => _skillAttackInfo;
        public float SkillAttackRadius => _skillAttackRadius;
    }
}