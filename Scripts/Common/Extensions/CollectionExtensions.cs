using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.Extensions
{
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }

            return list[Random.Range(0, list.Count)]; 
        }
        
        public static TV GetRandomElement<T, TV>(this Dictionary<T, TV> dict)
        {
            if (dict == null || dict.Count == 0)
            {
                return default;
            }

            return dict.ElementAt(Random.Range(0, dict.Count)).Value; 
        }
        
        
    }
}