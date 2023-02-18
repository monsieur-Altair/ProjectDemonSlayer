using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class TowerButton : MonoBehaviour
    {
        public event Action<int> Clicked = delegate { }; 
        
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _costTMP;

        private int _cost;

        public void OnOpened(int cost)
        {
            _cost = cost;
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
            Clicked(_cost);
        }
    }
}