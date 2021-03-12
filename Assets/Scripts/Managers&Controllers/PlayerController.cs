using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    delegate IEnumerator ElementOverload();
    ElementOverload elementOverload;

    delegate IEnumerator Utility();
    Utility utility;

    Player player;

    SoundManager sound;

    ParticleSystem explosionFX;

    ParticleSystem healingFX;

    PlayerInput input;

    CharacterManager pj;

    Transform personajeTransform;

    [HideInInspector]
    public float recuperandoUtility=0;
    [HideInInspector]
    public float cooldownUtility;

    [HideInInspector]
    public float recuperandoEO=100;
    [HideInInspector]
    public float cooldownEO = 0;    


    Hada hada;

    float dashCarga;

    bool puedeExplotar=true;
    bool puedeSprintear=true;
    bool puedeCrearMuro=true;
    bool puedeHeal = true;
    bool utilityDisponible=true;

    public bool EOready=true;

    public bool UtilityReady = true;

    private void Start()
    {
        player = GetComponent<Player>();
        input = GetComponent<PlayerInput>();
        hada = GetComponentInChildren<Hada>();

        explosionFX = player.explosion;
        healingFX = player.waterHealing;

        pj = CharacterManager.Instance;

        personajeTransform= player.transform.Find("Personaje").transform;

        sound = SoundManager.Instance;

    }

    private void Update()
    {
        switch (player.personaje) 
        {
            case "Bric":
                elementOverload = BricEO;
                cooldownEO = pj.bric.recuperacioEO;

                utility = BricUtility;
                cooldownUtility = pj.bric.recuperacionUtility;
                
                break;

            case "Stuarto":
                elementOverload = StuartoEO;
                cooldownEO = pj.stuarto.recuperacionEO;

                utility = StuartoUtility;
                cooldownUtility = pj.stuarto.recuperacionUtility;
                break;

            case "Malmon":
                elementOverload = MalmonEO;
                utility = MalmonUtility;
                break;

            case "Okki":
                elementOverload = OkkiEO;
                cooldownEO = pj.okki.recuperacionEO;

                utility = OkkiUtility;
                cooldownUtility = pj.okki.recuperacionUtility;
                break;
        }//Cambiar habilidades segun personaje, si lo dejo en el update es porque me olvide de cambiarlo xd

        if (input.leerInput == false) return;

        if (Input.GetButtonDown(input.Ultimate))
        {
            if (elementOverload != null)
            {
                if (EOready)
                {
                    StartCoroutine(elementOverload());
                    recuperandoEO = 0;
                    EOready = false;
                }
            }
            else print("Element Overload nulo");
        }

       
    }

    Vector3 aMirar;
    

    public void MovimientoAnalogico(float h, float j)
    {
       
        if (h * player.rb.velocity.x < player.velocidadMaxima)
        {
            player.rb.AddForce(PlayerManager.Instance.right * h * player.fuerzaMovimiento);
        }
        if (j * player.rb.velocity.z < player.velocidadMaxima)
        {
            player.rb.AddForce(PlayerManager.Instance.forward * j * player.fuerzaMovimiento);

        }

        if (h != 0 || j != 0)
            aMirar = Vector3.Normalize(PlayerManager.Instance.right * h + PlayerManager.Instance.forward * j);

        if (aMirar != new Vector3(0, 0, 0) && player.puedeRotar)
            player.transform.forward = aMirar; //Constantemente redefino rotacion

        if (Mathf.Abs(h) > 0.75f || Mathf.Abs(j) > 0.75f)
        {
            player.inGameAnimator.SetBool("Running", true);

        }
        else if (Mathf.Abs(h) > 0.2f || Mathf.Abs(j) > 0.2f)
        {
            player.inGameAnimator.SetBool("Walking", true);
            player.inGameAnimator.SetBool("Running", false);
        }
        else if (Mathf.Abs(h) < 0.15f && Mathf.Abs(j) < 0.5f)
        {
            player.inGameAnimator.SetBool("Walking", false);
            player.inGameAnimator.SetBool("Running", false);
        }
    }

    public void SprintFunc()
    {
        if (player.airMana > player.airExpend * 5 && puedeSprintear && player.sprintHabilitado)
            StartCoroutine(Sprint());
    }

    public void EarthWallFunc()
    {
        if (player.earthMana > player.earthExpend * 30 && puedeCrearMuro)
            StartCoroutine(EarthWall());
    }

    public void HealFunc()
    {
        if (player.waterMana > player.waterExpend * 25 && puedeHeal)
            StartCoroutine(Heal());
    }

    public void ExplosionFunc()
    {

        if (player.fireMana > player.fireExpend * 20 && puedeExplotar)
            StartCoroutine(Explosion());
    }
    public void FuncionUtilidad()
    {
        //switch (hada.elementoActual)
        //{
        //    case Hada.Elementos.Earth:
                
        //        break;

        //    case Hada.Elementos.Air:

        //        break;

        //    case Hada.Elementos.Water:
                
        //        break;

        //    case Hada.Elementos.Fire:
        //        break;

        //    case Hada.Elementos.None:

                if (utilityDisponible)
                {
                    StartCoroutine(utility());
                }
        //        break;

        //}  por el momento no voy a hacer lo de los elementos del hada
    }

    

    IEnumerator EarthWall()
    {
        puedeCrearMuro = false;

        Vector3 spawn = player.transform.position;
        spawn.y -= 1.3f;
        GameObject wall = Instantiate(player.earthWall, spawn, player.transform.rotation);
        player.earthMana -= player.earthExpend * 30;
        float manaGastado = player.earthExpend * 30;

        sound.Reproducir(sound.magic.earthWall);

        yield return new WaitForSeconds(10.3f);

        if (Vector3.Distance(player.transform.position, wall.transform.position) < 8)
        {

            player.earthMana += manaGastado / 2; ;
        }

        sound.Reproducir(sound.magic.earthWallOff);

        yield return new WaitForSeconds(0.8f);


        Destroy(wall);

        puedeCrearMuro = true;

    }

    IEnumerator Sprint()
    {
        puedeSprintear = false;

        float fuerzaMovimientoOriginal = player.fuerzaMovimiento;

        dashCarga = Mathf.Clamp(dashCarga, 0, player.multiplicadorSprint + player.multiplicadorSprint / 2);

        player.fuerzaMovimiento = fuerzaMovimientoOriginal * player.multiplicadorSprint;

        while ( input.r>0.45f && player.airMana > player.airExpend * 5)
        {

            player.airMana -= player.airExpend *5;

            yield return new WaitForSeconds(0.5f);

        }

        player.fuerzaMovimiento = fuerzaMovimientoOriginal;

        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(0.6f);
        puedeSprintear = true;

    }

    IEnumerator Heal()
    {
        puedeHeal = false;
        player.inGameAnimator.SetBool("Healing", true);

        player.inGameAnimator.SetTrigger("Heal");

        StartCoroutine(player.DeshabilitarMovimiento());

        

        float count = 0;

        while (input.l >0.45f)
        {

            count += 0.25f;

            if (count % player.healChanneling == 0)
            {
                player.vida += player.curacion;

                ParticleSystem healing = Instantiate(healingFX, this.transform);
                Destroy(healing, healing.main.duration + 0.2f);
                sound.Reproducir(sound.magic.heal);

                player.waterMana -= player.waterExpend * 25;

            }
            yield return new WaitForSeconds(0.25f);

        }

        player.inGameAnimator.SetBool("Healing", false);

        personajeTransform = player.transform.Find("Personaje").transform;

        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform, suelo: true);

        StartCoroutine(player.HabilitarMovimiento(0.25f));

        puedeHeal = true;

    }


    IEnumerator Explosion()
    {
        

        player.inGameAnimator.SetTrigger("Explode");

        StartCoroutine(player.DeshabilitarMovimiento());

        player.fireMana -= player.fireExpend * 20;

        yield return new WaitForSeconds(0.52f);

        StartCoroutine(ExplodeFX());

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, player.radioExplosion);

        foreach (Collider objetoCercano in colliders)
        {
            Rigidbody otherRB = objetoCercano.GetComponent<Rigidbody>();

            if (otherRB != null && otherRB.name != "Player" + player.numeroJugador && otherRB.name != "Hada" + player.numeroJugador)
            {
                otherRB.AddExplosionForce(player.fuerzaExplosion, transform.position, player.radioExplosion);

                switch (otherRB.tag)  
                {
                    case "Player":

                        Player otherPlayer;

                        otherPlayer = otherRB.gameObject.GetComponent<Player>();

                        int i = Random.Range(20, 50);

                        float damage = player.fireDPS *i;

                        StartCoroutine(otherPlayer.TakeDamage(damage, player.name));

                        if (otherPlayer.isDead)
                            player.killScore++;

                        otherPlayer.fireMana += 15;

                        break;

                   

                    default:

                        break;
                } //Cambiar daño segun collider

            }

        }



        yield return null;
    } 

    IEnumerator ExplodeFX()
    {
        puedeExplotar = false;

        ParticleSystem explosion = Instantiate(explosionFX, this.transform);

        sound.Reproducir(sound.magic.explosion);

        if (explosion == null) yield return null;

        explosion.transform.parent = null;

        explosion.Play(true);

        yield return new WaitForSeconds(explosion.main.duration);

        StartCoroutine(player.HabilitarMovimiento());

        Destroy(explosion);

        personajeTransform = player.transform.Find("Personaje").transform;

        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform, suelo: true);

        puedeExplotar = true;

    }


    

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////   PODERES     PERSONAJES    //////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////BRIC////////////////////////
    ///
    IEnumerator BricEO()
    {

        Collider[] colliders=Physics.OverlapSphere(transform.position, pj.bric.radio);

        ParticleSystem exanimum = Instantiate(pj.bric.exanimum, player.transform);

        StartCoroutine(player.DeshabilitarMovimiento());

        player.inGameAnimator.SetTrigger("ElementOverload");

        recuperandoEO = 0;

        Destroy(exanimum, exanimum.main.duration);

        float timer = 0;


        while (timer < 3.5f)
        {
            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }//Veo de ralentizar a los players dentro del area despues

        foreach (Collider collider in colliders)  
        {
            Player otherPlayer = collider.GetComponent<Player>();

            if (otherPlayer == null ) continue;
            if (otherPlayer == player) continue;

            float damage = DamageSegunDistancia(transform.position, otherPlayer, pj.bric.radio, pj.bric.maxDamage, minimo: 1.3f);
            if (otherPlayer.isDead)
                player.killScore++;

            StartCoroutine(otherPlayer.TakeDamage(damage, player.name));

            ParticleSystem darkRelease = Instantiate(pj.bric.darkRelease, otherPlayer.transform);

                GameObject.Destroy(darkRelease.gameObject, darkRelease.main.duration+0.25f);

            GameObject lifeParticle=Instantiate(pj.bric.lifeParticle, otherPlayer.transform);
                lifeParticle.transform.localPosition = Vector3.zero;

                StartCoroutine(pj.bric.moveLifeParticle(lifeParticle, player.transform.position));

            

            yield return new WaitForSeconds(0.2f);

            float curacion = damage / 2;

            if (otherPlayer.isDead)
                curacion *= 2;

            StartCoroutine(player.Heal(curacion, rapidez: 150));


        } //Damage segun distancia al centro del circulo e instanciar efectos de robo de vida.

        StartCoroutine(player.HabilitarMovimiento());

        while (recuperandoEO < cooldownEO)
        {
            recuperandoEO += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        EOready = true;

        yield return null;
    }

    IEnumerator BricUtility()
    {
        StartCoroutine(player.DeshabilitarMovimiento(1.1f,delayInicial: 0));
        utilityDisponible = false;

        player.inGameAnimator.SetTrigger("Utility");
        yield return new WaitForSeconds(1);

        GameObject bats = Instantiate(pj.bric.batCloud, transform.position+transform.forward*3.5f, transform.rotation);

        bats.GetComponent<BatCloud>().player = player;

        while (recuperandoUtility < cooldownUtility)
        {
            recuperandoUtility += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        utilityDisponible = true;

        recuperandoUtility = 0;
        yield return null;
    }

    //////////////////////////////MALMON////////////////////////    
    ///
    IEnumerator MalmonEO()
    {
        Vector3 posSpawn = player.transform.position + player.transform.forward*pj.malmon.distanciaSpawnDemon;

        Quaternion orientacion = Quaternion.Euler(player.transform.forward);

        ParticleSystem fireColumn = Instantiate(pj.malmon.fireColumn, posSpawn, Quaternion.Euler(new Vector3(-90,0,0)));
        Destroy(fireColumn, fireColumn.main.duration * 1.25f);

        yield return new WaitForSeconds(fireColumn.main.duration * 0.5f);

        GameObject fireDemon = Instantiate(pj.malmon.demon, posSpawn, orientacion);

        FireDemonController demonController = fireDemon.GetComponent<FireDemonController>();

        demonController.player = this.player;

        yield return null;
    }

    IEnumerator MalmonUtility()
    {
        yield return null;
    }

    ////////////////////////////////STUARTO///////////////////////
    ///
    bool aterrizo = true;

    IEnumerator StuartoEO()
    {

        //ENTRADA A LA TIERRA
        //
        player.inmune = true;

        player.inGameAnimator.SetTrigger("ElementOverload");

        yield return new WaitForSeconds(0.75f); //Duracion de la animacion +o-

        personajeTransform = player.transform.Find("Personaje").transform;


        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform, suelo:true); //Adaptar posicion del personaje a la animaicon

        yield return new WaitForEndOfFrame();

       

        GameObject entrada=Instantiate(pj.stuarto.entradaTierra, transform.position, Quaternion.Euler(-90, 0, 0));
        Destroy(entrada, 1f);

        ParticleSystem tunel = Instantiate(pj.stuarto.permanecerTierra, player.transform.position, Quaternion.Euler(0, 90, 0), player.transform);

        Quaternion relativeRotation = tunel.transform.localRotation; //Para que no gire con el padre


        float time = 0;

        //PERMANECER EN TIERRA
        //
        while( time<= pj.stuarto.maxTime && (!Input.GetButton(input.Ultimate) || time<=pj.stuarto.minTime)) //Para que permanezca min un tiempo
        {
            tunel.transform.rotation = relativeRotation;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //COMIENZA SALIDA
        //
        tunel.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Destroy(tunel.gameObject, tunel.main.duration);


        GameObject salida = Instantiate(pj.stuarto.salirTierra, player.transform.position, Quaternion.Euler(90, 0, 0));

        float duracion=salida.GetComponent<ParticleSystem>().main.duration;

        StartCoroutine(player.DeshabilitarMovimiento());

        yield return new WaitForSeconds(duracion);

        //SALIDA
        //

        StuartoEOBurst();

        player.rb.AddForce(player.transform.up * pj.stuarto.fuerzaSalto, ForceMode.VelocityChange);
        player.inGameAnimator.SetTrigger("ElementOverload2");
        Destroy(salida, 10f);

        yield return new WaitForSeconds(1);
        aterrizo = false;

        //CAYENDO 

        while (aterrizo == false)
        {
            transform.position -= new Vector3(0, 20 * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }

        player.inmune = false;

        player.inGameAnimator.SetTrigger("Land");

        StartCoroutine(player.HabilitarMovimiento(1f));

        yield return new WaitForSeconds(1f);//espero a la animacion

        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform);

        while (recuperandoEO < cooldownEO)
        {
            recuperandoEO += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        

        EOready = true;

        yield return null;
    }

    
    void StuartoEOBurst()
    {
        Collider[] colliders=Physics.OverlapSphere(player.transform.position, pj.stuarto.radioSalida);

        foreach (Collider collider in colliders)
        {
            Rigidbody otherRB = collider.GetComponent<Rigidbody>();

            if (otherRB == null) continue;

            otherRB.AddExplosionForce(pj.stuarto.fuerzaBurstOut, player.transform.position, pj.stuarto.radioSalida);

            Player otherPlayer = collider.gameObject.GetComponent<Player>();

            if (otherPlayer == null || otherPlayer==player) continue;

            float damage = DamageSegunDistancia(transform.position, otherPlayer, pj.stuarto.radioSalida, pj.stuarto.maxDamage, minimo: 1.1f);

            StartCoroutine(otherPlayer.TakeDamage(damage, player.name));
            if (otherPlayer.isDead)
                player.killScore++;
        }

    }

    IEnumerator StuartoUtility()
    {
        utilityDisponible = false;
        personajeTransform = player.transform.Find("Personaje").transform;

        player.inGameAnimator.SetTrigger("Utility");

        float time = 0;


        player.fuerzaMovimiento += pj.stuarto.fuerzaRoll;
        while (time <= 0.8f)
        {
            player.transform.position = CorregirFuckingAnimacion(ref personajeTransform, suelo: true); //Adaptar posicion del personaje a la animaicon
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        player.fuerzaMovimiento -= pj.stuarto.fuerzaRoll;

        //yield return new WaitForSeconds(pj.stuarto.recuperacionRoll); //Lo cambio por un while para llevar cuenta del progreso
                                                                        //de la recuperacion
        while(recuperandoUtility< cooldownUtility)
        {
            recuperandoUtility += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        recuperandoUtility = 0;

        utilityDisponible = true;

        yield return null;
    }

    /////////////////////////////////OKKI////////////////////////
    ///
    IEnumerator OkkiEO()
    {
        player.inGameAnimator.SetTrigger("ElementOverload");

        //HadaMover hadaMov = hada.GetComponent<HadaMover>();

        //yield return(StartCoroutine(hadaMov.Regresar(player.transform.position, 0.5f, HadaMover.Estados.especial)));

        personajeTransform = player.transform.Find("Personaje").transform;

        //hadaMov.gameObject.SetActive(false);


        yield return new WaitForEndOfFrame();

        personajeTransform.Find("RigPelvis").Find("Shield").GetComponent<MeshRenderer>().enabled = true;

        float time = 0;

            StartCoroutine(player.DeshabilitarRotacion());

        float storedMaxVel = player.velocidadMaxima;
        player.velocidadMaxima = pj.okki.maxVelShield;

        float storedVel = player.fuerzaMovimiento;
        player.fuerzaMovimiento = pj.okki.velShield;

        while (time <= pj.okki.tiempoMaxShield && (!Input.GetButton(input.Ultimate) || time <= pj.okki.tiempoMinShield)) //Para que permanezca min un tiempo)
        {
            player.porcentajeDamage = pj.okki.porcentajeDamageShield;

            float h = Input.GetAxis(input.LHorizontal);
            float v = Input.GetAxis(input.LVertical);

            if (v > 0.2f && Mathf.Abs(h) < 0.25f)   //FRONTAL SHIELD
                player.inGameAnimator.SetBool("ForwardShield", true);
            else
                player.inGameAnimator.SetBool("ForwardShield", false);

            if (v < -0.2f && Mathf.Abs(h) < 0.25f) //ATRAS SHIELD
                player.inGameAnimator.SetBool("BackShield", true);
            else
                player.inGameAnimator.SetBool("BackShield", false);

            if (h>0.25f)  //RIGHT SHIELD
                player.inGameAnimator.SetBool("RightShield", true);
            else
                player.inGameAnimator.SetBool("RightShield", false);

            if (h < -0.25f)  //LEFT SHIELD
                player.inGameAnimator.SetBool("RightShield", true);
            else
                player.inGameAnimator.SetBool("RightShield", false);

            if(h==0 && v==0)
            { player.inGameAnimator.SetBool("ForwardShield", false); player.inGameAnimator.SetBool("BackShield", false);
               player.inGameAnimator.SetBool("LeftShield", false); player.inGameAnimator.SetBool("RightShield", false); }

            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        } //MOVIMIENTO CON ESCUDO

        player.inGameAnimator.SetTrigger("ElementOverload2");

        StartCoroutine(player.DeshabilitarMovimiento()); //Mientras corre

        player.porcentajeDamage = 150;

        yield return new WaitForSeconds(0.25f);

        float time2 = 0;
        float aumentoDamage = 0;


            StartCoroutine(OkkiMovimientoCharge()); //Corutina para movimiento (sprint)
        personajeTransform.Find("RigPelvis").Find("Shield").GetComponent<MeshRenderer>().enabled = false;


        while (time2 <= pj.okki.maxSprint && (!Input.GetButton(input.Ultimate) || time2 <= pj.okki.minSprint))
        {

            time2 += Time.deltaTime;

            aumentoDamage += Time.deltaTime * pj.okki.aumentoDamageCharge;

            yield return new WaitForEndOfFrame();

        } //CHARGE, SPRINT


        float maxDamage = pj.okki.maxDamageEO + aumentoDamage;

        player.inGameAnimator.SetTrigger("ThunderHammer");

        player.porcentajeDamage = 100;

            yield return new WaitForSeconds(1.2f);

        personajeTransform = player.transform.Find("Personaje").transform; 

        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform, suelo: true);

        ParticleSystem electricExplosion = Instantiate(pj.okki.electricSlam, player.transform.position, Quaternion.identity);
        OkkiThunderHammer(maxDamage); //HACER DAMAGE

        player.rb.velocity = Vector3.zero;

        StartCoroutine(player.HabilitarMovimiento(delayInicial: 0.5f));

            yield return new WaitForSeconds(0.5f);

        player.velocidadMaxima = storedMaxVel;

        player.fuerzaMovimiento = storedVel;
        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform, suelo: true);

        player.rb.velocity = Vector3.zero;

        player.inmune = false;

        //hadaMov.gameObject.SetActive(true);

        //StartCoroutine(hadaMov.Regresar(player.transform.position, 3 , HadaMover.Estados.enIdle));


        while (recuperandoEO < cooldownEO)
        {
            recuperandoEO += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        EOready = true;

        yield return null;
    }

    IEnumerator OkkiMovimientoCharge()
    {
        float time2 = 0;
        //player.rb.isKinematic = true;

        player.inmune = true;

        do
        {

            Vector3 targetPos = Vector3.zero;

            targetPos += player.transform.forward * pj.okki.velocidadSprint;

            player.rb.MovePosition(player.rb.position + targetPos * Time.fixedDeltaTime);

            targetPos = Vector3.zero;

            time2 += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();

        } while (time2 <= pj.okki.maxSprint && (!Input.GetButton(input.Ultimate) || time2 <= pj.okki.minSprint));

        yield return new WaitForEndOfFrame();
        //player.rb.isKinematic = false;

        yield return null;

    }

    void OkkiThunderHammer(float nMaxDamage)
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, pj.okki.radioEO);

        foreach (Collider collider in colliders)
        {
            Rigidbody otherRB = collider.GetComponent<Rigidbody>();

            if (otherRB == null) continue;

            Player otherPlayer = collider.gameObject.GetComponent<Player>();

            if (otherPlayer == null || otherPlayer == player) continue;

            float damage = DamageSegunDistancia(transform.position, otherPlayer, pj.okki.radioEO, nMaxDamage, minimo: 1.5f);

            if (otherPlayer.isDead)
                player.killScore++;

            StartCoroutine(otherPlayer.TakeDamage(damage, player.name, velocidadPerdida: 120));
        }
    }

    IEnumerator OkkiUtility()
    {
        utilityDisponible = false;
        personajeTransform = player.transform.Find("Personaje").transform; //Esto lo termino haciendo reiteradamentes porque en el start no lo hace


        player.inGameAnimator.SetTrigger("Utility");

        float time = 0;

        Vector3 electricSpawn = player.transform.position+player.transform.forward * 5+ player.rb.velocity/2;

        electricSpawn.y = 1.5f;
        ParticleSystem electricBlast = Instantiate(pj.okki.electricBlast, electricSpawn, Quaternion.identity);

        StartCoroutine(player.DeshabilitarMovimiento(electricBlast.main.duration+0.40f, 0));

        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform); //Adaptar posicion del personaje a la animaicon

        bool auxBool = false;
        while (time <= electricBlast.main.duration+0.35f)
        {
            time += Time.deltaTime;

            
            if (time >= electricBlast.main.duration * 0.75f && auxBool == false)
            {
                auxBool = true;
                //HACER DAMAGE

                Collider[] colliders = Physics.OverlapSphere(electricSpawn, pj.okki.radioElectricBlast);

                foreach (Collider collider in colliders)
                {
                    Rigidbody otherRB = collider.GetComponent<Rigidbody>();

                    if (otherRB == null) continue;

                    Player otherPlayer = collider.gameObject.GetComponent<Player>();

                    if (otherPlayer == null || otherPlayer == player) continue;

                    float damage = pj.okki.damageHammer;

                    StartCoroutine(otherPlayer.TakeDamage(damage, player.name));
                    if (otherPlayer.isDead)
                        player.killScore++;
                }
            } 
            yield return new WaitForEndOfFrame();
        }
        player.transform.position = CorregirFuckingAnimacion(ref personajeTransform); //Adaptar posicion del personaje a la animaicon



        while (recuperandoUtility < cooldownUtility)
        {
            recuperandoUtility += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        recuperandoUtility = 0;
        utilityDisponible = true;
        yield return null;
    }


   
    /// //////////////////////////////////////////////////////////////////////////////////////////
  


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terreno")
        {
            aterrizo = true;
        }
    }


    private float DamageSegunDistancia(Vector3 origen, Player objetivo, float radio, float maxDamage, float minimo=1 )
    {
        Vector3 difVector = transform.position - objetivo.transform.position;

        float distancia = difVector.magnitude;

        float damageRelativo = minimo - distancia / radio;

        Mathf.Clamp(damageRelativo, 0, 1);

        float nDamage = damageRelativo * maxDamage;

        return nDamage;
    }

    public Vector3 CorregirFuckingAnimacion(ref Transform obj, bool suelo=false , bool tambienRotacion=true)
    {
        Debug.Log("Corregida posicion de Player" + player.numeroJugador);

        Vector3 objetivo = obj.position;

        if(suelo)
            objetivo.y = 0;

         obj.localPosition = Vector3.zero;

        if(tambienRotacion)
            obj.localRotation = Quaternion.Euler(Vector3.zero);

        return objetivo;
    }
}
