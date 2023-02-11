using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class CollectionsExtensions
    {
        /// <summary>
        /// Compares two lists element by element
        /// </summary>
        public static bool Compare<T>(this List<T> original, List<T> second)
        {
            if (original.Count != second.Count)
                return false;
            for (int i = 0; i < original.Count; ++i)
            {
                if (!ReferenceEquals(original[i],second[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns random element of a list
        /// </summary>
        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        /// <summary>
        /// Returns random element of a array
        /// </summary>
        public static T Random<T>(this T[] list)
        {
            return list[UnityEngine.Random.Range(0, list.Length)];
        }

        /// <summary>
        /// Returns random element of a list and removes is
        /// </summary>
        public static T RandomAndRemove<T>(this List<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            var t = list[index];
            list.RemoveAt(index);
            return t;
        }

        /// <summary>
        /// Returns random element of an Array
        /// </summary>
        public static object Random(this Array data)
        {
            return data.GetValue(UnityEngine.Random.Range(0, data.Length));
        }
        
        /// <summary>
        /// Returns random element of an Array and casts it to T
        /// </summary>
        public static T Random<T>(this Array data)
        {
            return (T)data.GetValue(UnityEngine.Random.Range(0, data.Length));
        }

        /// <summary>
        /// Shuffles list
        /// </summary>
        public static T[] Shuffle<T>(this List<T> data)
        {
            T[] shuffledData = new T[data.Count];
            List<T> copiedData = new(data);
            for (int i = 0; i < data.Count; i++) 
                shuffledData[i] = copiedData.RandomAndRemove();
            return shuffledData;
        }

        /// <summary>
        /// Returns values of enum, cast to IEnumerable
        /// </summary>
        public static IEnumerable<T> GetValues<T>() where T : Enum => 
            Enum.GetValues(typeof(T)).Cast<T>();
    }
}