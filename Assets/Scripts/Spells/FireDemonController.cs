using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDemonController : MonoBehaviour {

    public enum Estados { idle, fireBall, ataque};

    public Estados estadoActual;

    public GameObject fireBall;

    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public Player player;

    List<Player> objetivosAtaque;

    List<Player> objetivosFireBall;

    public float rangoFireBall=20;
    public float ratioFireBall=4.5f;
    public float fireChanneling=0.8f;
    public float rangoAtaque=8;
    public float ratioAtaque=1;

    public float damage = 20;

	void Start () {
        StartCoroutine(MaquinaPrincipal());

        objetivosAtaque = new List<Player>();
        objetivosFireBall = new List<Player>();
	}

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerManager.Instance.players[i] == this.player) continue;
            if (PlayerManager.Instance.PlayerExists(i + 1)==false) continue;

            Player p = PlayerManager.Instance.players[i];
            float distancia = Vector3.Distance(p.transform.position, transform.position);

            if (distancia < rangoAtaque)
            {
                if (!objetivosAtaque.Contains(p))
                {
                    objetivosAtaque.Add(p);
                }
                if (objetivosFireBall.Contains(p))
                {
                    objetivosFireBall.Remove(p);
                }
            }


            else if (distancia < rangoFireBall )
            {
                if (!objetivosFireBall.Contains(p))
                {
                    objetivosFireBall.Add(p);
                }

                if (objetivosAtaque.Contains(p))
                {
                    objetivosAtaque.Remove(p);
                }
            }

            else if(distancia >rangoFireBall)
            {
                if (objetivosFireBall.Contains(p))
                    objetivosFireBall.Remove(p);

            }
        } //VERIFICAR DISTANCIAS A PLAYER

        if (objetivosAtaque.Count > 0)
            estadoActual = Estados.ataque;

        else if (objetivosFireBall.Count > 0)
            estadoActual = Estados.fireBall;
        else
            estadoActual=Estados.idle;
    }
    IEnumerator MaquinaPrincipal()
    {
        while (!isDead)
        {
            switch (estadoActual)
            {
                case Estados.idle:

                    yield return new WaitForEndOfFrame();
                    break;

                case Estados.fireBall:
                    yield return StartCoroutine(FireBall());
                    break;

                case Estados.ataque:

                    yield return StartCoroutine(Atack());

                    yield return new WaitForEndOfFrame();
                    break;

            }
        }
        yield return null;
    }

    IEnumerator FireBall()
    {
        //anim

        Player objetivo=DeterminarObjetivo(objetivosFireBall);

        float count=0;

        while (count<fireChanneling)
        {
            count += Time.deltaTime;

            transform.LookAt(objetivo.transform);
            yield return new WaitForEndOfFrame();
        }

        //Vector3 spawnPoint = transform.position + transform.forward + transform.up*5;


        //GameObject fireBolt = Instantiate(fireBall, spawnPoint, gameObject.transform.rotation, transform);

        print("FIREBALL");
       

        yield return new WaitForSeconds(ratioFireBall);
    }

    IEnumerator Atack()
    {
        Player objetivo = DeterminarObjetivo(objetivosAtaque);
        //anim

        StartCoroutine(objetivo.TakeDamage(damage, player.name));

        yield return new WaitForSeconds(ratioAtaque);
    }

    Player DeterminarObjetivo(List<Player> list)
    {
        int i = Random.Range(0, list.Count-1);

        return list[i]; 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, rangoAtaque);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, rangoFireBall);


    }

}
