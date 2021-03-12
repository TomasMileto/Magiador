using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour {

    private GameObject[] panelesHijos;

    private GameObject[] listaPersonajes;

    private GameObject charList;

    private List<GameObject> personajesDisponibles;

    Player player;

    private int panelActual = 0;
    private int personajeActual=0;

    public int numeroJugador;

    public bool rightArrow;
    public bool leftArrow;

    bool presionoDPad=false;

    SoundManager sound;

    void Start() {

        
        player = GameObject.Find("Player" + numeroJugador).GetComponent<Player>();

        panelesHijos = new GameObject[transform.childCount];

        personajesDisponibles = new List<GameObject>();

        sound = SoundManager.Instance;
        

            for (int i = 0; i < panelesHijos.Length; i++)
            {
                panelesHijos[i] = transform.GetChild(i).gameObject;

            }

            foreach(GameObject panel in panelesHijos)
            {
                panel.SetActive(false);
                
            }


        StartCoroutine(RutinaPrincipal());

        GameObject listObject = GameObject.Find("CharListP" + numeroJugador);

        listaPersonajes = new GameObject[listObject.transform.childCount];

            for (int p = 0; p < listaPersonajes.Length; p++)
            {
                listaPersonajes[p] = listObject.transform.GetChild(p).gameObject;
            }

            foreach(GameObject p in listaPersonajes)
            {
                p.SetActive(false);
            }




    }

    private void Update()
    {
        //Cambiar entre los hijos del Panel del Jugador
        //

       
        if (Input.GetButtonDown("J" + numeroJugador + "A"))
        {
            print("Player "+ numeroJugador + " siguiente panel");

            sound.Reproducir(sound.ui.AbuttonPlayerSelection);
            panelActual++;

        } else if (Input.GetButtonDown("J" + numeroJugador + "B") && panelActual > 0)
        {
            print("Player " +numeroJugador + "panel anterior");
            sound.Reproducir(sound.ui.BbuttonPlayerSelection);

            panelActual--;
        }



        //Tratar las flechas del Joystick como botones
        //

        float horizontalDPad = Input.GetAxisRaw("J" + numeroJugador + "ArrowsHorizontal");

        if  (horizontalDPad> 0.45f)
        {
            if (!presionoDPad)
                rightArrow = true;
           else rightArrow = false;

            presionoDPad = true; 
        }
        else if ( horizontalDPad< 0.80f && horizontalDPad>=0 && presionoDPad)
        {
            rightArrow = false;
            presionoDPad = false;
        }

        else if (horizontalDPad < -0.45f)
        {
            if (!presionoDPad)
                leftArrow = true;
            else leftArrow = false;

            presionoDPad = true;
        }
        else if ( horizontalDPad> -0.80f && horizontalDPad<=0 && presionoDPad)
        {
            leftArrow = false;
            presionoDPad = false;
        }






    } //Input A, Input B e Input Flechas D PAD

    IEnumerator RutinaPrincipal() {

        while (true)
        {

            switch (panelActual)
            {
                case 0:

                    
                    panelesHijos[0].SetActive(true);
                    panelesHijos[1].SetActive(false);
                    yield return new WaitForEndOfFrame();
                    break;
                    

                case 1:

                    if(PlayerManager.Instance.playersAssigned.ContainsKey(numeroJugador))
                        PlayerManager.Instance.ClearAssignedPlayer(numeroJugador);

                    panelesHijos[0].SetActive(false);
                    panelesHijos[2].SetActive(false);

                    panelesHijos[1].SetActive(true);

                    yield return StartCoroutine(ElegirPersonaje());



                    break;


                case 2:

                   //personajeElegido=null; //??????????

                    panelesHijos[1].SetActive(false);
                    panelesHijos[3].SetActive(false);

                    

                    panelesHijos[2].SetActive(true);
                    yield return StartCoroutine(VerDetalles());
                    
                    break;

                case 3:
                
                    
                    panelesHijos[2].SetActive(false);
                    panelesHijos[3].SetActive(true);

                    yield return StartCoroutine(AsignarPersonaje(personajeActual));
                    yield return StartCoroutine(EsperandoParaJugar());

                    StartCoroutine(GameManager.Instance.StartLevel("Desert Pit", delay: 1.5f));
                    panelActual++;
                    break;

                default:
                    yield return null;
                    break;
                

            }

            yield return null;
        }

    }

    int index = 0;
    IEnumerator ElegirPersonaje()
    {


        foreach (GameObject p in listaPersonajes)
        {
            p.SetActive(false);
        }

        while (panelActual == 1)
       {
            
            
            if (rightArrow)
            {
                sound.Reproducir(sound.ui.moveInPlayerSelection);
                listaPersonajes[personajeActual].SetActive(false);
                index++;
            }

            if (leftArrow)
            {
                sound.Reproducir(sound.ui.moveInPlayerSelection);

                listaPersonajes[personajeActual].SetActive(false);
                index--;
            }

            index = Mathf.Clamp(index, 0, 100);
            personajeActual = index % listaPersonajes.Length;


            listaPersonajes[personajeActual].SetActive(true);

            yield return new WaitForEndOfFrame();

        }
        foreach (GameObject p in listaPersonajes)
        {
            if (p == listaPersonajes[personajeActual])
                continue;
            p.SetActive(false);
        }
        yield return null;

    }
    CharacterSettings personajeElegido;

    IEnumerator AsignarPersonaje(int i)
    {
        
        personajeElegido = listaPersonajes[i].GetComponent<PersonajeData>().data;

        PlayerManager.Instance.CrearPersonajeSeleccionado(numeroJugador, personajeElegido);

        yield return null;
    }

   IEnumerator VerDetalles()
    {
        
        Text text = panelesHijos[panelActual].GetComponentInChildren<Text>();

        if (text != null && personajeElegido!=null)
        {
            text.text = personajeElegido.nombre + "\n" + personajeElegido.descripcion;
        }
        while (panelActual == 2)
        {
            yield return new WaitForEndOfFrame();
        }
    } 

    IEnumerator EsperandoParaJugar()
    {

        while (!Input.GetButton("J"+numeroJugador+"Start"))
        {
            yield return new WaitForEndOfFrame();

        }

        GameObject.FindGameObjectWithTag("SceneAnimation").GetComponent<Animator>().SetBool("Out", true);
        //UIManager.Instance.lobbyAnimator.SetBool("Out", true);

    }
}
