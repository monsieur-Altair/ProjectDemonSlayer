using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Scriptables.Core.TowerUpgrade;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.SavedData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class HeroUpgradeIcon : MonoBehaviour
    {
        public event Action<HeroType> Clicked = delegate { };
        
        [SerializeField] private Button _button;
        [SerializeField] private HeroType _heroType;
        [SerializeField] private GameObject _canUpgradeIndicator;
        [SerializeField] private TextMeshProUGUI _upgradeLevelTMP;
    
        private ProgressService _progressService;
        private List<BaseHeroUpgradeData> _heroUpgrades;

        public void Initialize(CoreConfig coreConfig, ProgressService progressService)
        {
            _progressService = progressService;
            _heroUpgrades = coreConfig.HeroUpgrades[_heroType];
        }
        
        public void OnOpened()
        {
            _button.onClick.AddListener(OnClicked);
            
            HeroUpgrade heroUpgrade = _progressService.PlayerProgress.HeroUpgrades
                .First(upgrade => upgrade.HeroType == _heroType);

            int currentLevel = heroUpgrade.AchievedLevel;
            _upgradeLevelTMP.text = $"{currentLevel + 1}";

            bool isMaxLevel = currentLevel == _heroUpgrades.Count - 1;

            if (isMaxLevel)
            {
                _canUpgradeIndicator.SetActive(false);
                return;
            }

            bool canUpgrade = heroUpgrade.SavedCard >= _heroUpgrades[currentLevel + 1].RequiredCardAmount;
            _canUpgradeIndicator.SetActive(canUpgrade);
        }
        
        public void OnClosed()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked(_heroType);
        }
    }
}