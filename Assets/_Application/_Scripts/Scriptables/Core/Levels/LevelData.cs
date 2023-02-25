using System.Collections.Generic;
using _Application.Scripts.Scriptables.Rewards;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Levels
{
    [CreateAssetMenu(fileName = "level data", menuName = "Resources/Level data", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private List<SingleReward> _rewards;
        [SerializeField] private int _approachingCount;
        [SerializeField] private int _startElixirAmount = 200;

        [SerializeField] private List<WaveData> _wavesData;

        
        public List<SingleReward> Rewards => _rewards;
        public int ApproachingCount => _approachingCount;
        public int StartElixirAmount => _startElixirAmount;
        public List<WaveData> WavesData => _wavesData;
    }
}