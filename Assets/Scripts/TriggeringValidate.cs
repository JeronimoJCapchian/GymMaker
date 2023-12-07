using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TriggeringValidate : MonoBehaviour
{
    public bool validity = true;

    public LayerMask layerMask;

    public float rayLenghtForward = 2.5f;
    public float rayLenghtRight = 2.5f;

    [HideInInspector] public Transform centerTransform;

    public bool isRotated = false;

    private void Awake()
    {
        centerTransform = transform.GetChild(0);
    }

    public void RotateCenter()
    {
        centerTransform.eulerAngles += new Vector3(0, 90, 0);

        var xValue = centerTransform.localPosition.x;
        var zValue = centerTransform.localPosition.z;

        centerTransform.localPosition = new Vector3(zValue, 0, xValue);

        var change = rayLenghtForward;
        rayLenghtForward = rayLenghtRight;
        rayLenghtRight = change;

        isRotated = !isRotated;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(centerTransform.position, transform.forward, rayLenghtForward, layerMask)) validity = false;
        else if (Physics.Raycast(centerTransform.position, transform.right, rayLenghtRight, layerMask)) validity = false;
        else if (Physics.Raycast(centerTransform.position, -transform.forward, rayLenghtForward, layerMask)) validity = false;
        else if (Physics.Raycast(centerTransform.position, -transform.right, rayLenghtRight, layerMask)) validity = false;
        else validity = true;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(centerTransform.position, transform.forward * rayLenghtForward, Color.red);
        Debug.DrawRay(centerTransform.position, transform.right * rayLenghtRight, Color.red);
        Debug.DrawRay(centerTransform.position, -transform.forward * rayLenghtForward, Color.red);
        Debug.DrawRay(centerTransform.position, -transform.right * rayLenghtRight, Color.red);
    }
}
