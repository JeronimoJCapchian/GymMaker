using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Transform mirror;
    //public Transform player;

    void Start()
    {

    }


    void Update()
    {
        Quaternion direccion = Quaternion.Inverse(transform.rotation) * (Camera.main.transform.rotation);

        mirror.transform.localEulerAngles = new Vector3(direccion.eulerAngles.x, -direccion.eulerAngles.y, direccion.eulerAngles.z);

        Vector3 distancia = transform.InverseTransformPoint(Camera.main.transform.position);

        mirror.transform.localPosition = new Vector3(distancia.x, mirror.transform.localPosition.y, distancia.z);





        //Vector3 reflectedPosition = mirror.position;
        //reflectedPosition.y = -transform.position.y + 2 * mirror.position.y;
        //transform.position = reflectedPosition;

        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mirror.eulerAngles.y, transform.rotation.eulerAngles.z);





        /* Vector3 playerOffset = player.position - mirror.position;
         transform.position = mirror.position + playerOffset;

         float angularDifference = Quaternion.Angle(mirror.rotation, player.rotation);

         Quaternion rotationDifference = Quaternion.AngleAxis(angularDifference, Vector3.up);
         Vector3 newCamera = rotationDifference * player.forward;
         transform.rotation = Quaternion.LookRotation(newCamera, Vector3.up);*/

    } 


}
