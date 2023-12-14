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

    Dictionary<int, string> myDicScenes = new Dictionary<int, string>();

    CinemachineVirtualCamera currentCam;

    [SerializeField] List<Button> buttons;

    [SerializeField] LevelManager lManager;

    private void Awake() {
        currentCam = cameras[0];

        myDicScenes.Add(0, "Gym 2");
        myDicScenes.Add(1, "Gym 3");
        myDicScenes.Add(2, "Gym 4");
        myDicScenes.Add(3, "Gym 5");
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

    public void OnSelectLevel()
    {
        if(myDicScenes.ContainsKey(currentIndex)) lManager.OnChangeScene(myDicScenes[currentIndex]);
    }
}
