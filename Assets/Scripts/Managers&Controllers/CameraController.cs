using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    Vector3 target;

    public float tiempoDamp = 0.2f;
    public float tiempoZoom = 0.3f;

    private Vector3 velMovimiento;

    public float distMin = 6.5f;
    private float velZoom;

    public float minTamanioCamara = 3.5f;
    public float extraTamanioCamara = 2;
    public float dist2Tamanio = 1;

    int numeroCamera;

    Camera camara;



    private void Start()
    {
        camara = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        camara.orthographicSize = Mathf.Clamp(camara.orthographicSize, 5, 25);
    }
    private void FixedUpdate()
    {
        target = Move();

        Zoom(target);
    }


    private Vector3 Move()
    {
        Vector3 promedio = Vector3.zero;

        Vector3 objetivo = BuscarPosicionObjetivo();

        transform.position = Vector3.SmoothDamp(transform.position, objetivo, ref velMovimiento, tiempoDamp);

        return objetivo;
    } //Mover la camara fluidamente

    private Vector3 BuscarPosicionObjetivo()
    {
        Vector3 promedio = Vector3.zero;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int p = 0; p < players.Length; p++)
        {
            promedio += players[p].transform.position;
        }
        if (players.Length > 0)
        {
            promedio /= players.Length;
        }

        promedio.y = transform.position.y;

        return promedio;
    } //Buscar promedio entre distancias de los players totales al target de la camara




    private void Zoom(Vector3 objetivo)
    {
        float tamanioObjetivo = BuscarTamanioObjetivo(objetivo);
        camara.orthographicSize = Mathf.SmoothDamp(camara.orthographicSize,
            tamanioObjetivo, ref velZoom, tiempoZoom);
    }

    private float BuscarTamanioObjetivo(Vector3 objetivo)
    {
        float distMax = DistanciaMaxima(objetivo);

        float zoomObjetivo = distMax * dist2Tamanio + extraTamanioCamara;

        zoomObjetivo = Mathf.Max(zoomObjetivo, minTamanioCamara);

        return zoomObjetivo;
    }

    private float DistanciaMaxima(Vector3 objetivo) //Valor mas grande
    {
        float distMax = -1;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
            return 0;

        for (int i = 0; i < players.Length; i++)
        {
            float currentDistance = (objetivo - players[i].transform.position).magnitude;

            if (currentDistance > distMax)
            {
                distMax = currentDistance;

            }

        }

        return distMax;
    }
}
