using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application._Scripts.Scriptables.Core.TowerUpgrade;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.SavedData;
using _Application.Scripts.Scriptables.Core.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class TowerUpgradePopup : MonoBehaviour
    {
        private const string Format = "F0";

        [SerializeField] private Button _upgradeButton; 
        [SerializeField] private Button _closeButton;

        [SerializeField] private List<Image> _bars;
        [SerializeField] private List<TextMeshProUGUI> _barsValues;

        [SerializeField] private TextMeshProUGUI _cardAmountTMP;
        [SerializeField] private GameObject _cardRectGO;
        
        [SerializeField] private TextMeshProUGUI _upgradeLevelTMP;
        
        private const int HealthIndex = 0;
        private const int PowerIndex = 1;
        private const int RangeIndex = 2;
        
        private ProgressService _progressService;
        private CoreConfig _coreConfig;
        private TowersUpgrade _towersUpgrade;
        private int _towerLevel;
        private TowerType _towerType;

        public void Initialize(CoreConfig coreConfig, ProgressService progressService)
        {
            _coreConfig = coreConfig;
            _progressService = progressService;
        }

        public void OnOpened(TowerType towerType, int towerLevel)
        {
            _towerType = towerType;
            _towerLevel = towerLevel;
            
            List<BaseTowerUpgradeData> towersUpgrades = _coreConfig.TowersUpgradesLists[_towerType][_towerLevel];
            BaseTowerData baseTowerData = _coreConfig.TowersData[_towerType][_towerLevel];

            _towersUpgrade = _progressService.PlayerProgress.TowersUpgrades
                .First(upgrade => upgrade.TowerType == _towerType);

            int currentLevel = _towersUpgrade.AchievedLevels[_towerLevel];
            _upgradeLevelTMP.text = $"{currentLevel + 1}";

            SetHealth(towersUpgrades, currentLevel, baseTowerData);
            SetPower(towersUpgrades, currentLevel, baseTowerData);
            SetRange(towersUpgrades, currentLevel, baseTowerData);
            
            _closeButton.onClick.AddListener(Close);
            _upgradeButton.onClick.AddListener(Upgrade);

            bool isMaxLevel = currentLevel == towersUpgrades.Count - 1;
            if (isMaxLevel)
            {
                _cardRectGO.SetActive(false);
                _upgradeButton.gameObject.SetActive(false);
                return;
            }
            
            SetCardZone(towersUpgrades, currentLevel);
        }

        private void Upgrade()
        {
            List<BaseTowerUpgradeData> towersUpgrades = _coreConfig.TowersUpgradesLists[_towerType][_towerLevel];
            int currentLevel = _towersUpgrade.AchievedLevels[_towerLevel];
            
            _towersUpgrade.SavedCard[_towerLevel] -= towersUpgrades[currentLevel + 1].RequiredCardAmount;
            _towersUpgrade.AchievedLevels[_towerLevel]++;
            
            OnClosed();
            OnOpened(_towerType, _towerLevel);
        }

        private void Close()
        {
            OnClosed();
            gameObject.SetActive(false);
        }

        private void OnClosed()
        {
            _closeButton.onClick.RemoveListener(Close);
            _upgradeButton.onClick.RemoveListener(Upgrade);
        }

        private void SetRange(List<BaseTowerUpgradeData> towersUpgrades, int currentLevel, BaseTowerData baseTowerData)
        {
            float minCoefficient = towersUpgrades[0]            .RangeCoefficient;
            float maxCoefficient = towersUpgrades[^1]           .RangeCoefficient;
            float currCoefficient = towersUpgrades[currentLevel].RangeCoefficient;

            float percent = (currCoefficient - minCoefficient) / (maxCoefficient - minCoefficient);
            _bars[RangeIndex].fillAmount = percent;
            _barsValues[RangeIndex].text = (baseTowerData.Radius * currCoefficient).ToString(Format);
        }
        private void SetPower(List<BaseTowerUpgradeData> towersUpgrades, int currentLevel, BaseTowerData baseTowerData)
        {
            float minCoefficient = towersUpgrades[0]            .PowerCoefficient;
            float maxCoefficient = towersUpgrades[^1]           .PowerCoefficient;
            float currCoefficient = towersUpgrades[currentLevel].PowerCoefficient;

            float percent = (currCoefficient - minCoefficient) / (maxCoefficient - minCoefficient);
            float damage = CoreMethods.CalculateDamage(baseTowerData.AttackInfo, new List<DamageInfo>());
            _bars[PowerIndex].fillAmount = percent;
            _barsValues[PowerIndex].text = (damage * currCoefficient).ToString(Format);
        }

        private void SetHealth(List<BaseTowerUpgradeData> towersUpgrades, int currentLevel, BaseTowerData baseTowerData)
        {
            float minCoefficient = towersUpgrades[0].HealthCoefficient;
            float maxCoefficient = towersUpgrades[^1].HealthCoefficient;
            float currCoefficient = towersUpgrades[currentLevel].HealthCoefficient;

            float percent = (currCoefficient - minCoefficient) / (maxCoefficient - minCoefficient);
            _bars[HealthIndex].fillAmount = percent;
            _barsValues[HealthIndex].text = (baseTowerData.Health * currCoefficient).ToString(Format);
        }

        private void SetCardZone(List<BaseTowerUpgradeData> towersUpgrades, int currentLevel)
        {
            int currentAmount = _towersUpgrade.SavedCard[_towerLevel];
            int requiredCardAmount = towersUpgrades[currentLevel + 1].RequiredCardAmount;
            _cardAmountTMP.text = $"{currentAmount / requiredCardAmount}";
            
            _upgradeButton.gameObject.SetActive(true);
            _upgradeButton.interactable = currentAmount >= requiredCardAmount;
        }
    }
}