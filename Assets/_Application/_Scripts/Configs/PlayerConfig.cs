using System;
using System.Linq;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [CreateAssetMenu (fileName = "player",menuName = "Resources/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
    }
    
    [Serializable]
    public class Pair<TKey, TValue> where TKey : Enum 
    {
        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;

        public TValue Value => _value;
        public TKey Key => _key;
    }

    [Serializable] 
    public class MyDictionary<TKey, TValue> where TKey : Enum 
    {
        [SerializeField] private Pair<TKey, TValue>[] _pairs;
            
        public TValue GetValue(TKey key)
        {
            return _pairs.First(pair => pair.Key.Equals(key)).Value;
        }
        public Pair<TKey, TValue>[] Pairs => _pairs;
    } 
}