using System;
using UnityEngine;

namespace _Application.Scripts.SavedData
{
    [Serializable]
    public class LevelInfo
    {
        [SerializeField] private int _lastCompletedLevel;

        public int LastCompletedLevel => _lastCompletedLevel;

        public LevelInfo(int levelNumber) =>
            SetLevel(levelNumber);

        public void SetLevel(int level) => 
            _lastCompletedLevel = level;
    }
}