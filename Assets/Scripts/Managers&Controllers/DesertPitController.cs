using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class DesertPitController : MonoBehaviour {

    public static DesertPitController Instance; //creo que me va a servir, pero no lo hago singleton

    UIManager UI;

    public GameObject[] HUDplayers;

    public Transform[] spawnPositions;


    public Canvas PauseUI;

    public Canvas suddenDeath;

    public ParticleSystem fireworksFX;

    public Animator skullFade;

    Player playerVictorioso;

    public GameState estadoActual= GameState.comienzo;

    public GameEvents eventoActual = GameEvents.mainEvent;

    public float startDelay = 5f;

    public float timer=180f;

    public Text initialCountdown;

    public Text countDown;

    public Coroutine gameLoop;

    bool juegoTerminado;

    SoundManager sound;

    public enum GameState
    {
        comienzo,
        jugando,
        pausa,
        final
    }

    public enum GameEvents
    {
        mainEvent,
        suddenDeath
    } //Hago un enum por si en el futuro quiero agregar eventos

	void Start () {

        UI = UIManager.Instance;

        Instance = this;

        for (int i = 0; i < 4; i++)
        {
            if (!PlayerManager.Instance.PlayerExists(i + 1)) continue;

             PlayerManager.Instance.players[i].transform.position = spawnPositions[i].position;
            

        }

        gameLoop=StartCoroutine(GameLoop());


        sound = SoundManager.Instance;

        countDown.text = "03:00";

        foreach(Player p in PlayerManager.Instance.players)
        {
            if (p.isActiveAndEnabled == true)
            {
                p.vida = p.maxHealth;
                p.vidasLeft = 3;
                p.killScore = 0;
            }
        }
    }


	

    public IEnumerator GameLoop()
    {
        while (true)
        {
            switch (estadoActual)
            {
                case GameState.comienzo:

                    yield return StartCoroutine(DeshabilitarPlayers());
                    Debug.Log("Comienzo de Game Loop");
                    AsignarIconosUI();
                    StartCoroutine(InitialCountdown());
                    yield return StartCoroutine(HabilitarPlayers(startDelay));
                    estadoActual = GameState.jugando;
                    yield return new WaitForEndOfFrame();
                    break;

                case GameState.jugando:
                    Debug.Log("Jugando");
                    //eventos
                    StartCoroutine(StartButton(pasarAEstado: GameState.pausa));

                    switch (eventoActual)
                    {
                        case GameEvents.mainEvent:
                            yield return StartCoroutine(MainEvent());
                            break;

                        case GameEvents.suddenDeath:
                            yield return StartCoroutine(SuddenDeath());
                            break;
                    }
                    yield return new WaitForEndOfFrame();
                    break;

                case GameState.pausa:
                    print("Pausado");
                    StartCoroutine(StartButton(pasarAEstado: GameState.jugando));
                    yield return StartCoroutine(MenuPausa());

                    break;

                case GameState.final:

                    if (juegoTerminado)
                        yield return StartCoroutine(EndGame(delaySalida: 5));
                    else
                    {
                        if (CompararKillScores().Count > 1)
                        {
                            yield return StartCoroutine(StartSuddenDeath(CompararKillScores()));

                            estadoActual = GameState.jugando;

                            yield return new WaitForEndOfFrame();
                        }
                        else
                        {
                            playerVictorioso= CompararKillScores()[0]; //El unico de la lista de leaders

                            yield return StartCoroutine(EndGame(delayBanner:1.5f, delaySalida: 5));

                        }
                    }
                    
                    break;

            }
        }
    }
    void AsignarIconosUI()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!PlayerManager.Instance.PlayerExists(i + 1)) continue;

            Image icono = HUDplayers[i].GetComponent<Image>();

            icono.sprite = PlayerManager.Instance.players[i].iconoJugador;
        }
    }

    IEnumerator InitialCountdown()
    {
        float t = 5;

        while (t > 0)
        {
            t -= Time.deltaTime;

            int seconds = (int)(t % 60);

            initialCountdown.text =seconds.ToString("0");

            yield return null;
        }

        initialCountdown.gameObject.SetActive(false);
    }
    IEnumerator HabilitarPlayers(float delay=0)
    {

        yield return new WaitForSeconds(delay);

        foreach(Player player in PlayerManager.Instance.players)
        {
            if (!PlayerManager.Instance.PlayerExists(player.numeroJugador)) continue;

            player.GetComponent<PlayerInput>().leerInput = true;
        }



    } //Probablemente cambie la funcion del Player Manager por esta

    IEnumerator DeshabilitarPlayers(float delay=0)
    {

        yield return new WaitForSeconds(delay);

        foreach (Player player in PlayerManager.Instance.players)
        {
            if (!PlayerManager.Instance.PlayerExists(player.numeroJugador)) continue;

            player.GetComponent<PlayerInput>().leerInput = false;
        }



    }

    //GAME LOOP

    IEnumerator StartButton(GameState pasarAEstado)
    {
        while (!Input.GetButton("Start"))  //Input Get Button Down no me funciona por algun motivo
        {
            
            yield return new WaitForEndOfFrame();
        }

        estadoActual = pasarAEstado;

        yield return new WaitForEndOfFrame();
    }


    IEnumerator MainEvent()
    {
        
        while (timer > 0 && estadoActual==GameState.jugando)
        {
            timer -= Time.deltaTime;

            int minutes = (int) (timer / 60);
            int seconds = (int) (timer % 60);
            
            countDown.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            yield return new WaitForEndOfFrame();
        }

        if (timer <= 0)
            estadoActual = GameState.final;

    }


    IEnumerator StartSuddenDeath(List<Player> leaders)
    {
        sound.Reproducir(sound.evento.suddenDeath);

        foreach(Player player in PlayerManager.Instance.players)
        {
            if (leaders.Contains(player)) continue;

            if (PlayerManager.Instance.PlayerExists(player.numeroJugador))
            {
                player.vidasLeft = 1; //Lo pongo en uno porque Die() resta una vida

                player.Die();
            }
            //Hacer explotar o algo

        }//Sacar players que no son leaders de la partida

        foreach(Player leader in leaders)
        {
            leader.vidasLeft = 1;

            leader.killScore = 2;

        } //Dejar 1 vida para los leaders y un kill score de 2

        eventoActual = GameEvents.suddenDeath;

        yield return new WaitForEndOfFrame();
    }

    IEnumerator SuddenDeath()
    {
        Debug.Log("SUDDEN DEATH AAAAAAAAAAA!!!");

        suddenDeath.gameObject.SetActive(true);

        while (estadoActual == GameState.jugando)
        {

            foreach(Player player in PlayerManager.Instance.players)
            {
                if (PlayerManager.Instance.PlayerExists(player.numeroJugador))
                {
                    StartCoroutine(player.TakeDamage(2.5f));
                }
            }
            yield return new WaitForSeconds(1);
        }

        suddenDeath.GetComponent<Animator>().SetTrigger("EndGame");
        
    }
    IEnumerator MenuPausa()
    {
        float prevTimeScale = Time.timeScale;

        PauseUI.gameObject.SetActive(true);

        Time.timeScale = 0;

        StartCoroutine(DeshabilitarPlayers());

        while (estadoActual == GameState.pausa)
        {

            if (Input.GetButton("A"))
            {
                estadoActual = GameState.jugando;
            }

            if (Input.GetButton("B"))
            {
                ToMenu();

                break;
            }

            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = prevTimeScale;

        PauseUI.gameObject.SetActive(false);

        StartCoroutine(HabilitarPlayers());
    }

    IEnumerator EndGame(float delaySalida, float delayBanner=0)
    {
        StartCoroutine(DeshabilitarPlayers(0.5f));

        yield return new WaitForSeconds(delayBanner);

        sound.Reproducir(sound.evento.cheers);
        foreach (Player player in PlayerManager.Instance.players)
        {
            if (player==playerVictorioso) continue;

            if (PlayerManager.Instance.PlayerExists(player.numeroJugador))
            {
                player.vidasLeft = 1; //Lo pongo en uno porque Die() resta una vida

                player.Die();
            }

        }//Sacar players que no son leaders de la partida

        Debug.Log(playerVictorioso.name +" ha ganado la partida!");

        ParticleSystem fireworks = Instantiate(fireworksFX, playerVictorioso.transform.position+new Vector3(0,5,0), Quaternion.identity);


        yield return new WaitForSeconds(delaySalida);

        skullFade.SetBool("Out", true);

        ToMenu();

        yield return null;
    }

    void ToMenu()
    {
        StartCoroutine(GameManager.Instance.StartLevel("Menu Principal", 3f));

        Time.timeScale = 1;
        PlayerManager.Instance.ClearAssignedPlayers();

        StopCoroutine(gameLoop);
    }

    [ContextMenu("Comparar Scores")]
    List<Player> CompararKillScores()
    {
        List<Player> leaders = new List<Player>();

        foreach (Player player in PlayerManager.Instance.players)
        {
            if(PlayerManager.Instance.PlayerExists(player.numeroJugador) && player.isOutOfGame==false)
            {
                bool isLeader = true;

                foreach(Player otherPlayer in PlayerManager.Instance.players) 
                {
                    if (otherPlayer == player) continue;

                    if(otherPlayer.killScore > player.killScore)
                    {
                        isLeader = false;
                    }

                } //Determinar si es uno de los lideres

                if (isLeader)
                {
                    leaders.Add(player);
                    print(player.name + " is leader");
                }
            }
        }

        return leaders;
    }

   

    public void ChequearVictoria(Player nKiller)
    {
        if (nKiller.killScore >= 3)
        {
            estadoActual = GameState.final;

            juegoTerminado = true;

            playerVictorioso = nKiller;
        }
       

    } 

    public void ChequearLastManStanding()
    {
        Debug.Log("Chequear Last Man Standing");
        foreach (Player player in PlayerManager.Instance.players)
        {
            if (PlayerManager.Instance.PlayerExists(player.numeroJugador))
            {
                bool isLastMan = true;

                print("Verifico si " + player.name + " es last man");
                
                foreach (Player otherPlayer in PlayerManager.Instance.players)
                {
                    if (otherPlayer == player) continue;
                    if (otherPlayer.isActiveAndEnabled == false) continue;

                    print(otherPlayer.name + " out of play: " + otherPlayer.isOutOfGame);
                    if (otherPlayer.isOutOfGame==false)
                    {
                        isLastMan = false;
                    }

                } //Determinar si es el unico player que queda con vida

                if (isLastMan)
                {
                    Debug.Log(player.name + " unico sobreviviente");

                    juegoTerminado = true;

                    playerVictorioso = player;

                    estadoActual = GameState.final;

                }
            }
        }
    }

    //IEnumerator DesertPitMusic()
    //{
    //    float t = 0;

    //    while()
    //}
}
