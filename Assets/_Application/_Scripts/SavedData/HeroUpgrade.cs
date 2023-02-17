using System;
using _Application._Scripts.Core.Heroes;

namespace _Application.Scripts.SavedData
{
    [Serializable]
    public class HeroUpgrade
    {
        public HeroType HeroType;
        public int UpgradeLevel;
    }
}