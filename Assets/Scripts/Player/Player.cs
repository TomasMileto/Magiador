using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Intrinsico")]
    public string personaje;
    public Animator inGameAnimator;
    public int numeroJugador;
    public Sprite iconoJugador;
    public Color colorPlayer;
    public Transform startPosition;
    public bool isDead;
    public bool isOutOfGame = false;
    public ParticleSystem resurrection;
    public ParticleSystem muerte;
    public ParticleSystem muerteForGood;

    [SerializeField] string lastHitBy = "";

    SoundManager sound;

    public bool puedeMoverse=true;
    public bool puedeRotar=true;
    public bool inmune=false;
    public float porcentajeDamage = 100;

    [Header(" ")]
    [Header("                                   Vida & Mana                      ")]
    [Header(" ")]


    [Range(0, 300)]
    public float vida = 300;

    [HideInInspector]
    public float maxHealth;

    [HideInInspector]
    public bool sprintHabilitado=true;

    [Header("Fuego")]
    [Range(0, 100)]
    public float fireMana;
    public float fireExpend=1f;
    public float fireRegen = 1;
    public float fireDPS;
    public ParticleSystem explosion;
    public float fuerzaExplosion=100;
    public float radioExplosion=5;

    [Header("Aire")]
    [Range(0, 100)]
    public float airMana;
    public float multiplicadorSprint;
    public float airExpend=1f;
    public float runningAirRegen = 3.5f;

    [Header("Agua")]
    [Range(0, 100)]
    public float waterMana;
    public float waterExpend=1f;
    public ParticleSystem waterHealing;
    public float curacion = 40;
    public float waterRegen = 1;
    public float healChanneling=1.5f;

    [Header("Tierra")]
    [Range(0, 100)]
    public float earthMana;
    public float earthExpend=1f;
    public float earthRegen = 1;
    public GameObject earthWall;


    [Header(" ")]
    [Header("                                     Movimiento  \n                      ")]
    [Header(" ")]
    public float fuerzaMovimiento;
    public float velocidadRotacion;
    public float velocidadMaxima;
    public float masa;
    public float drag;

    public Rigidbody rb;


    //[Header(" ")]
    //[Header("                                     Element Overload                      ")]
    //[Header(" ")]
    //public ParticleSystem EOmainFX;


    [HideInInspector]
    public int vidasLeft=3;
    public int killScore = 0;

    /// ///////////////////////////////////////////////////////////////////////////




    void Start () {

        rb = GetComponent<Rigidbody>();

        maxHealth = vida;

        sound = SoundManager.Instance;
	}
	
	void Update () {


        vida = Mathf.Clamp(vida, 0, maxHealth);

        fireMana = Mathf.Clamp(fireMana, 0, 100);
        airMana = Mathf.Clamp(airMana, 0, 100);
        waterMana = Mathf.Clamp(waterMana, 0, 100);
        earthMana = Mathf.Clamp(earthMana, 0, 100);

        if (transform.position.y < -0.4f)
        {
            transform.position += new Vector3(0, 2, 0);  //lo pongo provisoriamente por el bug de hundirse 
        }
        if (vida <= 0 && isDead==false)
        {
            isDead = true;
            vida = 0;
            Die();

        }

        if (rb.velocity.magnitude >= 0.5)
        {
            airMana += runningAirRegen * Time.deltaTime * rb.velocity.magnitude;
        }

        fireMana += Time.deltaTime * fireRegen;
        earthMana += Time.deltaTime * earthRegen;
        waterMana += Time.deltaTime * waterRegen;

       

    }

    float curVelocity;


    public IEnumerator TakeDamage(float damage, string victimario="Event", float velocidadPerdida = 100)
    {
        if (!inmune)
        {
            float vidaActual = vida;

            if(victimario!="Player"+numeroJugador)
                lastHitBy = victimario;

            float objetivo = vidaActual - damage * porcentajeDamage/100;

            switch(personaje)
        {
            case "Bric":
                sound.Reproducir(sound.playerSFX.bricHit, vol: 2);
                break;
            case "Okki":
                sound.Reproducir(sound.playerSFX.okkiHit, vol: 2);
                break;
            case "Stuarto":
                sound.Reproducir(sound.playerSFX.stuartoHit, vol: 2);
                break;

            }

            while (vida >= objetivo && isDead==false)
            {
                vida -= Time.deltaTime * velocidadPerdida;

                //vida = Mathf.Lerp(vidaActual, objetivo, 2f);

                yield return new WaitForEndOfFrame();
            }

        }

        yield return null;
        
    }

    public void Die()
    {
        //Animacion y morir
        inGameAnimator.SetBool("Dead", true);
        inGameAnimator.SetTrigger("Die");
        DeshabilitarMovimiento();

        switch (personaje)
        {
            case "Bric":
                sound.Reproducir(sound.playerSFX.bricDeath, vol: 2);
                break;
            case "Okki":
                sound.Reproducir(sound.playerSFX.okkiDeath, vol: 2);
                break;
            case "Stuarto":
                sound.Reproducir(sound.playerSFX.stuartoDeath, vol: 2);
                break;

        }//SFX segun personaje

        Debug.Log("Jugador " + numeroJugador + " ha muerto");

        vidasLeft--;

        //Chequear si el otro player gano

        if (lastHitBy != "Event")
        {
            Player killer = GameObject.Find(lastHitBy).GetComponent<Player>();

            killer.killScore++;

            DesertPitController.Instance.ChequearVictoria(killer);
        }
        //Revivir
        if (vidasLeft > 0)
        {
            StartCoroutine(Respawn());
        }
        //Morir definitivamente
        else
        {
            StartCoroutine(DieForGood());
            DesertPitController.Instance.ChequearLastManStanding();
        }
    }


    public IEnumerator DeshabilitarMovimiento(float delayInicial=0 )
    {
        yield return new WaitForSeconds(delayInicial);
        puedeMoverse = false;
    }

    public IEnumerator DeshabilitarMovimiento(float duracion, float delayInicial = 0)
    {
        yield return new WaitForSeconds(delayInicial);
        puedeMoverse = false;
        yield return new WaitForSeconds(duracion);
        StartCoroutine(HabilitarMovimiento());
    } //SobreCarga


    public IEnumerator RalentizarMovimiento(float duracion, float reduccionPorcentaje)
    {

        float velocidadValue = fuerzaMovimiento;

        sprintHabilitado = false;

        fuerzaMovimiento -= velocidadValue* reduccionPorcentaje/100;

        yield return new WaitForSeconds(duracion);

        sprintHabilitado = true;

        fuerzaMovimiento += velocidadValue*  reduccionPorcentaje/100;
    }

    



 
    public IEnumerator Respawn()
    {
        try
        {
            ParticleSystem calavera = Instantiate(muerte, transform.position, Quaternion.identity);
            if (calavera != null)
                Destroy(calavera, 3f);
        }
        catch { }

        

        yield return new WaitForSeconds(3f);

        HabilitarMovimiento();

        StartCoroutine(Heal(maxHealth, 300));

        isDead = false;

        inGameAnimator.SetBool("Dead", false);

        ParticleSystem hazDeLuz = Instantiate(resurrection, transform.position, Quaternion.Euler(-90, 0, 0));

        if(hazDeLuz!=null)
            Destroy(hazDeLuz, 3f);


    }

    public IEnumerator DieForGood()
    {
        try
        {
            ParticleSystem instancia = Instantiate(muerteForGood, transform.position, Quaternion.identity);
            if (instancia != null)
                Destroy(instancia, 3f);
        }
        catch { };

        Debug.Log(gameObject.name + " out of game");

        isOutOfGame = true;

        yield return new WaitForSeconds(4f);

        PlayerManager.Instance.ClearAssignedPlayer(numeroJugador);
    }

    public IEnumerator HabilitarMovimiento(float delayInicial=0, bool andRotacion=true)
    {
        yield return new WaitForSeconds(delayInicial);
        puedeMoverse = true;

        if (andRotacion) 
            StartCoroutine(HabilitarRotacion());

            
    }

    public IEnumerator DeshabilitarRotacion(float delayInicial = 0)
    {
        yield return new WaitForSeconds(delayInicial);
        puedeRotar = false;
    }

    public IEnumerator HabilitarRotacion(float delayInicial=0)
    {
        yield return new WaitForSeconds(delayInicial);
        puedeRotar = true;
    }

    public IEnumerator Heal(float life, float rapidez=200)
    {
        float vidaActual = vida;

        float objetivo = vidaActual +life;

        while (vida < objetivo && vida<maxHealth)
        {

            vida += Time.deltaTime * rapidez;


            yield return new WaitForEndOfFrame();
        }

        yield return null;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }

   

}
