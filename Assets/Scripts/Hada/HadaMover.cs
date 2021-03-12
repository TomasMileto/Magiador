using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadaMover : MonoBehaviour
{

    [Header("Estados")]
    Player player;

    public enum Estados { knocked, regresando, ataque, enIdle, }
    public Estados estadoActual = Estados.enIdle;
    Coroutine estadoCR;

    [Header("Movimiento")]
    //public float hadaMaxVel;
    public float fuerzaAtaque = 100;
    public float regresoSmooth = 20;
    float h = 0;
    float j = 0;
    int i = 0;
    public bool hadaHaVuelto;

    bool canDamage = false;


    Hada hada;
    Vector3 aMirar;

    [Header("Tipo Movimiento")]
    public Idle idle;


    [System.Serializable]
    public class Idle
    {

        public Transform target;

        public float distanciaMax;

        public float smoothing;

        public float timeCount;

    }



    //public Orbita orbita;

    //[System.Serializable]
    //public class Orbita
    //{
    //    public Transform target;



    //    public float velocidad = 10;

    //    public float radio;

    //    public float aumentoConsumo;

    //    public float damage;


    //}

    PlayerInput input;
    Rigidbody rb;




    private void OnEnable()
    {
        //OBTENER COMPONENTES
        //
        hada = GetComponent<Hada>();

        if (hada == null) return;

        player = PlayerManager.Instance.players[hada.numeroHada - 1];

        input = player.GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody>();



        StartCoroutine(InicioMaquinaDeEstado());

        //MISC
        //

        i = 0;

    }


    private void FixedUpdate()
    {
        //JOYSTICK ANALOGICO DERECHO INPUT

        //h = Input.GetAxis(input.RHorizontal);

        //j = Input.GetAxis(input.RVertical); 


    }

    private void Update()
    {
        // X BUTTON TOGGLE
        //

        transform.forward = aMirar; //Para rotar

        if (player == null) return;

        if (Input.GetButtonDown(input.Action))
        {
            i++;

            int switcher = i % 2;



            switch (switcher)
            {
                case 0:
                    estadoActual = Estados.regresando;
                    break;
                case 1:
                    estadoActual = Estados.ataque;
                    break;
            }

            StartCoroutine(InicioMaquinaDeEstado());
        }


    }




    public IEnumerator InicioMaquinaDeEstado()
    {
        Debug.Log("Inicio Maquina de Estado del Hada Mover");
        while (true)
        {
            //CAMBIAR ENTRE ESTADOS
            //
            switch (estadoActual)
            {
                case Estados.regresando:

                    //if (hada != null)
                    //    hada.CanDamage = true;

                    yield return estadoCR = StartCoroutine(Regresar(target: player.transform.position, distancia: idle.distanciaMax, estado: Estados.enIdle));

                    //hada.CanDamage = false;
                    break;

                case Estados.enIdle:

                    yield return estadoCR = StartCoroutine(IdleStand());
                    estadoActual = Estados.ataque;


                    break;

                case Estados.ataque:

                    yield return estadoCR = StartCoroutine(Ataque());

                    break;

                //case Estados.modoOrbita:

                //    yield return estadoCR = StartCoroutine(Regresar(target: orbita.target.position, distancia: orbita.radio, estado: Estados.modoOrbita));

                //    yield return estadoCR = StartCoroutine(Orbitando());

                //    break;

                case Estados.knocked:
                    yield return new WaitForEndOfFrame();
                    break;

                default:
                    yield return new WaitForEndOfFrame();
                    break;



            }
        }
    }
    //IEnumerator MovimientoHada()
    //{


    //    while (estadoActual == Estados.ataque)
    //    {

    //        print("Viajando");



    //        if (rb.velocity.x < hadaMaxVel)
    //        {

    //            rb.AddForce(PlayerManager.Instance.right * h * fuerzaMovimiento * Time.fixedDeltaTime);
    //        }  //Movimiento eje x

    //        if (rb.velocity.z < hadaMaxVel)
    //        {

    //            rb.AddForce(PlayerManager.Instance.forward * j * fuerzaMovimiento * Time.fixedDeltaTime);
    //        } //Movimiento eje z

    //        if (Input.GetButton(input.R3))
    //        {
    //            estadoActual = Estados.regresando;
    //            yield break;
    //        }



    //        if (h != 0 || j != 0)
    //            aMirar = Vector3.Normalize(PlayerManager.Instance.right * h + PlayerManager.Instance.forward * j); //Salvo el caso del analogico en reposo para no apuntar a Vector Nulo.

    //        //if (aMirar != new Vector3(0, 0, 0))
    //        //    transform.forward = aMirar; //Constantemente redefino rotacion

    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    [ContextMenu("Regresar")]
    public IEnumerator Regresar(Vector3 target, float distancia, Estados estado)
    {
        rb.velocity = Vector3.zero;
        while (Vector3.Distance(target, transform.position) > distancia)
        {
            print("Regresando");

            Vector3 currentVelocity = rb.velocity;
            rb.MovePosition(Vector3.SmoothDamp(transform.position, target, ref currentVelocity, regresoSmooth * 0.5f * Time.deltaTime));

            aMirar = target;


            yield return new WaitForFixedUpdate();
        }

        print("Llegue");

        estadoActual = estado;

    }

    [ContextMenu("Idle")]
    IEnumerator IdleStand()
    {

        while (estadoActual == Estados.enIdle)
        {
            print("Idle");

            Vector3 currentVelocity = rb.velocity;

            if (idle.target.position == null) ;
            rb.MovePosition(Vector3.SmoothDamp(transform.position, idle.target.position, ref currentVelocity, idle.smoothing * Time.deltaTime));

            transform.rotation = player.transform.rotation;


            yield return new WaitForFixedUpdate();

        }


        estadoActual = Estados.ataque;

        yield break;
    }

    [ContextMenu("Atack")]
    IEnumerator Ataque()
    {
        canDamage = true;

        rb.AddForce(player.transform.forward * fuerzaAtaque);



        while (canDamage == true)
        {
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Regresar(player.transform.position, idle.distanciaMax, Estados.enIdle));
    }



    //[ContextMenu("Orbita")]
    //IEnumerator Orbitando()
    //{


    //    Rigidbody targetRB = orbita.target.GetComponent<Rigidbody>();
    //    print(targetRB);

    //    orbita.radio = 5;

    //    float timer = 0;

    //    while (estadoActual == Estados.modoOrbita)
    //    {

    //        timer += Time.deltaTime;

    //        orbita.radio += j / 0.2f * Time.deltaTime;
    //        orbita.radio = Mathf.Clamp(orbita.radio, 3, 15);


    //        float x = Mathf.Sin(timer * orbita.velocidad/*/(masa*3)*/) * orbita.radio;
    //        float y = 0.1f;
    //        float z = Mathf.Cos(timer * orbita.velocidad/*/(masa*3)*/) * orbita.radio;

    //        Vector3 movCircular = new Vector3(x, y, z);

    //        transform.position = (orbita.target.position + movCircular);

    //        aMirar = movCircular + new Vector3(

    //            Mathf.Sin((timer + 0.8f) * orbita.velocidad) * orbita.radio,
    //            0.1f,
    //            Mathf.Cos((timer + 0.8f) * orbita.velocidad/*/(masa*3)*/) * orbita.radio

    //            );

    //        yield return new WaitForEndOfFrame();
    //    }
    //    yield return null;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (canDamage == true)
        {
            canDamage = false;

            if(collision.gameObject.tag=="Player")
            {
                Player otherPlayer = collision.gameObject.GetComponent<Player>();

                StartCoroutine(otherPlayer.TakeDamage(hada.oneShotDamage * rb.velocity.magnitude));

            }
        }
    }
}
