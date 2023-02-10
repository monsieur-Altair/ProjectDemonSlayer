using System;
using System.Linq;

namespace _Application.Scripts.SavedData
{
    [Serializable]
    public class PlayerProgress
    {
        public LevelInfo LevelInfo;
        public int Money;
        public AchievedUpgrades[] AchievedUpgrades;
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
            int length = Enum.GetValues(typeof(UpgradeType)).Length;

            if (length == 0)
                return;

            AchievedUpgrades = new AchievedUpgrades[length];
            
            for (int i = 0; i < length; i++)
                AchievedUpgrades[i] = new AchievedUpgrades();
        }
    }
}