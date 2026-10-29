using UnityEngine;




public class Teleport_Point : MonoBehaviour
{
    [SerializeField] Transform point_to;
    Teleport p_tp;
    GameObject player;

    void Awake()
    {
        player = GameObject.Find("player");
        p_tp = player.GetComponent<Teleport>();
    }
    void Start()
    {
        point_to = player.GetComponent<Spawn_Random>().sp;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == p_tp.gameObject)
        {
            p_tp.TP(point_to.position);
            Player p = player.GetComponent<Player>();
            if (p != null) p.Get_Hit();

        }

    }
}