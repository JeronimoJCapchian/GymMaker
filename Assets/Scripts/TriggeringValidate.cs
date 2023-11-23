using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeringValidate : MonoBehaviour
{
    public bool validity = true;

    public LayerMask layerMask;

    public float rayLenghtForward = 2.5f;
    public float rayLenghtRight = 2.5f;
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, rayLenghtForward, layerMask)) validity = false;
        else if (Physics.Raycast(transform.position, transform.right, rayLenghtRight, layerMask)) validity = false;
        else validity = true;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * rayLenghtForward, Color.red);
        Debug.DrawRay(transform.position, transform.right * rayLenghtRight, Color.red);
    }
}
