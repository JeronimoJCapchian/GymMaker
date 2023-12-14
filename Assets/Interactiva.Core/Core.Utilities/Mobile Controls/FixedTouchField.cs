using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.OnScreen {
    /// <summary>
    /// Class that gives a 
    /// </summary>
    public class FixedTouchField : OnScreenControl, IPointerDownHandler, IPointerUpHandler
    {


        [SerializeField]
        private float sensitivity = 1f;
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;
        private Canvas canvas;

        public bool smooth;
        public float smoothTime = 5f;

        public Vector2 TouchDist { get; private set; }
        private PointerEventData pointerEventData { get; set; }
        private Vector2 PointerOld { get; set; }
        
        public bool Pressed { get; private set; }

        public float Sensitivity { get  => sensitivity; } 

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            if (pointerEventData != null)
            {
                if (Pressed)
                {
                    Vector2 touchPos = pointerEventData.position;

                    TouchDist = touchPos - PointerOld;
                    PointerOld = touchPos;
                }

            } else
            {
                TouchDist = Vector2.zero;
            }

            SendValueToControl(TouchDist.normalized);
        }

        /// <summary>
        /// Method is executed when pointer starts pressing.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            PointerOld = eventData.position;
            pointerEventData = eventData;
        }

        /// <summary>
        /// Method is executed when pointer (touch) is no longer being pressed.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
            TouchDist = Vector2.zero;
        }

    }
}
