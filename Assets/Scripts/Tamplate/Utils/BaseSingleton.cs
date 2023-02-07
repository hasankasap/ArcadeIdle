using System.Threading;
using UnityEngine;

namespace Game {
    // This object's lifetime depends on the related Scene. When the scene destroyed, this object is destroyed.
    public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        private static bool _isDestroyed;

        public static T instance {
            get {
                if (_isDestroyed) {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T).FullName}' already destroyed. Returning null.");
                    return null;
                }
                if (_instance == null) {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (_instance == null) {
                        Debug.Log(typeof(T) + " singleton object is not instantiated yet.");
                    }

                    if (FindObjectsOfType(typeof(T)).Length > 1) {
                        Debug.LogError("[Singleton] Something went really wrong - there should never be more then 1 singletion!" +
                                " Reopening the scene might fix it. Thread: " + Thread.CurrentThread.Name);
                    }
                }
                return _instance;
            }
        }

        virtual protected void Awake() {

            string name = typeof(T).Name;
            if (_instance == null) {
                _instance = GetComponent<T>();
                Debug.Log($"[{name}::Awake] BaseSingleton object initialized.");
            } else {
                if (gameObject.GetComponents<Component>().Length > 1) {
                    Debug.Log($"[{name}::Awake] '{name}' already created! GameObject has other components, so just destroying newly created component.");
                    DestroyImmediate(this);
                } else {
                    Debug.Log($"[{name}::Awake] '{gameObject.name}' already created! Destroying newly created one");
                    DestroyImmediate(gameObject);
                }
            }
        }

        private void OnApplicationQuit() {
            _isDestroyed = true;
        }
    }
}