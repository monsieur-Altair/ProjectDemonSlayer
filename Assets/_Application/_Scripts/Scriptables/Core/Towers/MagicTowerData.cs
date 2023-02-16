using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Towers
{
    [CreateAssetMenu(fileName = "magic tower", menuName = "Resources/Towers/Magic", order = 0)]
    public class MagicTowerData : BaseTowerData
    {
        [SerializeField] private float _slowDur = 0.6f;
        [SerializeField] private float _slowCoefficient = 0.4f;

        public float SlowDur => _slowDur;
        public float SlowCoefficient => _slowCoefficient;
    }
}