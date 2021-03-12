using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    //public GameObject[] totems;

    [SerializeField]
    public int totemActual;

    MenuCamera cameraMenu;

    public Transform toFireTotem;

    public Transform toWaterTotem;

    public Transform toEarthTotem;

    public Transform toAirTotem;

    public GameObject[] textos;

    public Animator menuAnimator;

    SoundManager sound;

    private void Start()
    {
        print("Bienvenido al menu principal");

        UIManager.Instance.presionoStart = false;

        PlayerManager.Instance.DisablePlayers();

        StartCoroutine(MaquinaPrincial());

        sound = SoundManager.Instance;

    }


    void OnEnable ()
    {
        cameraMenu = Camera.main.GetComponentInParent<MenuCamera>();



        foreach (GameObject texto in textos)
        {
            texto.SetActive(false);
        }

    }

    [SerializeField]
    bool presionoDPad;
    [SerializeField]
    bool rightArrow;
    [SerializeField]
    bool leftArrow;

    public void Update()
    { 

        //A BUTTON 

        


        //FLECHAS
        float horizontalDPad = Input.GetAxisRaw("J" + 1 + "ArrowsHorizontal");

            if (horizontalDPad > 0.45f)
            {
                if (!presionoDPad)
                    rightArrow = true;
                else rightArrow = false;

                presionoDPad = true;
            }
            else if (horizontalDPad < 0.80f && horizontalDPad >= 0 && presionoDPad)
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
            else if (horizontalDPad > -0.80f && horizontalDPad <= 0 && presionoDPad)
            {
                leftArrow = false;
                presionoDPad = false;
            }
        

    } //Para las flechas del joystick

    IEnumerator MaquinaPrincial()
    {
        yield return StartCoroutine(UIManager.Instance.PressStart());

        while (true)
        {
            
           
                switch (totemActual)
                {
                    case 0:

                        textos[3].SetActive(false);
                        textos[0].SetActive(true);
                        textos[1].SetActive(false);


                    yield return StartCoroutine(cameraMenu.TravelTo(toFireTotem));

                    yield return StartCoroutine(TotemPVP());

                    yield return new WaitForEndOfFrame();
                        break;


                    case 1:

                        textos[1].SetActive(true);
                        textos[0].SetActive(false);
                        textos[2].SetActive(false);

                        yield return StartCoroutine(cameraMenu.TravelTo(toWaterTotem));

                        yield return StartCoroutine(TotemHowToPlay());

                        yield return new WaitForEndOfFrame();

                        yield return null;
                        break;


                    case 2:

                        textos[2].SetActive(true);
                        textos[1].SetActive(false);
                        textos[3].SetActive(false);

                        yield return StartCoroutine(cameraMenu.TravelTo(toAirTotem));

                        yield return StartCoroutine(TotemQuit());

                        yield return new WaitForEndOfFrame();

                        break;

                    case 3:

                    textos[2].SetActive(false);
                    textos[3].SetActive(true);
                    textos[1].SetActive(false);

                         yield return StartCoroutine(cameraMenu.TravelTo(toEarthTotem));

                        yield return StartCoroutine(TotemSettings());

                        yield return new WaitForEndOfFrame();

                        break;

                    default:

                        yield return new WaitForEndOfFrame();

                        break;


                }
            
        }

    }



    IEnumerator TotemPVP()
    {
       
        print("TOTEM PVP");
        while (totemActual == 0)
        {
            if (rightArrow)
            {
                sound.Reproducir(sound.ui.moveTotem);
                totemActual = 1;
            }

            if (leftArrow)
            {
                sound.Reproducir(sound.ui.moveTotem);

                totemActual = 3;
                
            }

            if (Input.GetButton("J" + 1 + "A"))
            {
                menuAnimator.SetBool("Out", true); //Uso setBool porque el trigger decidio no andar
                sound.Reproducir(sound.ui.enterTotem);

                yield return StartCoroutine(GameManager.Instance.StartLevel("Lobby", delay: 3f));

                totemActual = 5;

            }

            yield return new WaitForEndOfFrame();

        }
        yield return null;
    }

    IEnumerator TotemQuit()
    {
        while (totemActual == 2)
        {
            if (rightArrow)
            {
                sound.Reproducir(sound.ui.moveTotem);

                totemActual = 3;
            }

            if (leftArrow)
            {
                sound.Reproducir(sound.ui.moveTotem);

                totemActual = 1;

            }

            if (Input.GetButtonDown("J" + 1 + "A"))
            {
                StartCoroutine(GameManager.Instance.SalirDelJuego());

                sound.Reproducir(sound.ui.enterTotem);


            }
            yield return new WaitForEndOfFrame();

        }
        yield return null;
    }

    IEnumerator TotemSettings()
    {
        while (totemActual == 3)
        {
            if (rightArrow)
            {
                sound.Reproducir(sound.ui.moveTotem);

                totemActual = 0;
            }

            if (leftArrow)
            {
                sound.Reproducir(sound.ui.moveTotem);

                totemActual = 2;

            }

            if (Input.GetButtonDown("J" + 1 + "A"))
            {
                //config. Parece que no llegue con las configuraciones
                sound.Reproducir(sound.ui.enterTotem);

            }
            yield return new WaitForEndOfFrame();

        }
        yield return null;
    }

    IEnumerator TotemHowToPlay()
    {
        while (totemActual == 1)
        {
            if (rightArrow)
            {
                totemActual = 2;
            }

            if (leftArrow)
            {
                totemActual = 0;

            }

            if (Input.GetButtonDown("J" + 1 + "A"))
            {
                sound.Reproducir(sound.ui.enterTotem);

            }
           yield return new WaitForEndOfFrame();

        }
        yield return null;
    }
}




