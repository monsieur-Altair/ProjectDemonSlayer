using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Application._Scripts.Scriptables.Core.Levels
{
    [Serializable]
    public class WaveData
    {
        [SerializeField, NonReorderable] private List<MiniWaveData> _miniWavesData;
        [SerializeField] private float _startDelay;

        public List<MiniWaveData> MiniWavesData => _miniWavesData;
        public float StartDelay => _startDelay;
    }
}