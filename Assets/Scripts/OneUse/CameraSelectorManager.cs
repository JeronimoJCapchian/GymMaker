using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraSelectorManager : MonoBehaviour
{
    [SerializeField] List<CinemachineVirtualCamera> cameras;

    [SerializeField] UnityEvent onLimitLeft;
    [SerializeField] UnityEvent onLimitRight;

    int currentIndex = 0;
    CinemachineVirtualCamera currentCam;

    [SerializeField] List<Button> buttons;

    private void Awake() {
        currentCam = cameras[0];
    }

    public void ChangeCameras(int value)
    {
        currentIndex += value;

        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }

        if (currentIndex < 0 || currentIndex > cameras.Count -1) return;

        if(currentIndex == 0) onLimitLeft?.Invoke();
        if(currentIndex == cameras.Count -1) onLimitRight?.Invoke();

        currentCam.gameObject.SetActive(false);
        cameras[currentIndex].gameObject.SetActive(true);
        currentCam = cameras[currentIndex];
    }
}
