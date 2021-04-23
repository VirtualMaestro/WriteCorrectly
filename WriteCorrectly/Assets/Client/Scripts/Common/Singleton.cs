using System;
using UnityEngine;

namespace Client.Scripts.Common
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(_CreateSingleton);

        public static T I => LazyInstance.Value;

        private static T _CreateSingleton()
        {
            var existingInstance = FindObjectsOfType<T>();
            
            if (existingInstance == null || existingInstance.Length == 0)
            {
                var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
                var instance = ownerObject.AddComponent<T>();
                DontDestroyOnLoad(ownerObject);
                return instance;
            }

            return existingInstance[0];
        }
    }
}