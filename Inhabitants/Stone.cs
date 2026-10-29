using UnityEngine;


public class Stone : MonoBehaviour
{


    void OnCollisionEnter(Collision collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        if (p != null) p.Get_Hit();
        Destroy(gameObject);
    }
}