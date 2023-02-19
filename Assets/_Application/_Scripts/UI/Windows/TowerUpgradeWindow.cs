using System.Collections.Generic;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class TowerUpgradeWindow : Window
    {
        [SerializeField] private List<TowerUpgradeIcon> _towerUpgradeIcons;
        [SerializeField] private TowerUpgradePopup _towerUpgradePopup;
        [SerializeField] private Button _closeButton;

        public override void GetDependencies()
        {
            base.GetDependencies();

            CoreConfig coreConfig = AllServices.Get<CoreConfig>();
            ProgressService progressService = AllServices.Get<ProgressService>();

            foreach (TowerUpgradeIcon towerUpgradeLabel in _towerUpgradeIcons)
                towerUpgradeLabel.Initialize(coreConfig, progressService);

            _towerUpgradePopup.Initialize(coreConfig, progressService);
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            foreach (TowerUpgradeIcon towerUpgradeLabel in _towerUpgradeIcons)
            {
                towerUpgradeLabel.OnOpened();
                towerUpgradeLabel.Clicked += OnIconClicked;
            }
            
            _closeButton.onClick.AddListener(CloseWindow);
        }
        
        private void CloseWindow()
        {
            Close();
            UISystem.ShowWindow<LobbyWindow>();
        }

        protected override void OnClosed()
        {
            base.OnClosed();

            foreach (TowerUpgradeIcon towerUpgradeLabel in _towerUpgradeIcons)
            {
                towerUpgradeLabel.OnClosed();
                towerUpgradeLabel.Clicked -= OnIconClicked;
            }
            
            _closeButton.onClick.RemoveListener(CloseWindow);
        }

        private void OnIconClicked(TowerType towerType, int towerLevel)
        {
            _towerUpgradePopup.gameObject.SetActive(true);
            _towerUpgradePopup.OnOpened(towerType, towerLevel);
        }
    }
}