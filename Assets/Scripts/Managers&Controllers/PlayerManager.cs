using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance;

    

    [SerializeField]
    public SortedList<int, CharacterSettings> playersAssigned;

    public Player[] players;

    public Vector3 forward;
    public Vector3 right;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        } //SINGLETON       
        else Destroy(gameObject);

        
    }
    void Start () {

        playersAssigned = new SortedList<int, CharacterSettings>();

        

        //players = new Player[4];

        //for (int i = 0; i <= 3; i++)
        //{
        //    players[i] = transform.GetChild(i).GetComponent<Player>();
        //}

    }




    public void CrearPersonajeSeleccionado(int nPlayer, CharacterSettings configuracion)
    {
        Player player = players[nPlayer - 1];



        if (player == null) return;

        GameObject modelo = Instantiate(configuracion.personaje);

        modelo.transform.SetParent(player.transform); //CENTRAR MODELO A PLAYER

        modelo.transform.localPosition = Vector3.zero;

        modelo.transform.localRotation = Quaternion.Euler(Vector3.zero);

        modelo.name = "Personaje";

        player.personaje = configuracion.name;                          //NOMBRE
        player.iconoJugador = configuracion.Imagen;
        player.resurrection = configuracion.resurrection;
        player.inGameAnimator = modelo.GetComponent<Animator>();        //ANIMATOR
        player.fireMana = configuracion.fireMana;                       //FIRE MANA
        player.fireExpend = configuracion.fireExpend;                   //GASTO MANA
        player.fireDPS = configuracion.fireDPS;                         //FIRE DPS
                                                                        //player.explosion = configuracion.explosion;                     //EFECTO EXPLOSION
        player.fuerzaExplosion = configuracion.fuerzaExplosion;         //FUERZA EXPLOSION
        player.radioExplosion = configuracion.radioExplosion;           //RADIO EXPLOSION
        player.fireRegen = configuracion.fireRegen;


        player.airMana = configuracion.airMana;                         //AIR MANA
        player.multiplicadorSprint = configuracion.multiplicadorSpint;                   //FUERZA DASH
        player.airExpend = configuracion.airExpend;                     //GASTO MANA
        player.runningAirRegen = configuracion.runningAirRegen;         //AIR REGEN


        player.waterMana = configuracion.waterMana;                     //WATER MANA
        player.waterExpend = configuracion.waterExpend;                 //GASTO MANA
        player.curacion = configuracion.curacion;                       //LO QUE SE CURA
        player.healChanneling = configuracion.healChanneling;           //LO QUE TARDA EM HACERLO
        player.waterRegen = configuracion.waterRegen;

        player.earthMana = configuracion.earthMana;                     // EARTH MANA
        player.earthExpend = configuracion.earthExpend;                 //GASTO MANA
                                                                        //player.earthWall = configuracion.earthWall;                     //EFECTO MURO
        player.earthRegen = configuracion.earthRegen;

        player.fuerzaMovimiento = configuracion.fuerzaMovimiento;           //VELOCIDAD MOVIMIENTO
        player.velocidadRotacion = configuracion.velocidadRotacion;         //VELOCIDAD ROTACION

        player.velocidadMaxima = configuracion.velocidadMaxima;             //MAX VEL PARA QUE NO SE VAYA A LA GARCHA
        player.masa = configuracion.masa;                                   //MASA
        player.drag = configuracion.drag;                                   //FUERZA ROZAMIENTO



        Hada hada = player.GetComponentInChildren<Hada>();

        Debug.Log(hada.name);

        HadaMover hadaMov = hada.GetComponent<HadaMover>();

        Debug.Log(hadaMov.name);

        GameObject modeloHada = Instantiate(configuracion.hada.modelo);

        modeloHada.transform.SetParent(hada.transform);

        modeloHada.transform.localPosition = Vector3.zero;

        modeloHada.name = "PersonajeHada";

        hada.oneShotDamage = configuracion.hada.oneShotDamage;

            hadaMov.fuerzaAtaque = configuracion.hada.fuerzaAtaque;

            hadaMov.regresoSmooth= configuracion.hada.regresoSmooth;

            hadaMov.idle.smoothing = configuracion.hada.smoothing;

         hadaMov.idle.distanciaMax = configuracion.hada.distanciaMax;

        hada.fireFX = configuracion.hada.fireFX;
            hada.waterfx = configuracion.hada.waterFX;
            hada.earthFX = configuracion.hada.earthFX;
            hada.airFX = configuracion.hada.airFX;


        print(hada.gameObject.name);

        playersAssigned.Add(nPlayer, configuracion);

    } //Asignar todas las caracteristicas y objetos del scriptanle

    public void EnablePlayers()
    {
        foreach(Player player in players) 
        {
            Debug.Log(player.name + " desactivado");
            player.gameObject.SetActive(false);

        }//Apago todos los players para eliminar los que no vayan a jugar

        foreach ( int i in playersAssigned.Keys)
        {
            Player nPlayer = players[i - 1];

            nPlayer.gameObject.SetActive(true);

            Debug.Log("Player " + i + " activado");

            MonoBehaviour[] componentes = nPlayer.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour component in componentes) //Activo todos los componentes de los players que 
                                                            //van a jugar (los tenia apagados para que no hincharan)
            {
                component.enabled = true;
            }

            EnableCompanion(nPlayer);


            Debug.Log("Player " + i + " cargado en el level");
        }
    } //Activar players que vayan a jugar y sus componentes (apaga los otros)

    public void DisablePlayers()
    {
        

        foreach (Player player in players)
        {
            player.gameObject.SetActive(true); //Me asegura de tenerlo prendido

            Debug.Log("Desactivo Player"+player.name);
            MonoBehaviour[] componentes = player.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour component in componentes) //Desactivo componentes para que no hinchen en el menu
            {                                                //(excepto player que lo uso para crear el personaje)
                if (component == player)
                    continue;
                component.enabled = false;
            }


        }
    } //Desactivar componentes de todos los players (excepto el MonoBeahviour "Player"

    public void EnableCompanion(Player player)
    {
        GameObject hada = player.transform.Find("Hada" + player.numeroJugador).gameObject;

        MonoBehaviour[] componentes = hada.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour component in componentes)
        {
            component.enabled = false;
        }


        foreach (MonoBehaviour component in componentes) 
        {
            component.enabled = true;
        }

        hada.transform.parent = null;
    } 

   

    [ContextMenu("ClearPlayers")]
    public void ClearAssignedPlayers()
    {
        playersAssigned.Clear();

        for (int i = 1; i < 5; i++)
        {
            ClearAssignedPlayer(i);
        }
    } //Eso

    public void ClearAssignedPlayer(int key)
    {

         playersAssigned.Remove(key);
         Player player = players[key - 1];



        if (player.isActiveAndEnabled)
        {
            GameObject hada = GameObject.Find("Hada" + player.numeroJugador);

            hada.transform.SetParent(player.gameObject.transform);

            GameObject hadaModelo = hada.transform.Find("PersonajeHada").gameObject;
            Destroy(hadaModelo);

            GameObject modelo = player.transform.Find("Personaje").gameObject;

            Destroy(modelo);
        }

        player.gameObject.SetActive(false);
            //GameObject hadaModelo = player.GetComponentInChildren<Hada>().transform.Find("PersonajeHada").gameObject;

            //if (hadaModelo != null)
            //    Destroy(hadaModelo);

    }


    public void CorregirEjes()
    {
        
            forward = Camera.main.transform.forward;
            forward.y = 0;
            transform.forward = Vector3.Normalize(forward);

            right = Quaternion.Euler(new Vector3(0, 90, 0)) * transform.forward;

        right = Vector3.Normalize(right);

    } //Corregir ejes X y Z teniendo en cuenta la main camara de la escena


    public bool PlayerExists(int i) {

        if (playersAssigned.ContainsKey(i))
            return true;
        else
            return false;
    } //Funcion para verificar si un player esta asginado

    [ContextMenu("Chequear Players Assigned")]
    public void PrintPlayersAssigned()
    {
        if (playersAssigned.Count == 0)
            print("Ningun player asignado");

        foreach(int i in playersAssigned.Keys)
            print("Player "+i+ " asignado");
    }
}
