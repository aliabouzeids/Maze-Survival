using UnityEngine;


public class Shrine : MonoBehaviour
{

    GameObject player;
    [SerializeField] GameObject shrine;
    void Awake() => player = GameObject.Find("player");
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) shrine.SetActive(true);

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player) shrine.SetActive(false);
    }
}