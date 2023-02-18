using System;
using _Application._Scripts.Scriptables.Core.Towers;
using _Application.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class BuyTowerButton : MonoBehaviour
    {
        public event Action<TowerType, int> Clicked = delegate { }; 
        
        [SerializeField] private Button _button;
        [SerializeField] private TowerType _towerType;
        [SerializeField] private TextMeshProUGUI _costTMP;

        private int _cost;

        public void Initialize(CoreConfig coreConfig)
        {
            _cost = coreConfig.TowersData[_towerType][0].BuildCost;
            _costTMP.text = _cost.ToString();
        }
        
        public void Subscribe()
        {
            _button.onClick.AddListener(OnClicked);
        }

        public void Unsubscribe()
        {
            _button.onClick.RemoveListener(OnClicked);
        }
        
        private void OnClicked()
        {
            Clicked(_towerType, _cost);
        }
    }
}