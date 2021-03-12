using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCactus : MonoBehaviour {

    public float damage = 15;
	
	
    private void OnCollisionEnter(Collision other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            StartCoroutine(player.TakeDamage(damage));
        }
    }
}
