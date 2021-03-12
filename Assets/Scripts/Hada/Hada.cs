using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hada : MonoBehaviour {

   

    [Header("Vida")]
    public float vida=100;
    public int numeroHada;

    [Header("Damage")]
    public float oneShotDamage=30;
    //public float dps;
    float empuje = 20;

    [Header("Estados")]
    public GameObject fireFX;
    public GameObject airFX;
    public GameObject waterfx;
    public GameObject earthFX;

    
    Coroutine elemento;

    SoundManager sound;

    public Player player;


    Rigidbody rb;

    public Elementos elementoActual;
    
    


    public enum Elementos { None, Fire, Air, Water, Earth };


    private void Update()
    {
        //if (player != null)
        //    transform.position = player.transform.position + new Vector3(0, 3, 0);
    }

    void OnEnable () {
        rb = GetComponent<Rigidbody>();
        player = GetComponentInParent<Player>();


        //StartCoroutine(RutinaEstados());
	}

    private void Start()
    {
        sound = SoundManager.Instance;   
    }

    //IEnumerator RutinaEstados()
    //{

    //    while (true) { 

    //        switch (elementoActual)
    //        {

    //            case Elementos.Fire:

    //                if (player.fireMana > 0)
    //                    yield return elemento = StartCoroutine(FireRoutine());
    //                else
    //                    yield return new WaitForEndOfFrame();


    //                break;

    //            case Elementos.Air:

    //                if (player.airMana > 0)
    //                    yield return elemento = StartCoroutine(AirRoutine());
    //                else
    //                    yield return new WaitForEndOfFrame();


    //                break;

    //            case Elementos.Water:

    //                if (player.waterMana > 0)
    //                    yield return elemento = StartCoroutine(WaterRoutine());
    //                else
    //                    yield return new WaitForEndOfFrame();


    //                break;

    //            case Elementos.Earth:

    //                if(player.earthMana>0)
    //                    yield return elemento=StartCoroutine(EarthRoutine());
    //                else
    //                    yield return new WaitForEndOfFrame();
    //                break;

    //            default:
    //                //rb.mass = 0.3f;
    //                yield return elemento = StartCoroutine(None());
    //                break;

    //        }

    //    } //Cambiar entre estados elementales
    //}



    IEnumerator FireRoutine()
    {
        //Caracteristicas
        fireFX.SetActive(true);
        


        float fireExpend;
        do{
            fireExpend = 0;

           
            player.fireMana -= fireExpend/10;
            yield return new WaitForSeconds(0.1f);

        } while (elementoActual == Elementos.Fire && player.fireMana >0); //Gasto de Mana y determinar si puede hacer daño

        fireFX.SetActive(false);
    }

    IEnumerator AirRoutine()
    {   
        //Caracteristicas
        airFX.SetActive(true);
        oneShotDamage = 0.8f;
        empuje = 250;

        
        rb.mass = 0.13f;

        do{

            

            player.airMana -= player.airExpend/10;
            yield return new WaitForSeconds(0.1f);

        } while (elementoActual == Elementos.Air && player.airMana>0); //Gasto de Mana y determinar si puede hacer daño


        airFX.SetActive(false);

    }

    IEnumerator WaterRoutine()
    {   
        //Caracteristicas
        waterfx.SetActive(true);
        oneShotDamage = 0.5f;
        empuje = 50;

        
        rb.mass = 0.35f;
        

        do{

            

            player.waterMana -= player.waterExpend/10;
            yield return new WaitForSeconds(0.1f);

        } while (elementoActual == Elementos.Water && player.waterMana>0); //Gasto de Mana y dterminar si puede hacer daño

        waterfx.SetActive(false);

    }

    IEnumerator EarthRoutine()
    {
        
        earthFX.SetActive(true);
        oneShotDamage = 2f;
        empuje = 125;

        
        rb.mass = 0.65f;


        do{


            player.earthMana -= player.earthExpend/10;
            yield return new WaitForSeconds(0.1f);

        } while (elementoActual == Elementos.Earth && player.earthMana>0); //Gasto de Mana y determinar si puede hacer daño

        earthFX.SetActive(false);


    }

    IEnumerator None()
    {
        //fromMass = rb.mass;
        //toMass = 0.3f;

        //while (rb.mass != 0.3f)
        //{
        //    rb.mass = Mathf.SmoothDamp(fromMass, toMass, ref curVelocity, 0.1f);

        //    yield return new WaitForEndOfFrame();
        //}


        yield return null;
    }

    

    }


