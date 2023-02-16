using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Towers
{
    [CreateAssetMenu(fileName = "Warrior tower", menuName = "Resources/Towers/Warrior tower", order = 0)]
    public class WarriorTowerData : BaseTowerData
    {
        [SerializeField] private int _warriorAmount = 3;
        [SerializeField] private float _spawnRadius = 3f;

        public int WarriorAmount => _warriorAmount;
        public float SpawnRadius => _spawnRadius;
    }
}