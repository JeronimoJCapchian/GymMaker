using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneLoader : MonoBehaviour
{
    [SerializeField] private Transform loadingIcon;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private int levelToLoad = 2;
    private void Start()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(levelToLoad);
        op.allowSceneActivation = true;
    }

    private void Update()
    {
        loadingIcon.Rotate(Vector3.forward, -45f * rotationSpeed * Time.deltaTime);
    }
}
