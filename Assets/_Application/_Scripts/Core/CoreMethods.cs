using System.Collections.Generic;
using System.Linq;
using _Application._Scripts.Core.Towers;
using _Application.Scripts.Misc;
using _Application.Scripts.Scriptables.Core.Enemies;
using UnityEngine;

namespace _Application._Scripts.Core
{
    public static class CoreMethods
    {
        public static float CalculateDamage(List<DamageInfo> attackInfos, List<DamageInfo> defenceInfos)
        {
            float result = 0.0f;

            foreach (DamageInfo attackInfo in attackInfos)
            {
                DamageInfo defenceInfo = defenceInfos.FirstOrDefault(defenceInfo => defenceInfo.DamageType == attackInfo.DamageType);
                float defenceValue = defenceInfo?.Value ?? 0.0f;
                result += attackInfo.Value * (1 - defenceValue);
            }

            return result;
        }
        
        public static T FindClosest<T>(List<T> list, float radius, Transform centrePoint) where T : IFindable
        {
            T target = default;
            float minDistance = 1000000f; 
            
            foreach (T findable in list)
            {
                float distance = Vector2.Distance(findable.FindPoint.position.ToXZ(), centrePoint.position.ToXZ());
                if (distance < minDistance && distance < radius)
                {
                    minDistance = distance;
                    target = findable;
                }
            }

            return target;
        }
    }
}