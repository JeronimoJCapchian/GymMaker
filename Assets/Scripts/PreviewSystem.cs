using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] float previewYOffset = 0.06f;

    [SerializeField] GameObject cellIndicator;
    [SerializeField] GameObject previewObject;
    public GameObject PreviewObject => previewObject;

    [SerializeField] Material previewMaterialPrefab;
    Material previewMaterialInstance;

    Renderer cellIndicatorRenderer;

    public bool isRotated;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (var i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    public void RotateObject()
    {
        Debug.Log("Roto Roto");
        var previousSize = cellIndicator.transform.localScale;
        cellIndicator.transform.localScale = new(previousSize.z, 1, previousSize.x);

        if (previewObject.transform.eulerAngles != Vector3.zero)
        {
            Debug.Log("No roto");
            previewObject.transform.eulerAngles = Vector3.zero;
            previewObject.transform.position = new(previewObject.transform.position.x,
                                                   previewObject.transform.position.y,
                                                   previewObject.transform.position.z - 1);
        }
        else
        {
            Debug.Log("Si roto");
            previewObject.transform.Rotate(new(0f, 90f, 0f));
            previewObject.transform.position = new(previewObject.transform.position.x,
                                                   previewObject.transform.position.y,
                                                   previewObject.transform.position.z + 1);
        }

    }

    bool pr;

    private void Update()
    {
        if (previewObject != null) pr = previewObject.GetComponent<TriggeringValidate>().validity;
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        StartCoroutine(CO(validity));
    }

    IEnumerator CO(bool validity)
    {
        yield return new WaitForSeconds(0.025f);
        Debug.Log(validity + " " + pr);
        Color c = validity && pr ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        StartCoroutine(CO2(validity));
    }

    IEnumerator CO2(bool validity)
    {
        yield return new WaitForSeconds(0.025f);
        Debug.Log(validity + " " + pr);
        Color c = validity && pr ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYOffset,
            position.z);
    }
}
