using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public static CharacterManager Instance;


    public SortedList<string, CharacterSettings> characters;  //Me olvide por que hice esto pero lo dejo nunca de mas


    public Characters _characters;

    public Bric bric;

    public Creepyno creepyno;

    public Malmon malmon;

    public Okki okki;

    public Pok pok;

    public Stuarto stuarto;

    [System.Serializable]
    public class Characters
    {
        public CharacterSettings Bric;
        public CharacterSettings Creepyno;
        public CharacterSettings Malmon;
        public CharacterSettings Okki;
        public CharacterSettings Pok;
        public CharacterSettings Stuarto;
    }

    [System.Serializable]
    public class Bric
    {
        public ParticleSystem exanimum;
        public GameObject lifeParticle;
        public ParticleSystem darkRelease;

        public GameObject batCloud;

        public float maxDamage=120;
        public float radio = 15;

        public float recuperacioEO = 50;

        public float recuperacionUtility = 11;

        public IEnumerator moveLifeParticle(GameObject go, Vector3 objetivo)
        {
            while(Vector3.Distance(go.transform.position, objetivo) > 0.15f)
            {
                go.transform.position = Vector3.MoveTowards(go.transform.position, objetivo, 2f);

                yield return new WaitForFixedUpdate();
            }
            Destroy(go,0.1f);
            yield return null; 
        }



    }

    [System.Serializable]
    public class Creepyno
    {
        //Coming soon
    } //Coming soon

    [System.Serializable]
    public class Malmon
    {
        public GameObject demon;
        public ParticleSystem fireColumn;
        public ParticleSystem minionSpawn;

        public ParticleSystem demonDead;
        public ParticleSystem minionDead;

        public float distanciaSpawnDemon;

        public float distanciaSpawnMinion;



    }

    [System.Serializable]
    public class Okki
    {
        public ParticleSystem electricBlast;
        public ParticleSystem electricSlam;
        public ParticleSystem chargeDust; //opcional


        public float tiempoMinShield;
        public float tiempoMaxShield;

        public float maxVelShield = 5;
        public float velShield = 20;

        public float recuperacionUtility;
        public float recuperacionEO;

        public float porcentajeDamageShield; //Cuanto toma del damage total??


        public float damageHammer = 55;

        public float aumentoDamageCharge=16; 

        public float radioElectricBlast=5.5f;

        public float minSprint = 2f;

        public float maxSprint = 5f;

        public float velocidadSprint=40; //Cuando carga

        public float fuerzaSalto = 50;

        public float radioEO=10;

        public float minDamageEO = 70;
        public float maxDamageEO= 120;


    }

    [System.Serializable]
    public class Pok
    {
        //Coming soon
    } //Coming soon

    [System.Serializable]
    public class Stuarto
    {
        public float distanciaRoll=5;
        public float fuerzaRoll = 200;
        public float recuperacionUtility=8;

        public float recuperacionEO = 60;
        
        public GameObject entradaTierra;

        public ParticleSystem permanecerTierra;

        public GameObject salirTierra;

        public float radioSalida=10;
        public float fuerzaBurstOut = 100;
        public float fuerzaSalto = 100;

        public float minTime = 2.5f;

        public float maxTime = 6f;

        public float maxDamage=100;

    }



    private void Awake()
    {
        if (Instance == null)  
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }  //SINGLETON
        else  Destroy(gameObject);
    }


    void Start () {

        characters = new SortedList<string, CharacterSettings>();
        characters.Add("Bric", _characters.Bric);
        characters.Add("Creepyno", _characters.Creepyno);
        characters.Add("Malmon", _characters.Malmon);
        characters.Add("Okki", _characters.Okki);
        characters.Add("Pok", _characters.Pok);
        characters.Add("Stuarto", _characters.Stuarto);

    }

	
	
}
