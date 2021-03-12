using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecuperacionUtility : MonoBehaviour {

    Image imagen;

    PlayerController playerControl;

    public int numeroJugador;

	void Start () {

        imagen = GetComponent<Image>();

        playerControl = PlayerManager.Instance.players[numeroJugador - 1].GetComponent<PlayerController>();

    }

    void Update () {

        if (playerControl == null) return;
        if (imagen == null) return;

        imagen.fillAmount = playerControl.recuperandoUtility / playerControl.cooldownUtility;
         
    }
}
