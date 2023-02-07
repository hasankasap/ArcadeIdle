using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager eventManager;

        public static EventManager instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        private Dictionary<string, Action<object[]>> eventDictionary;

        private void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, Action<object[]>>();
            }
        }

        public static void StartListening(string eventName, Action<object[]> listener)
        {
            Action<object[]> thisEvent;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += listener;
                instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<object[]> listener)
        {
            if (eventManager == null) return;
            Action<object[]> thisEvent;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= listener;
                instance.eventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, object[] parameters)
        {
            Action<object[]> thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(parameters);
            }
        }
    }
}