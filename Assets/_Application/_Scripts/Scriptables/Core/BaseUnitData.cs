using UnityEngine;

namespace _Application.Scripts.Scriptables.Core.Enemies
{
    [CreateAssetMenu(fileName = "Base Unit Data", menuName = "Resources/Base Unit Data", order = 0)]
    public class BaseUnitData : BaseData
    {
        [SerializeField] private float _reviveDur;
        [SerializeField] private float _closeAttackRadius;

        public float CloseAttackRadius => _closeAttackRadius;
        public float ReviveDur => _reviveDur;
    }
}