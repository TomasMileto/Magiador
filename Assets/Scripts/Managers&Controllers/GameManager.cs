using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;


    public string actualScene;

    bool cursorBloqueado;


    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded; //SceneManager.sceneLoaded es un "delegado". Por lo que entendi, el delegado es un tipo
                                                   //de variable que permite "almacenar" funciones, lo cual es util para definir a gusto
                                                  //ciertos comportamientos complejos
    }

    public void OnSceneLoaded(Scene escena, LoadSceneMode mode)  //Defino el metodo para asignar al delegado
    {

        Debug.Log("Escena: " +escena.name);

        actualScene = escena.name;

        SoundManager.Instance.GetAudioSource();

        PlayerManager.Instance.CorregirEjes();

        //HadaMover[] hadas = FindObjectsOfType<HadaMover>();

       
    }

    private void Start()
    {
        //StartCoroutine(SeteoInicial());
    }


    void Update () {

        DetectarBloqueoDelMouse();


	}

    private void DetectarBloqueoDelMouse()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorBloqueado = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorBloqueado = true;
        }

        if (cursorBloqueado == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (cursorBloqueado == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


    }

    public IEnumerator StartLevel(string level, float delay=0)
    {
        yield return new WaitForSeconds(delay);
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        if(level=="Desert Pit")
            PlayerManager.Instance.EnablePlayers();


        while (operation.isDone == false)
        {
            Debug.Log("Cargando " + level);

            yield return new WaitForEndOfFrame();
        }

        print("Cargue level:" +level);
        PlayerManager.Instance.CorregirEjes();



    }

    public IEnumerator SalirDelJuego()
    {
        yield return new WaitForSeconds(0.5f);

        Application.Quit();
    }
    public IEnumerator SeteoInicial()
    {

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        PlayerManager.Instance.DisablePlayers();
    }

    public IEnumerator StartLevel(int levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

        

        while (operation.isDone == false)
        {
            Debug.Log("Cargando " + levelIndex);

            yield return new WaitForEndOfFrame();
        }


        yield return new WaitForSeconds(2.2f);


        


    } //SOBRECARGA

    public IEnumerator EndLevel()
    {
        yield return null;
    }

    
}
