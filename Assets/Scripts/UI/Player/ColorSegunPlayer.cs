using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSegunPlayer : MonoBehaviour {

    Player player;

    PlayerController playerControl;

    public ParticleSystem glow;

    Image imagen;

    Color color;

    public int numeroJugador;

    void Start () {

        player = PlayerManager.Instance.players[numeroJugador - 1].GetComponent<Player>();

        playerControl = PlayerManager.Instance.players[numeroJugador - 1].GetComponent<PlayerController>();

        imagen = GetComponent<Image>();

        color = player.colorPlayer;

        color.a = 0.65f;

        imagen.color = color;

        StartCoroutine(Brillar());
    }


    IEnumerator Brillar()
    {
            yield return new WaitUntil(() => playerControl.EOready == true);

        color.a = 1;

        glow.Play(true);

        imagen.color = color;

            yield return new WaitUntil(() => playerControl.EOready == false);

        color.a = 0.65f;

        glow.Stop(true);

        imagen.color = color;

        yield return new WaitForEndOfFrame();

        StartCoroutine(Brillar());

    }
    
}
