using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Audio;


namespace Interactiva.Debugging
{
    /// <summary>
    /// DebugManager is a class that adds functionality to the Interactiva.Core systems for debugging. It should only be used for testing and not included in final build.
    /// </summary>
    public class DebugManager : MonoBehaviour
    {
        /// <summary>
        /// If true, objective completion will be logged to console.
        /// </summary>
        [SerializeField] private bool logObjectives = true;
        /// <summary>
        /// On Start is a convenient event that is executed at the beginning of the simulation.
        /// </summary>
        [SerializeField] private UnityEvent onStart;
        /// <summary>
        /// Debug action is an event that you can set up with a specific key or input.
        /// </summary>
        [SerializeField] private InputAction debugAction;
        /// <summary>
        /// Event triggered when debugAction input is actioned.
        /// </summary>
        [SerializeField] private UnityEvent onDebug;

        /// <summary>
        /// Debug action is an event that you can set up with a specific key or input.
        /// </summary>
        [SerializeField] private InputAction debugAction2;
        /// <summary>
        /// Event triggered when debugAction2 input is actioned.
        /// </summary>
        [SerializeField] private UnityEvent onDebug2;
        /// <summary>
        /// Debug action is an event that you can set up with a specific key or input.
        /// </summary>
        [SerializeField] private InputAction debugAction3;
        /// <summary>
        /// Event triggered when debugAction3 input is actioned.
        /// </summary>
        [SerializeField] private UnityEvent onDebug3;
        public static DebugManager singleton;

        
        private void Start()
        {
            Debug.LogWarning("Debug Manager: On Start invoked. Debugging is active. Please remove Debug Manager for final build.");
            onStart.Invoke();
            singleton = this;
            debugAction.Enable();
            debugAction2.Enable();
            debugAction3.Enable();
        }

        

        private void Update()
        {
            if (debugAction2.triggered)
            {
                Debug.Log("Debug Manager: Debug input 2 pressed");
                onDebug2.Invoke();
                PrintUnityEventListeners(onDebug2);
            }
            if (debugAction.triggered)
            {
                Debug.Log("Debug Manager: Debug input pressed");
                onDebug.Invoke();
                PrintUnityEventListeners(onDebug);
            }
            if (debugAction3.triggered)
            {
                Debug.Log("Debug Manager: Debug input 3 pressed");
                onDebug3.Invoke();
                PrintUnityEventListeners(onDebug3);
            }
            
        }

        public void PrintUnityEventListeners(UnityEvent ev)
        {
            string methods = ev.ToString() + "\n";
            
            for (int i = 0; i < ev.GetPersistentEventCount(); i++)
            {
                methods += (ev.GetPersistentMethodName(i) + "\n");
            }
            Debug.Log(methods);
        }
        
        public void LogToConsole(string msg)
        {
            Debug.Log("DEBUG MESSAGE: " + msg);
        } 
        
        
    }
}