using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class extends built-in Unity classes with additional methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Returs whether the given point is within the camera's viewport.
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool IsInsideViewport(this Camera cam, Vector3 point)
    {
        Vector3 viewport = cam.WorldToViewportPoint(point);
        if (viewport.x < 0 || viewport.x > 1)
        {
            return false;
        }
        if (viewport.y < 0 || viewport.y > 1)
        {
            return false;
        }
        if (viewport.z < 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Instantiates a game object in front of the camera, randomly.
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="go"></param>
    public static void InstantiateInFrontRandomly(this Camera cam, GameObject go)
    {
        const float offsetZ = 1f;
        Vector3 viewportPoint = new Vector3(Random.Range(02f, 0.8f), Random.Range(0.2f, 0.8f), offsetZ);
        Vector3 worldPoint = cam.ViewportToWorldPoint(viewportPoint);
        Object.Instantiate(go, worldPoint, Quaternion.identity);
    }
}
