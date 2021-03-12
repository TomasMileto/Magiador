using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public int numeroJugador;
    float vidaMax;
    float vidaPlayer;
    Player player;

    Image image;

    //Text porcentajeVida;

	void Start () {

        image = GetComponent<Image>();

        //porcentajeVida = GetComponentInChildren<Text>();

        player = PlayerManager.Instance.players[numeroJugador - 1];

        vidaMax = player.vida;


    }

    void Update () {

        if (player == null) return;
        if (image == null) return;

        vidaPlayer = player.vida;

        image.fillAmount = vidaPlayer / vidaMax;


	}

   
    
}
