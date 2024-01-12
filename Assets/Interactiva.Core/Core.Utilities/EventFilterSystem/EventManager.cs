using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;

namespace Interactiva.Core.Utilities
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        [SerializeField] private List<EventNamed> events = new List<EventNamed>();

        private Dictionary<string, UnityEvent> eventsDic = new Dictionary<string, UnityEvent>();

        private Dictionary<string, Action<object[]>> eventsDicParams = new Dictionary<string, Action<object[]>>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        private void Start()
        {
            foreach (var item in events)
            {
                eventsDic.Add(item.key, item.onTrigger);
            }
        }

        [Serializable]
        internal class EventNamed
        {
            public string key;
            public UnityEvent onTrigger;

            public EventNamed(string keyParam, UnityAction onTriggerParam)
            {
                key = keyParam;
                onTrigger.AddListener(onTriggerParam);
            }
        }

        //WITHOUT PARAMETERS

        /// <summary>
        /// Add methods with specific key. Remember to subscribe afther the Awake method.
        /// </summary>
        /// <param name="key">Key of dictionary that contains unityaction.</param>
        /// <param name="action">Method to subscribe.</param>
        public void Subscribe(string key, UnityAction action)
        {
            if (eventsDic.ContainsKey(key)) eventsDic[key].AddListener(action);
            else
            {
                var element = new EventNamed(key, action);

                eventsDic.Add(key, element.onTrigger);
            }
        }

        /// <summary>
        /// Trigger methods with specific key.
        /// </summary>
        /// <param name="key">Key of dictionary that Invoke methods.</param>
        public void TriggerEvent(string key)
        {
            if (eventsDic.ContainsKey(key)) eventsDic.First(x => x.Key == key).Value?.Invoke();
        }

        //WITH PARAMETERS

        /// <summary>
        /// Add methods (that could recibe parameters) with a specific key. Remember to subscribe afther the Awake method.
        /// </summary>
        /// <param name="key">Key of dictionary that contains methods.</param>
        /// <param name="action">Method to subscribe.</param>
        public void SubscribeWithParams(string key, Action<object[]> action)
        {
            if (eventsDicParams.ContainsKey(key)) eventsDicParams[key] += action;
            else eventsDicParams.Add(key, action);
        }

        /// <summary>
        /// Remove existing methods.
        /// </summary>
        /// <param name="key">Key of dictionay that contains methods.</param>
        /// <param name="action">Method to remove.</param>
        public void UnsubscribeWithParams(string key, Action<object[]> action)
        {
            if (eventsDicParams.ContainsKey(key)) eventsDicParams[key] -= action;
        }

        /// <summary>
        /// Trigger Methods with a specific key. Can receive parameters.
        /// </summary>
        /// <param name="key">Key of dictionary that contains methods.</param>
        /// <param name="parameters">Parameters that the method receives.</param>
        public void TriggerWithParams(string key, params object[] parameters)
        {
            if (eventsDicParams.ContainsKey(key)) eventsDicParams[key]?.Invoke(parameters);
        }
    }
}