using Pool_And_Particles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Application.Scripts.UI.Windows
{
    public class CardUI : PooledBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;

        public void Init(Sprite sprite, int count)
        {
            _icon.sprite = sprite;
            _tmp.text = count.ToString();
        }
    }
}