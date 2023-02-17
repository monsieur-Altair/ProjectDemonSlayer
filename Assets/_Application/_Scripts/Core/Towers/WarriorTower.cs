using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Enemies;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application._Scripts.Scriptables.Core.UnitsBehaviour;
using _Application.Scripts.Managers;
using _Application.Scripts.Misc;
using Pool_And_Particles;
using UnityEngine;

namespace _Application._Scripts.Core.Towers
{
    public class WarriorTower : BaseTower
    {
        public event Action<IDamagable> WarriorAdded = delegate {  };
        
        [SerializeField] private Transform _unitSpawnPoint;


        private readonly List<BaseUnit> _warriors = new();
        private Warrior _warriorPrefab;
        private WarriorTowerData _warriorTowerData;

        public List<IDamagable> Damagables { get; private set; }
        protected override bool CanAttack => _warriors.Count != 0 && _warriors.Any(IsAvailable);
        

        public override void Initialize(CoreConfig coreConfig, EnemyTracker enemyTracker, GlobalPool globalPool)
        {
            base.Initialize(coreConfig, enemyTracker, globalPool);

            _warriorPrefab = coreConfig.Warehouse.WarriorPrefab;
            _warriorTowerData = _baseTowerData as WarriorTowerData;

            SpawnUnits();

            Damagables = new List<IDamagable>(_warriors);
        }

        private void SpawnUnits()
        {
            for (int i = 0; i < _warriorTowerData.WarriorAmount; i++)
            {
                Warrior warrior = _globalPool.Get(_warriorPrefab, parent: transform);
                _warriors.Add(warrior);
                warrior.Appeared += OnWarriorAppeared;
                warrior.Initialize(_coreConfig, _upgradeData.PowerCoefficient);
            }
        }

        public override void Clear()
        {
            base.Clear();

            foreach (BaseUnit baseUnit in _warriors)
            {
                baseUnit.Appeared -= OnWarriorAppeared;
                baseUnit.Clear();
            }
        }

        private void OnWarriorAppeared(BaseUnit baseUnit)
        {
            int i = _warriors.IndexOf(baseUnit);
            float delta = Mathf.PI * 2 / _warriorTowerData.WarriorAmount;
            Vector2 offset = BaseExtensions.GetPos(i * delta) * _warriorTowerData.SpawnRadius;
            Vector3 basePos = _unitSpawnPoint.position;
            Vector3 pos = new(basePos.x + offset.x, basePos.y, basePos.z + offset.y);
            baseUnit.Transform.position = pos;

            WarriorAdded(baseUnit);
        }

        protected override void Update()
        {
            if (_isEnabled == false)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _baseTowerData.AttackCooldown && CanAttack)
            {
                BaseEnemy target = CoreMethods.FindClosest(_enemyTracker.Enemies, Radius, transform);
                if (target != null && target.BehaviourType != EnemyBehaviourType.Runner)
                {
                    _elapsedTime = 0f;
                    Attack(target);
                }
            }
        }

        protected override void Attack(BaseEnemy target)
        {
            base.Attack(target);

            BaseUnit availableUnit = _warriors.Find(IsAvailable);
            availableUnit.SetTarget(target);
            availableUnit.GoToTarget();
        }

        private static bool IsAvailable(BaseUnit warrior) => 
            warrior.IsAlive && warrior.IsBusy == false;
    }
}