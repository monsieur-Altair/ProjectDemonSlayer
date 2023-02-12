using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace Extensions
{
    public static class TweenExt
    {
        /// <summary>
        /// Creates a tweener that lerps a number in TMPro, with a % sign at the end
        /// </summary>
        public static Tweener DOPercentage(this TextMeshProUGUI text, int final, float time)
        {
            int value = int.Parse(text.text.Trim('%'));
            return DOTween.To(() => value, val =>
            {
                value = val;
                text.text = val + "%";
            }, final, time);
        }

        /// <summary>
        /// Creates a tweener that waits for specified time
        /// </summary>
        public static Tweener Wait(float delay) =>
            DOTween.To(() => 0f, _ => { }, 0f, delay);

        
        public static Tween DoFalloff(this ProceduralImage image, float endValue, float duration)
        {
            return DOTween.To(() => image.FalloffDistance, 
                value => image.FalloffDistance = value, 
                endValue,
                duration);
        }
    }
}