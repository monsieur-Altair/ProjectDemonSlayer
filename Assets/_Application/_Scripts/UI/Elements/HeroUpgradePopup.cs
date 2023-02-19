using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core;
using _Application._Scripts.Core.Heroes;
using _Application._Scripts.Scriptables.Core.TowerUpgrade;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using _Application.Scripts.Misc;
using _Application.Scripts.SavedData;
using _Application.Scripts.Scriptables.Core.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class HeroUpgradePopup : MonoBehaviour
    {
        private const string Format = "F0";

        [SerializeField] private TextMeshProUGUI _labelTMP;

        [SerializeField] private float _previewPreferredSize = 360f;
        [SerializeField] private Image _heroPreviewImage;

        [SerializeField] private float _cardPreferredSize = 360f;
        [SerializeField] private Image _heroCardImage;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _closeButton;

        [SerializeField] private List<Image> _bars;
        [SerializeField] private List<TextMeshProUGUI> _barsValues;

        [SerializeField] private TextMeshProUGUI _cardAmountTMP;
        [SerializeField] private GameObject _cardRectGO;

        [SerializeField] private TextMeshProUGUI _upgradeLevelTMP;

        private const int HealthIndex = 0;
        private const int PowerIndex = 1;
        private const int SpeedIndex = 2;

        private ProgressService _progressService;
        private CoreConfig _coreConfig;
        private HeroType _heroType;
        private HeroUpgrade _heroUpgrade;

        public void Initialize(CoreConfig coreConfig, ProgressService progressService)
        {
            _coreConfig = coreConfig;
            _progressService = progressService;
        }

        public void OnOpened(HeroType heroType)
        {
            _heroType = heroType;

            SetImages();
            
            _labelTMP.text = $"{heroType}";

            List<BaseHeroUpgradeData> heroUpgrades = _coreConfig.HeroUpgrades[_heroType];
            BaseUnitData baseHeroData = _coreConfig.HeroDatas[_heroType];

            _heroUpgrade = _progressService.PlayerProgress.HeroUpgrades
                .First(upgrade => upgrade.HeroType == _heroType);

            int currentLevel = _heroUpgrade.AchievedLevel;
            _upgradeLevelTMP.text = $"{currentLevel + 1}";

            SetHealth(heroUpgrades, currentLevel, baseHeroData);
            SetPower(heroUpgrades, currentLevel, baseHeroData);
            SetSpeed(heroUpgrades, currentLevel, baseHeroData);

            _closeButton.onClick.AddListener(Close);
            _upgradeButton.onClick.AddListener(Upgrade);

            bool isMaxLevel = currentLevel == heroUpgrades.Count - 1;
            if (isMaxLevel)
            {
                _cardRectGO.SetActive(false);
                _upgradeButton.gameObject.SetActive(false);
                return;
            }

            SetCardZone(heroUpgrades, currentLevel);
        }

        private void SetImages()
        {
            Sprite previewSprite = _coreConfig.Warehouse.HeroPreviewSprites[_heroType];
            _heroPreviewImage.rectTransform.sizeDelta = previewSprite.GetResized(_previewPreferredSize);
            _heroPreviewImage.sprite = previewSprite;

            Sprite cardSprite = _coreConfig.Warehouse.HeroCardSprites[_heroType];
            _heroCardImage.rectTransform.sizeDelta = cardSprite.GetResized(_cardPreferredSize);
            _heroCardImage.sprite = cardSprite;
        }

        private void Upgrade()
        {
            List<BaseHeroUpgradeData> heroUpgrades = _coreConfig.HeroUpgrades[_heroType];
            int currentLevel = _heroUpgrade.AchievedLevel;

            _heroUpgrade.SavedCard -= heroUpgrades[currentLevel + 1].RequiredCardAmount;
            _heroUpgrade.AchievedLevel++;

            OnClosed();
            OnOpened(_heroType);
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

        private void SetHealth(List<BaseHeroUpgradeData> heroUpgrades, int currentLevel, BaseUnitData baseHeroData)
        {
            float minCoefficient = heroUpgrades[0].HealthCoefficient;
            float maxCoefficient = heroUpgrades[^1].HealthCoefficient;
            float currCoefficient = heroUpgrades[currentLevel].HealthCoefficient;

            float percent = (currCoefficient - minCoefficient) / (maxCoefficient - minCoefficient);
            _bars[HealthIndex].fillAmount = percent;
            _barsValues[HealthIndex].text = (baseHeroData.Health * currCoefficient).ToString(Format);
        }

        private void SetSpeed(List<BaseHeroUpgradeData> heroUpgrades, int currentLevel, BaseUnitData baseHeroData)
        {
            float minCoefficient = heroUpgrades[0].SpeedCoefficient;
            float maxCoefficient = heroUpgrades[^1].SpeedCoefficient;
            float currCoefficient = heroUpgrades[currentLevel].SpeedCoefficient;

            float percent = (currCoefficient - minCoefficient) / (maxCoefficient - minCoefficient);
            _bars[SpeedIndex].fillAmount = percent;
            _barsValues[SpeedIndex].text = (baseHeroData.MotionsSpeed * currCoefficient).ToString(Format);
        }

        private void SetPower(List<BaseHeroUpgradeData> heroUpgrades, int currentLevel, BaseUnitData baseHeroData)
        {
            float minCoefficient = heroUpgrades[0].PowerCoefficient;
            float maxCoefficient = heroUpgrades[^1].PowerCoefficient;
            float currCoefficient = heroUpgrades[currentLevel].PowerCoefficient;

            float percent = (currCoefficient - minCoefficient) / (maxCoefficient - minCoefficient);
            float damage = CoreMethods.CalculateDamage(baseHeroData.AttackInfo, new List<DamageInfo>());
            _bars[PowerIndex].fillAmount = percent;
            _barsValues[PowerIndex].text = (damage * currCoefficient).ToString(Format);
        }

        private void SetCardZone(List<BaseHeroUpgradeData> heroUpgrades, int currentLevel)
        {
            int currentAmount = _heroUpgrade.SavedCard;
            int requiredCardAmount = heroUpgrades[currentLevel + 1].RequiredCardAmount;
            _cardAmountTMP.text = $"{currentAmount}/{requiredCardAmount}";

            _upgradeButton.gameObject.SetActive(currentAmount >= requiredCardAmount);
        }
    }
}