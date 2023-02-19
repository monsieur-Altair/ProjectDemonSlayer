using UnityEngine;

namespace _Application.Scripts.Scriptables.Core.Enemies
{
    [CreateAssetMenu(fileName = "Base Unit Data", menuName = "Resources/Base Unit Data", order = 0)]
    public class BaseUnitData : BaseData
    {
        [Space, SerializeField] private float _reviveDur = 4f;
        [SerializeField] private float _closeAttackRadius = 0.5f;

        public float CloseAttackRadius => _closeAttackRadius;
        public float ReviveDur => _reviveDur;
    }
}