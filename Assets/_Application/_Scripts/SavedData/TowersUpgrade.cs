using System;
using _Application._Scripts.Scriptables.Core.Towers;

namespace _Application.Scripts.SavedData
{
    [Serializable]
    public class TowersUpgrade
    {
        public TowerType TowerType;
        public int SavedCard;
        public int AchievedLevel = -1;
    }
}