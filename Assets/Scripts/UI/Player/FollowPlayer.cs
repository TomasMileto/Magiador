using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    Player player;

    public int numeroPlayer;

    void Start()
    {

        player = PlayerManager.Instance.players[numeroPlayer - 1];

        if (!PlayerManager.Instance.PlayerExists(numeroPlayer)) UIManager.Instance.ApagarUI(this.gameObject);

    }

	void Update () {

        if (player == null) return;

        if (player.transform.Find("Personaje") == null) return;
        transform.position = player.transform.Find("Personaje").transform.position + new Vector3(0, 7, 0);
	}
}
