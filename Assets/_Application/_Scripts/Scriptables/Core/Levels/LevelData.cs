using System;
using System.Collections.Generic;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Scriptables.Rewards;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Levels
{
    [CreateAssetMenu(fileName = "level data", menuName = "Resources/Level data", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private List<SingleReward> _currencyRewards;
        [SerializeField] private int _approachingCount;
        [SerializeField] private int _startElixirAmount = 200;

        [SerializeField] private List<WaveData> _wavesData;
        [SerializeField] private CardReward _cardReward;

        public CardReward CardReward => _cardReward;
        public List<SingleReward> CurrencyRewards => _currencyRewards;
        public int ApproachingCount => _approachingCount;
        public int StartElixirAmount => _startElixirAmount;
        public List<WaveData> WavesData => _wavesData;
    }

    [Serializable]
    public class CardReward
    {
        public List<TowerCardReward> TowerCardRewards;
        public List<HeroCardReward> HeroCardRewards;
    }

    
    [Serializable]
    public class HeroCardReward
    {
        public HeroType HeroType;
        public int CardAmount;
    }
    
    [Serializable]
    public class TowerCardReward
    {
        public TowerType TowerType;
        public int Level;
        public int CardAmount;
    }
}