using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {


    private string lHorizontalAxis;
    private string lVerticalAxis;
    private string rHorizontalAxis;
    private string rVerticalAxis;
    private string aButton;
    private string bButton;
    private string yButton;
    private string xButton;
    private string r3;
    private string leftB;
    private string rightB;
    private string leftT;
    private string rightT;
    private int numeroJoystick;
    private string pausa;
    private string horizontalKeys;
    private string verticalKeys;


    public string RHorizontal { get { return rHorizontalAxis; } }
    

    public string RVertical { get { return rVerticalAxis; } }

    public string LHorizontal { get { return lHorizontalAxis; } }

    public string LVertical { get { return lVerticalAxis; } }

    public string R3 { get { return r3; } }

    public string Dance { get { return yButton; } }

    public string Utility { get { return aButton; } }

    public string Action { get { return xButton; } }

    public string Ultimate { get { return bButton; } }

    public string Pausa { get { return pausa; } }


    public bool leerInput = false;

    public float l;    // DEBUG DE RT Y LT
    public float r;   //
    Player player;
    PlayerController playerControl;
    public Hada hada;





    private void Awake()
    {
        player = this.GetComponent<Player>();
    }

    void Start() {

        AsignarNumeroJoystick(player.numeroJugador);


        //hadaMov = GetComponentInChildren<HadaMover>();

        playerControl = GetComponent<PlayerController>();


    }

    private void OnEnable()
    {

        hada = GetComponentInChildren<Hada>();
    }


    float x;
    float y;

    private void Update() {

        if (!leerInput) return;

        //Input Player Analogico Izquierdo

        x = Input.GetAxis(lHorizontalAxis);
        y = Input.GetAxis(lVerticalAxis);

        if (player.puedeMoverse)
            playerControl.MovimientoAnalogico(x,y);


        //Cambiar elemento del Hada

        l = Input.GetAxis(leftT);
        r = Input.GetAxis(rightT);

        if (l > 0.5f)
        {
            playerControl.HealFunc();
            //hada.elementoActual = Hada.Elementos.Earth;
        }
        else if (r > 0.5f)
        {
            playerControl.SprintFunc();
            //hada.elementoActual = Hada.Elementos.Air;
        }
        else if (Input.GetButtonDown(leftB))
        {
            playerControl.EarthWallFunc();
            //hada.elementoActual = Hada.Elementos.Water;
        }
        else if (Input.GetButtonDown(rightB))
        {
            playerControl.ExplosionFunc();
            //hada.elementoActual = Hada.Elementos.Fire;
        }
        else
            hada.elementoActual = Hada.Elementos.None;

        //Cambiar funcion de boton A segun elemento del hada
        if (Input.GetButtonDown(aButton))
        {
            playerControl.FuncionUtilidad();

        }

    }




    void AsignarNumeroJoystick(int numero)
    {
        numeroJoystick = numero;
        lHorizontalAxis = "J" + numeroJoystick + "LHorizontal";
        lVerticalAxis = "J" + numeroJoystick + "LVertical";
        rHorizontalAxis = "J" + numeroJoystick + "RHorizontal";
        rVerticalAxis = "J" + numeroJoystick + "RVertical";
        aButton = "J" + numeroJoystick + "A";
        bButton = "J" + numeroJoystick + "B";
        xButton = "J" + numeroJoystick + "X";
        yButton = "J" + numeroJoystick + "Y";
        r3 = "J" + numeroJoystick + "R3";
        leftB = "J" + numeroJoystick + "LB";
        rightB = "J" + numeroJoystick + "RB";
        leftT = "J" + numeroJoystick + "LT";
        rightT = "J" + numeroJoystick + "RT";
        pausa = "J"+ numeroJoystick+"Start";
        horizontalKeys = "J" + numeroJoystick + "ArrowsHorizontal";
        verticalKeys = "J" + numeroJoystick + "ArrowsVertical";


    }



    
   
}
