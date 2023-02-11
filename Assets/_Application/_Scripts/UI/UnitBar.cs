using Pool_And_Particles;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace _Application.Scripts.UI
{
    public class UnitBar : PooledBehaviour
    {
        [SerializeField] private ProceduralImage _fillingPart;
        [SerializeField] private RectTransform _rectTransform;
        
        
        public void SetAnchorPos(Vector2 anchorPos)
        {
            _rectTransform.anchoredPosition = anchorPos;
        }

        public void UpdateBar(float percent)
        {
            _fillingPart.fillAmount = percent;
        }
    }
}