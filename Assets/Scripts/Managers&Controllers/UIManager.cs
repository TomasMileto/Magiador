using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public Animator lobbyAnimator;

    MenuCamera cameraMenu;
    MenuController menuControl;

    public bool presionoStart=false;

    [SerializeField]
    public string actualScene;

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

    } //SINGLETON

    private void Start()
    {
        actualScene = GameManager.Instance.actualScene;


        //StartCoroutine(MaquinaPrincipal()); ////No es la mejor idea

    }

    private void Update()
    {
        actualScene = GameManager.Instance.actualScene;
    }

    //IEnumerator MaquinaPrincipal()
    //{
    //    while (true) {


    //        switch (actualScene)
    //        {
    //            case "Menu Principal":

    //                menuAnimator = BuscarElemento("Menu").transform.Find("Fade").GetComponent<Animator>();
    //                cameraMenu = Camera.main.GetComponentInParent<MenuCamera>();
    //                menuControl = FindObjectOfType<MenuController>().GetComponent<MenuController>();

    //                yield return StartCoroutine(PressStart());
    //                yield return StartCoroutine(InMenu());
                    

    //                break;

    //            case "Lobby":

    //                print("Ui se encuentra en lobby");
    //                yield return new WaitForSeconds(2f);
    //                lobbyAnimator=GameObject.FindGameObjectWithTag("SceneAnimation").GetComponent<Animator>();
    //                break;

    //            case "Desert Pit":

    //                //GameObject[] elementos = BuscarElementos("PlayerHUD", apagar:true);



    //                yield return StartCoroutine(Esperando("Desert Pit"));


    //                break;

    //            default:
    //                yield return new WaitForEndOfFrame();

    //                break;


    //        }


    //    }
    //}


   public IEnumerator PressStart()
   {

        yield return new WaitUntil(() => Input.GetButton("Start"));

        presionoStart = true;

        //yield return StartCoroutine(cameraMenu.TravelTo(menuControl.toFireTotem));


        BuscarElemento("Menu>>UI").transform.Find("Start").gameObject.SetActive(false);

        yield return null;
       
   }

    

    //IEnumerator PlayerSelection()
    //{

    //}
    IEnumerator Esperando(string estado)
    {
        while (actualScene == estado)
        {
            yield return new WaitForEndOfFrame();
        }
    }


    public void PrenderUI(GameObject obj)
    {
        Debug.Log(obj.name + " UI: Prendido");
        obj.SetActive(true);
    }

    //public void PrenderUIs(GameObject[] objs, bool segunPlayers=false)
    //{
    //    if (segunPlayers)
    //    {
    //        foreach(GameObject obj in objs)
    //        {
    //            int i = obj.name[1]; //Posicion del nombre del game object donde esta el numero de player

    //            print("AAAAAAAAAA " + i);
    //            if (PlayerManager.Instance.PlayerExists(i))
    //            {
    //                PrenderUI(obj);
    //            }
    //        }

    //        return;
    //    }

    //    foreach (GameObject obj in objs)
    //    {

    //        PrenderUI(obj);
    //    }
    //}



    public void ApagarUI(GameObject obj)
    {
        obj.SetActive(false);
    }

    GameObject BuscarElemento (string nombre)
    {
        return GameObject.Find(nombre);
    }

    GameObject[] BuscarElementos(string tag, bool apagar=false)
    {
        GameObject[] elementos= GameObject.FindGameObjectsWithTag(tag);

        if (apagar)
        {
            foreach(GameObject elemento in elementos)
            {
                ApagarUI(elemento);
            }
        }

        return elementos;
    }


    public void SwitchUI(GameObject UIOn, GameObject UIOff)
    {
        UIOn.SetActive(true);

        UIOff.SetActive(false);
    }
}
