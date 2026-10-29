using UnityEngine;



public class Spawn_Random : MonoBehaviour
{
    public Transform[] sp_p;
    public Transform sp;
    void Awake()
    {
        int r = Random.Range(0, sp_p.Length);
        gameObject.SetActive(false);
        sp = sp_p[r];
        transform.position = sp_p[r].position;
        gameObject.SetActive(true);
    }
}