using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Scriptables.Core.Towers;
using Extensions;

namespace _Application.Scripts.SavedData
{
    [Serializable]
    public class PlayerProgress
    {
        public LevelInfo LevelInfo;
        public int Money;
        public TowersUpgrade[] TowersUpgrades;
        public Statistic Statistic;
        
        public PlayerProgress(int levelNumber)
        {
            Money = 0;
            LevelInfo = new LevelInfo(levelNumber);
            Statistic = new Statistic();
            CreateAchieveUpgrades();
        }

        private void CreateAchieveUpgrades()
        {
            List<TowerType> towerTypes = CollectionsExtensions.GetValues<TowerType>().ToList();
            int length = towerTypes.Count;

            if (length == 0)
                return;

            TowersUpgrades = new TowersUpgrade[length];

            for (int i = 0; i < length; i++)
            {
                TowersUpgrades[i] = new TowersUpgrade
                { 
                    TowerType = towerTypes[i],
                    SavedCard = 0
                };
            }
        }
    }
}