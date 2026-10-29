using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosion_force;
    public float up_force;
    public float radius;
    public bool hasExploded = false;

    public GameObject explosion_effects;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("player");
        Collider col = GetComponent<Collider>();
        Physics.IgnoreCollision(player.GetComponent<Collider>(), col, true);

    }
    void Explode()
    {
        hasExploded = true;
        if (explosion_effects != null) Instantiate(explosion_effects, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == player)
            {

                Rigidbody p_rb = player.GetComponent<Rigidbody>();
                p_rb.AddExplosionForce(explosion_force, transform.position, radius, up_force, ForceMode.Impulse);
                continue;
            }
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null) rb.AddExplosionForce(explosion_force, transform.position, radius, up_force, ForceMode.Impulse);
            Inhabitants inh = col.gameObject.GetComponent<Inhabitants>();
            if (inh != null) inh.dead = true;
        }
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player) return;
        if (!hasExploded)
        {
            Explode();
        }
    }
}
