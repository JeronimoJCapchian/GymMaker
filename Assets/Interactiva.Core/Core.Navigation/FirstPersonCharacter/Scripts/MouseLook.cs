using System;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace Interactiva.Core.Navigation.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        [NonSerialized] public float XSensitivity = 2f;
        [NonSerialized] public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;


        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool isEnabled = true;

        float yRot = 0;
        float xRot = 0;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public void SetRotation(Quaternion rot) {
            Vector3 eulerAngles = rot.eulerAngles;
            m_CharacterTargetRot = Quaternion.Euler(0, eulerAngles.y, 0);
            m_CameraTargetRot = Quaternion.Euler(-eulerAngles.x, 0, 0);
        }
        public void LookRotation(Transform character, Transform camera, FixedTouchField field)
        {
            if (!isEnabled) //If we are not enabled, stop any further functionality
            {
                return;
            }
            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);
            if (NavigationManager.GetInputMode() == NavigationManager.InputMode.Alternate)
            {
                const float upAngle = 4f;
                m_CameraTargetRot = Quaternion.Slerp(camera.localRotation, Quaternion.Euler(-upAngle, 0, 0), smoothTime * Time.deltaTime);
            }
            if (clampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }

#if (UNITY_STANDALONE || UNITY_WEBGL)
            if (smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
#else
            if (field.smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    field.smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    field.smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
#endif
        }

        public void GetMouseInput(Vector2 input)
        {
            if (!isEnabled)
            {
                return;
            }
            xRot = input.y * YSensitivity;
            yRot = input.x * XSensitivity;
        }

        public void SetEnabledState(bool value)
        {
            xRot = 0;
            yRot = 0;
            isEnabled = value;
        }

        

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
