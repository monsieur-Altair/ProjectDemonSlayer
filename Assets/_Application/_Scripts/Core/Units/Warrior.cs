using System.Collections.Generic;
using _Application.Scripts.Managers;
using _Application.Scripts.Scriptables.Core.Enemies;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class Warrior : BaseUnit
    {
        private float _powerCoefficient;
        private float _healthCoefficient;
        private List<DamageInfo> _attackInfo;

        public override float PowerCoefficient => _powerCoefficient;
        public override float MaxHealth => BaseUnitData.Health * _healthCoefficient;
        public override List<DamageInfo> AttackInfo => _attackInfo;

        protected override void FetchData(CoreConfig coreConfig)
        {
            base.FetchData(coreConfig);
            
            BaseUnitData = coreConfig.WarriorData;
        }

        public void Initialize(CoreConfig coreConfig, float powerCoefficient, float healthCoefficient, 
            List<DamageInfo> attackInfo)
        {
            _attackInfo = attackInfo;
            _healthCoefficient = healthCoefficient;
            _powerCoefficient = powerCoefficient;

            Initialize(coreConfig);
        }
    }
}