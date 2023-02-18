using System.Collections.Generic;
using _Application._Scripts.Core.Heroes;
using _Application.Scripts.Infrastructure.Services;
using _Application.Scripts.Infrastructure.Services.Progress;
using _Application.Scripts.Managers;
using UnityEngine;

namespace _Application.Scripts.UI.Windows
{
    public class HeroUpgradeWindow : Window
    {
        [SerializeField] private List<HeroUpgradeIcon> _heroUpgradeIcons;
        [SerializeField] private HeroUpgradePopup _heroUpgradePopup;
        
        
        public override void GetDependencies()
        {
            base.GetDependencies();

            CoreConfig coreConfig = AllServices.Get<CoreConfig>();
            ProgressService progressService = AllServices.Get<ProgressService>();
            
            foreach (HeroUpgradeIcon heroUpgradeIcon in _heroUpgradeIcons)
                heroUpgradeIcon.Initialize(coreConfig, progressService);
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            foreach (HeroUpgradeIcon heroUpgradeIcon in _heroUpgradeIcons)
            {
                heroUpgradeIcon.OnOpened();
                heroUpgradeIcon.Clicked += OnIconClicked;
            }
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
            foreach (HeroUpgradeIcon heroUpgradeIcon in _heroUpgradeIcons)
            {
                heroUpgradeIcon.OnClosed();
                heroUpgradeIcon.Clicked -= OnIconClicked;
            }
        }

        private void OnIconClicked(HeroType heroType)
        {
            _heroUpgradePopup.gameObject.SetActive(true);
            _heroUpgradePopup.OnOpened(heroType);
        }
    }
}