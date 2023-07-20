using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    InputManager inputManager;

    #region Camera Configuration
    [Serializable]
    internal class CameraConfiguration
    {
        public float movementSpeed;
        public float rotationSpeed;
        public float elevationSpeed;
    }
    /// <summary>Variable de la configuracion de la camara</summary>
    [SerializeField] CameraConfiguration cameraConfig;
    #endregion
    Vector3 moveDirection;
    Vector3 rotationDirection;

    private void OnEnable() => inputManager = gameManager.inputManager;

    public void HandleMovements()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        moveDirection = ((transform.forward * inputManager.verticalAxi) + (transform.right * inputManager.horizontalAxi)) * cameraConfig.movementSpeed;
        transform.Translate(moveDirection, Space.World);
    }

    void HandleRotation()
    {
        //Girar a la derecha
        if (inputManager.turnRight == 1)
            rotationDirection = new Vector3(0f, transform.rotation.y + cameraConfig.rotationSpeed, 0f);

        //Girar a la izquierda
        if (inputManager.turnLeft == 1)
            rotationDirection = new Vector3(0f, transform.rotation.y - cameraConfig.rotationSpeed, 0f);
        
        //No Girar
        if (inputManager.turnLeft == 0 && inputManager.turnRight == 0)
            rotationDirection = new Vector3(0f, 0f, 0f);


        transform.Rotate(rotationDirection, Space.Self);
    }

    // public void HandleElevation()
    // {

    // }
}
