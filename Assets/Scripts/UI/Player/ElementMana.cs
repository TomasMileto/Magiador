using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementMana : MonoBehaviour {

    public int numeroJugador;
    Player player;

    float waterMana;
    float fireMana;
    float earthMana;
    float airMana;

    public Image Water;
    public Image Fire;
    public Image Earth;
    public Image Air;
    
	void Start () {

        player = PlayerManager.Instance.players[numeroJugador - 1];

        if (! PlayerManager.Instance.PlayerExists(numeroJugador)) UIManager.Instance.ApagarUI(this.transform.parent.gameObject);

    }

    void Update () {

        if (player == null) return;

        waterMana = player.waterMana;
        Water.fillAmount = waterMana / 100;

        fireMana = player.fireMana;
        Fire.fillAmount = fireMana / 100;

        earthMana = player.earthMana;
        Earth.fillAmount = earthMana / 100;

        airMana = player.airMana;
        Air.fillAmount = airMana / 100;
	}
}
