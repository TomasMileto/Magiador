using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

    public ParticleSystem explosion;

    ParticleSystem fireBall;

    public float aceleracion=7;
    public float velocidadInicial=20;

    public float radioExplosion=5;
    public float fuerzaExplosion=200;
    public float damage=60;

    float t = 0;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(PrenderColision()); //Por las dudas, creo esto me hinchaba los huebos con la explosion

        fireBall = this.gameObject.GetComponent<ParticleSystem>();

        Destroy(gameObject, fireBall.main.duration * 1.15f);
    }

    IEnumerator PrenderColision()
    {
        yield return new WaitForSeconds(0.1f);

        GetComponent<Collider>().enabled = true;
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Explode();

        Destroy(gameObject,0.1f);


    }

     void Explode()
    {
        explosion.Emit(1);

        Collider[] colliders=Physics.OverlapSphere(transform.position, radioExplosion);

        foreach (Collider objetoCercano in colliders)
        {
            Rigidbody otherRB = objetoCercano.GetComponent<Rigidbody>();

            if (otherRB != null && otherRB.tag=="Player")
            {
                
                otherRB.AddExplosionForce(fuerzaExplosion, transform.position, radioExplosion);

                Player otherPlayer = otherRB.GetComponent<Player>();

                

                StartCoroutine(otherPlayer.TakeDamage(damage, "xd"));

            }

        }

        Destroy(gameObject, explosion.main.duration * 1.25f);
    }


  

    void Move()
    {
      

        float z = velocidadInicial * Time.deltaTime ;

        transform.Translate(Vector3.forward * z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
