using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {

    public float orbitalSpeed;

    public bool canRotate = true;

    //public float distancia;

    public float velTraslacion=5;

    public float velRotacion = 20;

    public float smooth=2;

    

    float rotacion;

    void Update()
    {
        if(canRotate)
            Rotate();


        if (UIManager.Instance.presionoStart)
        {
            canRotate = false;
        }
    }

    void Rotate()
    {
        rotacion += orbitalSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(20, rotacion, 0);


    }

    Vector3 curVelocity;

    public IEnumerator TravelTo(Transform target)
    {
        Quaternion targetRot = target.rotation;

        Vector3 targetPos = target.position;



        transform.position = targetPos;

        transform.rotation = targetRot;
        yield return null;
    }
    
}
