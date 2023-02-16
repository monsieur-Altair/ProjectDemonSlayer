using _Application.Scripts.Managers;

namespace _Application._Scripts.Scriptables.Core.UnitsBehaviour
{
    public class Warrior : BaseUnit
    {
        private float _powerCoefficient;
        
        public override float PowerCoefficient => _powerCoefficient;

        protected override void FetchData(CoreConfig coreConfig)
        {
            base.FetchData(coreConfig);
            
            BaseUnitData = coreConfig.WarriorData;
        }

        public void Initialize(CoreConfig coreConfig, float powerCoefficient)
        {
            _powerCoefficient = powerCoefficient;

            Initialize(coreConfig);
        }
    }
}