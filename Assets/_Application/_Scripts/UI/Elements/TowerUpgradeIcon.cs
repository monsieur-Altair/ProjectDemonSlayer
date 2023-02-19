using System;
using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application._Scripts.Scriptables.Core.TowerUpgrade;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.SavedData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class TowerUpgradeIcon : MonoBehaviour
    {
        public event Action<TowerType, int> Clicked = delegate { };

        [SerializeField] private Button _button;
        [SerializeField] private TowerType _towerType;
        [SerializeField] private int _towerLevel;
        [SerializeField] private GameObject _canUpgradeIndicator;
        [SerializeField] private TextMeshProUGUI _upgradeLevelTMP;

        private ProgressService _progressService;
        private List<BaseTowerUpgradeData> _towersUpgrades;

        public void Initialize(CoreConfig coreConfig, ProgressService progressService)
        {
            _progressService = progressService;
            _towersUpgrades = coreConfig.TowersUpgradesLists[_towerType][_towerLevel].List;
        }

        public void OnOpened()
        {
            _button.onClick.AddListener(OnClicked);

            TowersUpgrade upgrade = _progressService.PlayerProgress.TowersUpgrades
                .First(upgrade => upgrade.TowerType == _towerType);

            int currentLevel = upgrade.AchievedLevels[_towerLevel];
            _upgradeLevelTMP.text = $"{currentLevel + 1}";

            bool isMaxLevel = currentLevel == _towersUpgrades.Count - 1;

            if (isMaxLevel)
            {
                _canUpgradeIndicator.SetActive(false);
                return;
            }

            bool canUpgrade = upgrade.SavedCard[_towerLevel] >= _towersUpgrades[currentLevel + 1].RequiredCardAmount;
            _canUpgradeIndicator.SetActive(canUpgrade);
        }

        public void OnClosed()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked(_towerType, _towerLevel);
        }
    }
}