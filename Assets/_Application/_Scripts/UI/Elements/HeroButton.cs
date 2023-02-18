using _Application._Scripts.Core.Heroes;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class HeroButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _maskImage;
        
        private BaseHero _baseHero;
        private bool _isOnCooldown;
        private float _elapsedTime;
        private float _cooldownTime;

        public void OnOpened(BaseHero hero)
        {
            _baseHero = hero;
            _cooldownTime = _baseHero.SkillCooldown;
        }

        private void Update()
        {
            if (_isOnCooldown == false)
                return;

            _elapsedTime += Time.deltaTime;
            float percent = Mathf.Clamp01(_elapsedTime / _cooldownTime);
            _maskImage.fillAmount = 1 - percent;

            if (percent > 1 - float.Epsilon) 
                _isOnCooldown = false;
        }

        public void Subscribe()
        {
            _button.onClick.AddListener(ApplyUltimate);
        }

        public void Unsubscribe()
        {
            _button.onClick.RemoveListener(ApplyUltimate);
        }

        private void ApplyUltimate()
        {
            if(_isOnCooldown)
                return;
            
            _baseHero.DoUltimate();
            _elapsedTime = 0.0f;
            
            _isOnCooldown = true;
        }
    }
}