using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Personaje", menuName = "Personaje", order = 1)]
public class CharacterSettings :ScriptableObject  {

    public string nombre;

    [TextArea]
    public string descripcion;

    public Sprite Imagen;

    public GameObject personaje;

    public ParticleSystem resurrection;

    public Animator animator;

    public HadaSettings hada;



    [Header(" ")]
    [Header("                                   Vida & Mana                      ")]
    [Header(" ")]


    [Range(0, 300)]
    public float vida = 300;

    float maxHealth;

    [Header("Fuego")]
    [Range(0, 100)]
    public float fireMana;
    public float fireExpend = 1f;
    public float fireDPS;
    public ParticleSystem explosion;
    public float fuerzaExplosion = 100;
    public float radioExplosion = 5;
    public float fireRegen = 1;

    [Header("Aire")]
    [Range(0, 100)]
    public float airMana;
    public float multiplicadorSpint=5000;
    public float airExpend = 1f;
    public float runningAirRegen = 0.3f;

    [Header("Agua")]
    [Range(0, 100)]
    public float waterMana;
    public float waterExpend = 1f;
    public float curacion = 40;
    public float healChanneling = 1.5f;
    public float waterRegen = 1;

    [Header("Tierra")]
    [Range(0, 100)]
    public float earthMana;
    public float earthExpend = 1f;
    public GameObject earthWall;
    public float earthRegen=1;


    [Header(" ")]
    [Header("                                     Movimiento  \n                      ")]
    [Header(" ")]
    public float fuerzaMovimiento=100;
    public float velocidadRotacion=3;
    public float velocidadMaxima=40;
    public float masa = 1.5f;
    public float drag = 5;

   

}
