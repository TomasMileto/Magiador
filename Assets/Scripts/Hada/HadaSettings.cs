using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hada", menuName ="Hada", order =2)]
public class HadaSettings : ScriptableObject {

    [Header("HADA")]

    //public float vida=100;
    public float oneShotDamage=1;
    public GameObject modelo;

    [Header("ELEMENTOS FX")]

    public GameObject fireFX;
    public GameObject airFX;
    public GameObject waterFX;
    public GameObject earthFX;

    [Header("MOVIMIENTO")]
    public float hadaMaxVel = 125;
    public float fuerzaAtaque = 250;
    public float regresoSmooth = 12;

    [Header("IDLE")]

    //public Transform targetIdle;

    public float distanciaMax;

    public float smoothing;

    //[Header("ORBITA")]

    //public Transform targetOrbita;

    //public Transform targetAtaque;

    //public float velocidadOrbita = 3;

    //public float radioOrbita=5;

    //public float aumentoConsumoOrbita=2;

    //public float damageOrbita=30;


}
