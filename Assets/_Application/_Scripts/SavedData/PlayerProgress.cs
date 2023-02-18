using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Managers;
using Extensions;

namespace _Application.Scripts.SavedData
{
    [Serializable]
    public class PlayerProgress
    {
        public LevelInfo LevelInfo;
        public int Money;
        public TowersUpgrade[] TowersUpgrades;
        public HeroUpgrade[] HeroUpgrades;
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
            CreateHeroUpgrades();            
            CreateTowerUpgrades();
        }

        private void CreateHeroUpgrades()
        {
            List<HeroType> heroTypes = CollectionsExtensions.GetValues<HeroType>().ToList();
            
            int length = heroTypes.Count;

            if (length == 0)
                return;

            HeroUpgrades = new HeroUpgrade[length];

            for (int i = 0; i < length; i++)
            {
                HeroUpgrades[i] = new HeroUpgrade
                {
                    HeroType = heroTypes[i],
                    AchievedLevel = 0,
                    SavedCard = 0
                };
            }
        }

        private void CreateTowerUpgrades()
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
                    SavedCard = new int[CoreConfig.TowerLevelAmount],
                    AchievedLevels = new int[CoreConfig.TowerLevelAmount]
                };
            }
        }
    }
}