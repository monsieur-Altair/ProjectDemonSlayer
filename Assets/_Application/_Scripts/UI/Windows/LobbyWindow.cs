using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class LobbyWindow : Window
    {
        [SerializeField] private Button _toStatsButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _upgradeTowersButton;
        [SerializeField] private Button _upgradeHeroesButton;

        private StateMachine _stateMachine;


        public override void GetDependencies()
        {
            base.GetDependencies();

            _stateMachine = AllServices.Get<StateMachine>();
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            _toStatsButton.onClick.AddListener(OpenStats);
            _startGameButton.onClick.AddListener(StartGame);
            
            _upgradeTowersButton.onClick.AddListener(OpenTowerUpgrades);
            _upgradeHeroesButton.onClick.AddListener(OpenHeroesUpgrades);
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
            _toStatsButton.onClick.RemoveListener(OpenStats);
            _startGameButton.onClick.RemoveListener(StartGame);
            
            _upgradeTowersButton.onClick.RemoveListener(OpenTowerUpgrades);
            _upgradeHeroesButton.onClick.RemoveListener(OpenHeroesUpgrades);
        }

        private void OpenHeroesUpgrades()
        {
            Close();
            UISystem.ShowWindow<HeroUpgradeWindow>();
        }

        private void StartGame()
        {
            _stateMachine.Enter<GameLoopState>();   
        }

        private void OpenTowerUpgrades()
        {
            Close();
            UISystem.ShowWindow<TowerUpgradeWindow>();
        }
        
        private void OpenStats()
        {
            Close();
            UISystem.ShowWindow<StatisticWindow>();
        }
    }
}