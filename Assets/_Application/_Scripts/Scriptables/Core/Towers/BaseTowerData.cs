using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Towers
{
    [CreateAssetMenu(fileName = "base tower", menuName = "Resources/Towers/Base", order = 0)]
    public class BaseTowerData : BaseUnitData
    {
        [SerializeField] private float _radius;
        [SerializeField] private TowerType _towerType;


        public TowerType TowerType => _towerType;
        public float Radius => _radius;
    }
}