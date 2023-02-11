using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Application.Scripts.Managers
{
    [Serializable] 
    public class MyDictionary<TKey, TValue> where TKey : Enum 
    {
        [SerializeField, NonReorderable] private List<Pair<TKey, TValue>> _pairs;

        public TValue this[TKey key] => 
            _pairs.First(pair => pair.Key.Equals(key)).Value;
        
        public List<Pair<TKey, TValue>> Pairs => _pairs;
    }
    
    [Serializable]
    public class Pair<TKey, TValue> where TKey : Enum 
    {
        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;

        public TValue Value => _value;
        public TKey Key => _key;
    }
}