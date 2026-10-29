using UnityEngine;


public class Player_Foot : MonoBehaviour
{
    GameObject player;
    float force;
    Collider p_col;
    void Awake()
    {
        player = GameObject.Find("player");
        p_col = player.GetComponent<Collider>();
        Player_Movement pm = player.GetComponent<Player_Movement>();
        force = pm.jump_force / 2;
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {

        Inhabitants inh = other.gameObject.GetComponent<Inhabitants>();
        if (inh == null) return;
        Physics.IgnoreCollision(p_col, inh.GetComponent<Collider>(), true);
        inh.dead = true;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}