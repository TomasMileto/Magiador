using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCloud : MonoBehaviour {

    public float velocidad = 30;

    public float damage = 30;

    public float reduccionMovimiento=25;

    public float duracionDebuff=3.5f;

    [HideInInspector]
    public Player player;

	void Start () {

        Destroy(gameObject, 10);
	}
	
	void Update () {

        transform.position += transform.forward * velocidad * Time.deltaTime;

	}

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":

               
                Player otherPlayer = other.GetComponent<Player>();

                if (otherPlayer != player)
                {
                    StartCoroutine(otherPlayer.TakeDamage(damage, player.name));

                    StartCoroutine(otherPlayer.RalentizarMovimiento(duracion: duracionDebuff, reduccionPorcentaje: reduccionMovimiento));
                }
                break;

            case "Environment":
            case "Wall":
                Destroy(gameObject);
                break;

              
        }
    }
}
