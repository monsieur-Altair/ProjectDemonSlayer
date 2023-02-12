using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class WarriorTower : BaseTower
    {
        [SerializeField] private int _warriorAmount = 3;


        private List<BaseUnit> _warriors = new();

        protected override bool CanAttack => CanAttackNow();
        
        private bool CanAttackNow()
        {
            return _warriors.Count != 0 
                   && _warriors.Any(IsAvailable);
        }

        protected override void Update()
        {
            if (_isEnabled == false)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _baseTowerData.AttackCooldown && CanAttack)
            {
                BaseEnemy target = CoreMethods.FindClosest(_enemyTracker.Enemies, _baseTowerData.Radius, transform);
                if (target != null && target.BehaviourType != EnemyBehaviourType.Runner)
                {
                    _elapsedTime = 0f;
                    Attack(target);
                }
            }
        }

        private static bool IsAvailable(BaseUnit warrior) => 
            warrior.IsAlive && warrior.IsBusy == false;

        protected override void Attack(BaseEnemy target)
        {
            base.Attack(target);

            BaseUnit availableUnit = _warriors.Find(IsAvailable);
            availableUnit.SetTarget(target);
            availableUnit.GoToTarget();
        }
    }
}